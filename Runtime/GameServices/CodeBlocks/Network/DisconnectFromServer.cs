using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Disconnect From Server", menuName = "Code Blocks/Network/Disconnect", order = 0)]
    public class DisconnectFromServer : CodeBlock
    {
        [SerializeField] private bool restartGame = true;

        protected override async void Execute()
        {
            Services.All.Single<IRelayProvider>().StopServer();
            await Services.All.Single<ILobbyProvider>().LeaveConnectedLobby();
            if (restartGame) Command.Publish(new RunGame());
            Complete(true);
        }
    }
}