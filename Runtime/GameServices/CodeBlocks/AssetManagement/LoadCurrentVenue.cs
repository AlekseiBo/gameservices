using System;
using Toolset;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            var sceneInstance = await assets.LoadScene(GameData.Get<string>(Key.CurrentVenue));
            Command.Publish(new ShowVenueCanvas());
            Complete(sceneInstance.Scene != null);
        }
    }
}