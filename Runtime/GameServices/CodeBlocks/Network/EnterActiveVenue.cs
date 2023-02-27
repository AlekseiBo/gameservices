using System.Threading.Tasks;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "EnterActiveVenue", menuName = "Code Blocks/Network/Enter Active Venue", order = 0)]
    public class EnterActiveVenue : CodeBlock
    {
        [SerializeField] private int attempts = 5;

        private IStaticDataService staticData;
        private ILobbyProvider lobbyProvider;
        private IRelayProvider relayProvider;

        protected override void Execute()
        {
            lobbyProvider = Services.All.Single<ILobbyProvider>();
            relayProvider = Services.All.Single<IRelayProvider>();
            staticData = Services.All.Single<IStaticDataService>();

            var venueData = staticData.ForVenue(GameData.Get<string>(Key.ActiveVenue));

            if (venueData == null)
            {
                Complete(false);
            }
            else
            {
                Instantiate(venueData.NetworkManager).With(e => e.name = "Network Manager");

                var code = GameData.Get<string>(Key.LobbyCode);
                if (code != "")
                    EnterVenue(code);
                else
                    EnterPublicVenue(venueData.Address);

            }
        }

        private async void EnterVenue(string code)
        {
            var joinedLobby = await lobbyProvider.JoinLobby(code);
            var relayCode = lobbyProvider.RelayCode;
            var connected = await relayProvider.JoinServer(relayCode);
            GameData.Set(Key.LobbyCode, "");
            Complete(connected);
        }

        private async void EnterPublicVenue(string address)
        {
            var connected = false;
            var attempt = 1;

            while (!connected && attempt <= attempts)
            {
                if (GameData.Get<bool>(Key.RelayServer) || GameData.Get<bool>(Key.PlayerHost))
                {
                    connected = await CreatLobby();
                }
                else
                {
                    var joinedLobby = await lobbyProvider.JoinPublicLobby(attempt, address);
                    connected = joinedLobby == null ? await CreatLobby() : await JoinLobby();
                }

                Runner.LogMessage($"Entering lobby attempt {attempt} result: {connected}");

                if (!connected) await Task.Delay(1000);
                attempt++;
            }

            Complete(connected);
        }

        private async Task<bool> JoinLobby()
        {
            var code = lobbyProvider.RelayCode;
            return await relayProvider.JoinServer(code);
        }

        private async Task<bool> CreatLobby()
        {
            var hostedLobby = await lobbyProvider.CreateLobby();
            return hostedLobby != null && await relayProvider.CreateServer();
        }
    }
}