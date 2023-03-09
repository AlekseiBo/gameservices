using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Load Current Venue", menuName = "Code Blocks/Assets/Load Current Venue", order = 0)]
    public class LoadCurrentVenue : CodeBlock
    {
        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Load();
        }

        private async void Load()
        {
            Command.Publish(new LogMessage(LogType.Log, "Loading venue assets"));
            var sceneInstance = await assets.LoadScene(GameData.Get<string>(Key.CurrentVenue));
            Command.Publish(new ShowVenueCanvas());
            Complete(sceneInstance.Scene != null);
        }
    }
}