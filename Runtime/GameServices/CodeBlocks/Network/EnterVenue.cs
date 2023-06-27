using System.Threading.Tasks;
using Toolset;
using Unity.Netcode;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Enter Current Venue", menuName = "Code Blocks/Network/Enter Current Venue", order = 0)]
    public class EnterVenue : CodeBlock
    {
        private const int LOBBY_QUERY_TIMEOUT = 1000;

        [SerializeField] private GameObject networkTracker;
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
                    CreateRelayServer(true);
                    break;
                case NetState.Guest:
                    JoinRelayServer();
                    break;
                case NetState.Client:
                    JoinRelayServer();
                    break;
                case NetState.Host:
                    CreateRelayServer(true);
                    break;
                case NetState.Dedicated:
                    CreateRelayServer(false);
                    break;
                case NetState.Offline:
                    Complete(true);
                    break;
            }
        }

        private async void CreateRelayServer(bool asPlayer)
        {
            var connections = GameData.Get<int>(Key.LobbyMaxPlayers) - 1;
            var connected = await relayProvider.CreateServer(connections, asPlayer);

            Instantiate(networkTracker)
                .With(t => t.name = "Network Tracker")
                .With(t => t.GetComponent<NetworkObject>().Spawn());

            Complete(connected);
        }

        private async void JoinRelayServer()
        {
            GameData.Set(Key.CurrentLobbyCode, lobbyProvider.LobbyCode);
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
                {
                    JoinRelayServer();
                }
                else
                {
                    GameData.Set(Key.PlayerNetState, NetState.Host);
                    CreateRelayServer(true);
                }
            }
            else
            {
                GameData.Set(Key.PlayerNetState, NetState.Host);
                CreateRelayServer(true);
            }
        }
    }
}