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
            staticData = Services.All.Single<IStaticDataService>();

            var venueData = staticData.ForVenue(GameData.Get<string>(Key.CurrentVenue));

            if (venueData == null)
            {
                Complete(false);
                return;
            }

            relayProvider.StopServer();
            Instantiate(venueData.NetworkManager).With(e => e.name = "Network Manager");
            Complete(true);
        }
    }
}