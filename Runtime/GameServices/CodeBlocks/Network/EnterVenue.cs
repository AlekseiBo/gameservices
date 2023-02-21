using System.Linq.Expressions;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "EnterVenue", menuName = "Code Blocks/Network/EnterVenue", order = 0)]
    public class EnterVenue : CodeBlock
    {
        [SerializeField] private int attempts = 5;

        private ILobbyProvider lobbyProvider;
        private IRelayProvider relayProvider;

        protected override void Execute()
        {
            lobbyProvider = Services.All.Single<ILobbyProvider>();
            relayProvider = Services.All.Single<IRelayProvider>();

            Enter();
        }

        private async void Enter()
        {
            var connected = false;
            var attempt = 1;

            while (!connected && attempt <= attempts)
            {
                var joinedLobby = await lobbyProvider.JoinPublicLobby(attempt);
                if (joinedLobby != null)
                {
                    var code = lobbyProvider.GetRelayCode();
                    connected = await relayProvider.JoinRelay(code);
                }
                else
                {
                    var hostedLobby = await lobbyProvider.CreateLobby();
                    connected = hostedLobby != null && await relayProvider.CreateRelayServer();
                }

                Runner.LogMessage($"Entering lobby attempt {attempt} result: {connected}");
                attempt++;
            }

            Complete(connected);
        }
    }
}