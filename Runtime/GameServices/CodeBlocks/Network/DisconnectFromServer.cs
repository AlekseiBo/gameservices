using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Disconnect From Server", menuName = "Code Blocks/Network/Disconnect", order = 0)]
    public class DisconnectFromServer : CodeBlock
    {
        protected override async void Execute()
        {
            Services.All.Single<IRelayProvider>().StopServer();
            await Services.All.Single<ILobbyProvider>().LeaveConnectedLobby();
            Complete(true);
        }
    }
}