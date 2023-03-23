using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Enter Lobby", menuName = "Code Blocks/Network/Enter Lobby", order = 0)]
    public class EnterLobby : CodeBlock
    {
        private ILobbyProvider lobby;

        protected override void Execute()
        {
            lobby = Services.All.Single<ILobbyProvider>();

            Command.Publish(new LogMessage(LogType.Log, "Connecting to lobby"));


            switch (GameData.Get<NetState>(Key.PlayerNetState))
            {
                case NetState.Private:
                    Complete(true);
                    break;
                case NetState.Guest:
                    JoinWithCode();
                    break;
                case NetState.Client:
                    JoinWithVenue();
                    break;
                case NetState.Host:
                    CreateLobby(false);
                    break;
                case NetState.Dedicated:
                    CreateLobby(true);
                    break;
            }
        }

        private async void JoinWithCode()
        {
            Debug.Log(GameData.Get<string>(Key.RequestedLobbyCode));
            var joinedLobby = await lobby.JoinLobbyByCode(GameData.Get<string>(Key.RequestedLobbyCode));
            GameData.Set(Key.RequestedVenue, lobby.Venue);
            GameData.Set(Key.RequestedLobbyCode, "");
            Complete(joinedLobby != null);
        }

        private async void JoinWithVenue()
        {
            var joinedLobby = await lobby.JoinLobbyByVenue(GameData.Get<string>(Key.RequestedVenue));

            if (joinedLobby != null)
            {
                Complete(true);
            }
            else
            {
                GameData.Set(Key.PlayerNetState, NetState.Host);
                CreateLobby(false);
            }
        }

        private async void CreateLobby(bool asServer)
        {
            var owner = asServer ? "RELAY" : "PLAYER";
            var venue = GameData.Get<string>(Key.RequestedVenue);
            var lobbyName = $"{owner} ";
            var maxPlayers = GameData.Get<int>(Key.LobbyMaxPlayers);
            var lobbyData = new CreateLobbyData(lobbyName, venue, maxPlayers, false);
            var createdLobby = await lobby.CreateLobby(lobbyData);
            Complete(createdLobby != null);
        }
    }
}