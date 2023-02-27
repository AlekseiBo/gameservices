using GameServices.Commands;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "WaitForVenueRequest", menuName = "Code Blocks/Communication/Wait For Venue Request", order = 0)]
    public class WaitForVenueRequest : CodeBlock
    {
        [SerializeField] private AssetReference menuAsset;

        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();

            if (GameData.Get<bool>(Key.RelayServer))
                Complete(true);
            else
                Start();
        }

        private async void Start()
        {
            var menu = await assets.Instantiate(menuAsset.AssetGUID);
            if (menu == null)
            {
                Complete(false);
                return;
            }

            Mediator.Subscribe<VenueSelected>(Selected);
        }

        private void Selected(VenueSelected c)
        {
            Mediator.RemoveSubscriber<VenueSelected>(Selected);
            Complete(true);
        }
    }
}