using UnityEngine;
using Toolset;
using Unity.Netcode;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Disconnect From Server", menuName = "Code Blocks/Network/Disconnect", order = 0)]
    public class DisconnectFromServer : CodeBlock
    {
        protected override void Execute()
        {
            if (NetworkManager.Singleton != null)
            {
                var relay = Services.All.Single<IRelayProvider>();
                relay.StopServer();

                Destroy(NetworkManager.Singleton.gameObject);
            }

            Services.All.Single<ILobbyProvider>().LeaveConnectedLobby();

            Complete(true);
        }
    }
}