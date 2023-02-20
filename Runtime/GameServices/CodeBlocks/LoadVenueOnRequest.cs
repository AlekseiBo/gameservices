using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadVenueOnRequest", menuName = "Code Blocks/Asset Management/Load Venue On Request", order = 0)]
    public class LoadVenueOnRequest : CodeBlock
    {
        private IAssetProvider assets;

        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            OnLoadRequest();
        }

        private async void OnLoadRequest()
        {
            var sceneAsset = GameData<Key>.Get<string>(Key.SelectedVenue);
            await assets.LoadScene(sceneAsset);

            if (Runner != null)
                Complete(true);
        }
    }
}