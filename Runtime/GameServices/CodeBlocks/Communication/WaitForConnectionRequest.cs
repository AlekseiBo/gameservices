using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Wait For Connection Request", menuName = "Code Blocks/Communication/Wait For Connection Request", order = 0)]
    public class WaitForConnectionRequest : CodeBlock
    {
        protected override void Execute()
        {
            var requestedVenue = GameData.Get<string>(Key.RequestedVenue);
            if (!string.IsNullOrEmpty(requestedVenue))
            {
                Complete(true);
            }
            else
            {
                WaitForConnection();
            }
        }

        private void WaitForConnection()
        {
            Command.Publish(new SelectVenue());
            Command.Subscribe<ConnectToLobby>(Requested);
        }

        private void Requested(ConnectToLobby c)
        {
            Command.RemoveSubscriber<ConnectToLobby>(Requested);
            Complete(true);
        }
    }
}