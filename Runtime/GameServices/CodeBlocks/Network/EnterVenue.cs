using System.Threading.Tasks;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Enter Current Venue", menuName = "Code Blocks/Network/Enter Current Venue", order = 0)]
    public class EnterVenue : CodeBlock
    {
        private const int LOBBY_QUERY_TIMEOUT = 1000;

        [SerializeField] private int attempts = 5;

        private IStaticDataService staticData;
        private ILobbyProvider lobbyProvider;
        private IRelayProvider relayProvider;
        private int attemptCounter;

        protected override void Execute()
        {
            lobbyProvider = Services.All.Single<ILobbyProvider>();
            relayProvider = Services.All.Single<IRelayProvider>();
            attemptCounter = 1;

            switch (GameData.Get<NetState>(Key.PlayerNetState))
            {
                case NetState.Private:
                    CreatRelayServer(true);
                    break;
                case NetState.Guest:
                    JoinRelayServer();
                    break;
                case NetState.Client:
                    JoinRelayServer();
                    break;
                case NetState.Host:
                    CreatRelayServer(true);
                    break;
                case NetState.Dedicated:
                    CreatRelayServer(false);
                    break;
            }
        }

        private async void CreatRelayServer(bool asHost)
        {
            var connections = GameData.Get<int>(Key.LobbyMaxPlayers) - 1;
            var connected = await relayProvider.CreateServer(connections, asHost);
            Complete(connected);
        }

        private async void JoinRelayServer()
        {
            var relayCode = lobbyProvider.RelayCode;
            var connected = await relayProvider.JoinServer(relayCode);

            if (connected)
                Complete(true);
            else
                EnterPublicVenue(++attemptCounter);
        }

        private async void EnterPublicVenue(int attempt)
        {
            await Task.Delay(LOBBY_QUERY_TIMEOUT);
            var venue = GameData.Get<string>(Key.CurrentVenue);

            if (attempt <= attempts)
            {
                var joinedLobby = await lobbyProvider.JoinLobbyByVenue(venue, attempt);
                if (joinedLobby != null)
                    JoinRelayServer();
                else
                    CreatRelayServer(true);
            }
            else
            {
                CreatRelayServer(true);
            }
        }
    }
}