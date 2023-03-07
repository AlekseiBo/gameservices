using System;
using UnityEngine;
using Toolset;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public class LobbyProvider : ILobbyProvider, IDisposable
    {
        private const float HEARTBEAT_TIMEOUT = 10f;
        private const string RELAY_CODE = "RELAY_CODE";
        private const string VENUE = "VENUE";

        public string LobbyCode => joinedLobby?.LobbyCode;
        public string RelayCode => joinedLobby?.Data.GetValueOrDefault(RELAY_CODE).Value;
        public string Venue => joinedLobby?.Data.GetValueOrDefault(VENUE).Value;

        private string playerId => AuthenticationService.Instance?.PlayerId;

        private CoroutineRunner coroutine;
        private Coroutine heartbeatCoroutine;
        private Lobby hostedLobby;
        private Lobby joinedLobby;

        public void Dispose() => Command.RemoveSubscriber<AllocateRelayServer>(OnRelayServerAllocated);

        public async Task<Lobby> CreateLobby(CreateLobbyData data)
        {
            var lobbyOptions = new CreateLobbyOptions { IsPrivate = data.IsPrivate };

            try
            {
                hostedLobby = await LobbyService.Instance.CreateLobbyAsync(data.Name, data.MaxPlayers, lobbyOptions);
                heartbeatCoroutine = CoroutineRunner.Start(RunHeartbeat(HEARTBEAT_TIMEOUT));
                joinedLobby = hostedLobby;
                Debug.Log($"Lobby created: {hostedLobby.Name}");
                Command.Subscribe<AllocateRelayServer>(OnRelayServerAllocated);
                return hostedLobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return default;
            }
        }

        public async Task<Lobby> JoinLobbyByVenue(string venue, int attempt = 1)
        {
            try
            {
                var lobbies = await ListLobbies(venue);
                if (lobbies.Results.Count < attempt) return default;

                var lobby = lobbies.Results[attempt - 1];
                joinedLobby = await JoinOrReconnectLobby(lobby);
                Debug.Log($"Lobby {joinedLobby.Name} joined (Hosted by {joinedLobby.HostId})");
                return joinedLobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return default;
            }
        }

        public async Task<Lobby> JoinLobbyByCode(string code)
        {
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
                Debug.Log($"Lobby {joinedLobby.Name} joined (Hosted by {joinedLobby.HostId}");
                return joinedLobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                var lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
                if (lobbies is { Count: > 0 })
                    joinedLobby = await LobbyService.Instance.ReconnectToLobbyAsync(lobbies[0]);

                return joinedLobby;
            }
        }

        public async Task LeaveConnectedLobby()
        {
            if (hostedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.DeleteLobbyAsync(hostedLobby.Id);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e.Message);
                }

                Command.RemoveSubscriber<AllocateRelayServer>(OnRelayServerAllocated);
                CoroutineRunner.Stop(heartbeatCoroutine);
                Debug.Log($"Player {playerId} deleted lobby {hostedLobby.Name}");
                hostedLobby = null;
                joinedLobby = null;
            }
            else if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e.Message);
                }

                Debug.Log($"Player {playerId} left lobby {joinedLobby.Name}");
                joinedLobby = null;
            }
        }

        private async Task<Lobby> JoinOrReconnectLobby(Lobby lobby)
        {
            try
            {
                var player = lobby.Players.Find(p => p.Id == playerId);
                return player == null
                    ? await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id)
                    : await LobbyService.Instance.ReconnectToLobbyAsync(lobby.Id);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return default;
            }
        }

        private async Task<QueryResponse> ListLobbies(string venue)
        {
            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                        new(QueryFilter.FieldOptions.Name, venue, QueryFilter.OpOptions.CONTAINS)
                    },
                    Order = new List<QueryOrder>
                        { new(true, QueryOrder.FieldOptions.Created) }
                };

                var response = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

                Debug.Log($"Current lobbies: {response.Results.Count}");
                foreach (var lobby in response.Results)
                    Debug.Log($"{lobby.Name}: privates = {lobby.IsPrivate}, max players = {lobby.MaxPlayers}");

                return response;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return default;
            }
        }

        private async void RestartLobby()
        {
            Debug.Log($"Restarting the lobby");
            await LeaveConnectedLobby();
            var relayProvider = Services.All.Single<IRelayProvider>();
            relayProvider.StopServer();

            var asServer = GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated;
            var owner = asServer ? "RELAY" : "PLAYER";
            var venue = GameData.Get<string>(Key.CurrentVenue);
            var lobbyName = $"{owner} {venue}";
            var maxPlayers = GameData.Get<int>(Key.LobbyMaxPlayers);
            var lobbyData = new CreateLobbyData(lobbyName, maxPlayers, false);
            await CreateLobby(lobbyData);
            await relayProvider.CreateServer(maxPlayers - 1, !asServer);
        }

        private async void OnRelayServerAllocated(AllocateRelayServer server)
        {
            Debug.Log($"Updating relay server join code");

            var venue = GameData.Get<string>(Key.CurrentVenue);
            var lobbyOptions = new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, server.JoinCode) },
                    { VENUE, new DataObject(DataObject.VisibilityOptions.Public, venue) }
                }
            };

            try
            {
                hostedLobby = await LobbyService.Instance.UpdateLobbyAsync(hostedLobby.Id, lobbyOptions);
                joinedLobby = hostedLobby;
                GameData.Set(Key.CurrentLobbyCode, hostedLobby.LobbyCode);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                RestartLobby();
            }
        }

        private IEnumerator RunHeartbeat(float timeout)
        {
            while (hostedLobby != null)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(hostedLobby.Id);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e.Message);
                    RestartLobby();
                }

                yield return Utilities.WaitFor(timeout);
            }
        }
    }
}