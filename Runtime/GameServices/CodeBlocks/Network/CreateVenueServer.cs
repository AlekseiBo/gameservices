using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Create Venue Server", menuName = "Code Blocks/Network/Create Venue Server", order = 0)]
    public class CreateVenueServer : CodeBlock
    {
        private IRelayProvider relayProvider;
        private IStaticDataService staticData;

        protected override void Execute()
        {
            relayProvider = Services.All.Single<IRelayProvider>();
            Complete(relayProvider.CreateNetworkManager() != null);
        }
    }
}