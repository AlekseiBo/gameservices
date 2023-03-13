using System;
using UnityEngine;
using Toolset;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            GameData.Set(Key.CurrentLobbyCode, "");

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

            var progress = Services.All.Single<IProgressProvider>();
            await progress.SaveProgress();

            var relayProvider = Services.All.Single<IRelayProvider>();
            relayProvider.StopServer();

            await LeaveConnectedLobby();

            var asServer = GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated;
            var owner = asServer ? "RELAY" : "PLAYER";
            var venue = GameData.Get<string>(Key.CurrentVenue);
            var lobbyName = $"{owner} {venue}";
            var maxPlayers = GameData.Get<int>(Key.LobbyMaxPlayers);
            var lobbyData = new CreateLobbyData(lobbyName, maxPlayers, false);
            await CreateLobby(lobbyData);

            var assets = Services.All.Single<IAssetProvider>();
            await assets.LoadScene(GameData.Get<string>(Key.CurrentVenue));

            progress.LoadProgress();

            await relayProvider.CreateServer(maxPlayers - 1, !asServer);

            Command.Publish(new LogMessage(LogType.Log, "Lobby restarted due to activity timer"));
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
                LogServerData();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                Command.Publish(new RestartLobby());
            }
        }

        private IEnumerator RunHeartbeat(float timeout)
        {
            var checkRelayServerActivity = GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated;
            var activityTimeout = GameData.Get<float>(Key.ServerActivityTimer) * 60f;
            var activityTimer = Time.time;

            while (hostedLobby != null)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(hostedLobby.Id);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log($"SendHeartbeatPingAsync: {e.Message}");
                    Command.Publish(new RestartLobby());
                }

                if (checkRelayServerActivity && activityTimer + activityTimeout < Time.time)
                {
                    activityTimer = Time.time;
                    CheckLobbyActivity();
                }

                yield return Utilities.WaitFor(timeout);
            }
        }

        private async void CheckLobbyActivity()
        {
            try
            {
                hostedLobby = await LobbyService.Instance.GetLobbyAsync(hostedLobby.Id);
                if (hostedLobby.Players.Count <= 1)
                    Command.Publish(new RestartLobby());
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"GetLobbyAsync: {e.Message}");
                Command.Publish(new RestartLobby());
            }
        }

        private void LogServerData()
        {
            var output = $"Server Name: {hostedLobby.Name}\n";
            output += $"Player ID: {playerId}\n";
            output += $"Venue ID: {Venue}\n";
            output += $"Lobby ID: {hostedLobby.Id}\n";
            output += $"Lobby Code: {LobbyCode}\n";
            output += $"Max Players: {hostedLobby.MaxPlayers}\n";
            output += $"Is Private: {hostedLobby.IsPrivate}\n";
            output += $"Is Locked: {hostedLobby.IsLocked}\n";
            output += $"Relay Server Code: {RelayCode}\n";
            output += $"Created Time: {hostedLobby.Created}\n";

            Command.Publish(new ShowMessage("Server Data", "<align=\"left\">" + output));
            Debug.Log(output);
        }
    }
}