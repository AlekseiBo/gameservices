using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Wait For Authentication",
        menuName = "Code Blocks/Communication/Wait For Authentication", order = 0)]
    public class WaitForAuthentication : CodeBlock
    {
        protected override void Execute()
        {
            var netState = GameData.Get<NetState>(Key.PlayerNetState);
            if (netState == NetState.Dedicated)
            {
                Complete(true);
                return;
            }

            Command.Publish(new ShowAuthentication());
            Command.Subscribe<CompleteAuthentication>(Requested);
        }

        private void Requested(CompleteAuthentication c)
        {
            Command.RemoveSubscriber<CompleteAuthentication>(Requested);
            Complete(true);
        }
    }
}