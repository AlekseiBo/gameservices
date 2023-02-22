using System.Linq.Expressions;
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
                Instantiate(venueData.NetworkManager);
                EnterVenue(venueData.Address);
            }
        }

        private async void EnterVenue(string address)
        {
            var connected = false;
            var attempt = 1;

            while (!connected && attempt <= attempts)
            {
                var joinedLobby = await lobbyProvider.JoinPublicLobby(attempt, address);
                if (joinedLobby == null || GameData.Get<bool>(Key.RelayServer))
                {
                    var hostedLobby = await lobbyProvider.CreateLobby();
                    connected = hostedLobby != null && await relayProvider.CreateRelayServer();
                }
                else
                {
                    var code = lobbyProvider.GetRelayCode();
                    connected = await relayProvider.JoinRelay(code);
                }

                Runner.LogMessage($"Entering lobby attempt {attempt} result: {connected}");
                attempt++;
            }

            Complete(connected);
        }
    }
}