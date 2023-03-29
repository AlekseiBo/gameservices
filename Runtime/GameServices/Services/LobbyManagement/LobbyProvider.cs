using System;
using UnityEngine;
using Toolset;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public class LobbyProvider : ILobbyProvider, IDisposable
    {
        private const string LOBBY_CODE = "LOBBY_CODE";
        private const string RELAY_CODE = "RELAY_CODE";
        private const string VENUE = "VENUE";

        public string LobbyCode => joinedLobby?.LobbyCode;
        public string RelayCode => joinedLobby?.Data.GetValueOrDefault(RELAY_CODE).Value;
        public string Venue => joinedLobby?.Data.GetValueOrDefault(VENUE).Value;
        public Lobby JoinedLobby => joinedLobby;

        private string playerId => AuthenticationService.Instance?.PlayerId;

        private readonly LobbyRoutine routine;
        private readonly LobbyQuery query;
        private readonly LobbyRequest request;
        private Lobby hostedLobby;
        private Lobby joinedLobby;

        public LobbyProvider()
        {
            routine = new LobbyRoutine();
            query = new LobbyQuery();
            request = new LobbyRequest();

            Command.Subscribe<AllocateRelayServer>(OnRelayServerAllocated);
            Command.Subscribe<UpdatePlayerAllocation>(OnPlayerAllocated);
        }

        public void Dispose()
        {
            routine.Stop();
            Command.RemoveSubscriber<AllocateRelayServer>(OnRelayServerAllocated);
            Command.RemoveSubscriber<UpdatePlayerAllocation>(OnPlayerAllocated);
        }

        public async Task<Lobby> CreateLobby(CreateLobbyData data)
        {
            hostedLobby = await request.CreateLobby(data);
            if (hostedLobby == null) return null;

            routine.Start(hostedLobby);
            joinedLobby = hostedLobby;
            Debug.Log($"Lobby created: {hostedLobby.Name} (Venue: {Venue})");

            return hostedLobby;
        }

        public async Task<Lobby> JoinLobbyByVenue(string venue, int attempt = 1)
        {
            var lobbies = await query.ByVenue(venue);
            if (lobbies.Results.Count < attempt) return null;

            var lobby = lobbies.Results[attempt - 1];
            joinedLobby = await request.JoinOrReconnectLobby(lobby, playerId);
            Debug.Log($"Lobby {joinedLobby.Name} joined (Hosted by {joinedLobby.HostId})");
            return joinedLobby;
        }

        public async Task<Lobby> JoinLobbyByCode(string code)
        {
            joinedLobby = await request.JoinLobbyByCode(code);
            if (joinedLobby == null)
            {
                var lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
                if (lobbies is { Count: > 0 })
                    joinedLobby = await request.JoinLobby(lobbies[0]);

                return joinedLobby;
            }

            Debug.Log($"Lobby {joinedLobby.Name} joined (Hosted by {joinedLobby.HostId}");
            return joinedLobby;
        }

        public async Task LeaveConnectedLobby()
        {
            GameData.Set(Key.CurrentLobbyCode, "");

            if (hostedLobby != null)
            {
                await request.DeleteLobby(hostedLobby.Id);
                routine.Stop();
                Debug.Log($"Player {playerId} deleted lobby {hostedLobby.Name}");
                hostedLobby = null;
                joinedLobby = null;
            }
            else if (joinedLobby != null)
            {
                await request.LeaveLobby(joinedLobby.Id, playerId);
                Debug.Log($"Player {playerId} left lobby {joinedLobby.Name}");
                joinedLobby = null;
            }
        }

        public async Task UpdateVenue(string venue)
        {
            Debug.Log($"Updating venue on change");

            if (hostedLobby == null) return;

            var lobbyOptions = new UpdateLobbyOptions
            { Data = new Dictionary<string, DataObject>
                { { VENUE, new DataObject(DataObject.VisibilityOptions.Public, venue, DataObject.IndexOptions.S1) } } };

            hostedLobby = await request.UpdateLobby(hostedLobby.Id, lobbyOptions);
            if (hostedLobby == null)
            {
                Command.Publish(new UpdateVenue(VenueAction.Exit, GameData.Get<string>(Key.CurrentVenue)));
                return;
            }

            joinedLobby = hostedLobby;
        }

        public async Task<Lobby> QueryPlayerOnline(string friendId)
        {
            var lobbies = await query.ByFriend(friendId);
            if (lobbies != null && lobbies.Results.Count > 0) return lobbies.Results[0];
            return null;
        }

        private async void OnRelayServerAllocated(AllocateRelayServer server)
        {
            Debug.Log($"Updating relay server join code");

            var venue = GameData.Get<string>(Key.CurrentVenue);
            var lobbyOptions = new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { VENUE, new DataObject(DataObject.VisibilityOptions.Public, venue, DataObject.IndexOptions.S1) },
                    { LOBBY_CODE, new DataObject(DataObject.VisibilityOptions.Public, LobbyCode) },
                    { RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, server.JoinCode) }
                }
            };

            hostedLobby = await request.UpdateLobby(hostedLobby.Id, lobbyOptions);
            if (hostedLobby == null)
            {
                Command.Publish(new UpdateVenue(VenueAction.Exit, GameData.Get<string>(Key.CurrentVenue)));
                return;
            }

            joinedLobby = hostedLobby;

            GameData.Set(Key.CurrentLobbyCode, hostedLobby.LobbyCode);
            LogServerData();

            var allocation = server.Allocation.AllocationId.ToString();
            OnPlayerAllocated(new UpdatePlayerAllocation(allocation));
        }

        private async void OnPlayerAllocated(UpdatePlayerAllocation updatePlayer)
        {
            if (joinedLobby == null) return;
            var options = new UpdatePlayerOptions { AllocationId = updatePlayer.Allocation };
            joinedLobby = await request.UpdateLobbyPlayer(joinedLobby.Id, playerId, options);
            if (joinedLobby == null) return;

            joinedLobby.Players.ForEach(p => Debug.Log($"PlayerId: {p.Id}, Allocation: {p.AllocationId}"));
        }

        private void LogServerData()
        {
            var output = $"Server Name: {hostedLobby.Name}\n";
            output += $"Host ID: {hostedLobby.HostId}\n";
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

        // private async void RestartLobby()
        // {
        //     Debug.Log($"Restarting the lobby");
        //
        //     var progress = Services.All.Single<IProgressProvider>();
        //     await progress.SaveProgress();
        //
        //     var relayProvider = Services.All.Single<IRelayProvider>();
        //     relayProvider.StopServer();
        //
        //     await LeaveConnectedLobby();
        //
        //     var asServer = GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated;
        //     var owner = asServer ? "RELAY" : "PLAYER";
        //     var venue = GameData.Get<string>(Key.CurrentVenue);
        //     var lobbyName = $"{owner} {venue}";
        //     var maxPlayers = GameData.Get<int>(Key.LobbyMaxPlayers);
        //     var lobbyData = new CreateLobbyData(lobbyName, maxPlayers, false);
        //     await CreateLobby(lobbyData);
        //
        //     var assets = Services.All.Single<IAssetProvider>();
        //     await assets.LoadScene(GameData.Get<string>(Key.CurrentVenue));
        //
        //     progress.LoadProgress();
        //
        //     await relayProvider.CreateServer(maxPlayers - 1, !asServer);
        //
        //     Command.Publish(new LogMessage(LogType.Log, "Lobby restarted due to activity timer"));
        // }
    }
}