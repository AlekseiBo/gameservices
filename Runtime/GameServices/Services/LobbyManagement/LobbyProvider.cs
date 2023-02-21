using UnityEngine;
using Toolset;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameServices.Commands;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public class LobbyProvider : ILobbyProvider
    {
        private const float HEARTBEAT_TIMEOUT = 10f;
        private const string RELAY_CODE = "RELAY_CODE";
        private string playerId => AuthenticationService.Instance.PlayerId;

        private readonly IRelayProvider relayProvider;
        private readonly MonoBehaviour runner;
        private Coroutine heartbeatCoroutine;
        private Lobby hostedLobby;
        private Lobby joinedLobby;

        public LobbyProvider(MonoBehaviour runner, IRelayProvider relayProvider)
        {
            this.runner = runner;
            this.relayProvider = relayProvider;
        }

        public async Task<Lobby> CreateLobby(bool isPrivate = false)
        {
            var owner = GameData<Key>.Get<bool>(Key.RelayServer) ? "RELAY" : "USER";
            var lobbyName = $"{owner} {playerId}";
            var maxPlayers = GameData.Get<int>(Key.MaxPlayers);
            var lobbyOptions = new CreateLobbyOptions { IsPrivate = isPrivate };

            try
            {
                hostedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);
                heartbeatCoroutine = this.runner.StartCoroutine(RunHeartbeat(HEARTBEAT_TIMEOUT));
                joinedLobby = hostedLobby;
                Debug.Log($"Lobby created: {hostedLobby.Name}");
                Mediator.Subscribe<RelayServerAllocated>(OnRelayServerAllocated);
                return hostedLobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }



        public async Task<Lobby> JoinPublicLobby(int attempt = 1, string venue = "")
        {
            try
            {
                var lobbies = await ListLobbies();
                if (lobbies.Results.Count < attempt) return null;

                var lobbyId = lobbies.Results[attempt - 1].Id;
                joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                Debug.Log($"Lobby {joinedLobby.Name} joined (Hosted by {joinedLobby.HostId}");
                return joinedLobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public string GetRelayCode() => joinedLobby.Data.GetValueOrDefault(RELAY_CODE).Value;

        public async void LeaveConnectedLobby()
        {
            if (hostedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                    hostedLobby = null;
                    joinedLobby = null;
                    runner.StopCoroutine(heartbeatCoroutine);
                    Debug.Log($"Player {playerId} deleted lobby {joinedLobby.Name}");
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e.Message);
                }
            }
            else if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                    joinedLobby = null;
                    Debug.Log($"Player {playerId} left lobby {joinedLobby.Name}");
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        private async Task<QueryResponse> ListLobbies()
        {
            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                        { new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) },
                    Order = new List<QueryOrder>
                        { new(true, QueryOrder.FieldOptions.Created) }
                };

                var response = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

                Debug.Log($"Current lobbies: {response.Results.Count}");

                foreach (var lobby in response.Results)
                {
                    Debug.Log($"{lobby.Name}: privates = {lobby.IsPrivate}, max players = {lobby.MaxPlayers}");
                }

                return response;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        private async void OnRelayServerAllocated(RelayServerAllocated server)
        {
            var updateOptions = new UpdatePlayerOptions
            {
                AllocationId = server.Allocation.AllocationId.ToString(),
                ConnectionInfo = server.JoinCode
            };

            var lobbyOptions = new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>()
                    { { RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, server.JoinCode) } }
            };

            try
            {
                await LobbyService.Instance.UpdatePlayerAsync(hostedLobby.Id, playerId, updateOptions);
                hostedLobby = await LobbyService.Instance.UpdateLobbyAsync(hostedLobby.Id, lobbyOptions);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
            }
        }

        private IEnumerator RunHeartbeat(float timeout)
        {
            while (hostedLobby != null)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(hostedLobby.Id);
                yield return Utilities.WaitFor(timeout);
            }
        }
    }
}