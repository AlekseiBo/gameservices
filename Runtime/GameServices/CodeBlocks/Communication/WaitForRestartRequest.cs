using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Wait For Restart Request", menuName = "Code Blocks/Communication/Wait For Restart Request", order = 0)]
    public class WaitForRestartRequest : CodeBlock
    {
        protected override void Execute()
        {
            Command.Subscribe<RestartLobby>(Requested);
        }

        private void Requested(RestartLobby restart)
        {
            Command.RemoveSubscriber<RestartLobby>(Requested);
            Complete(true);
        }
    }
}