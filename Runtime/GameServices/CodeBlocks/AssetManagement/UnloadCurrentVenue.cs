using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Unload Current Venue", menuName = "Code Blocks/Assets/Unload Current Venue", order = 0)]
    public class UnloadCurrentVenue : CodeBlock
    {
        private IAssetProvider assets;

        protected override async void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            var scene = GameData.Get<string>(Key.CurrentVenue);
            var unloaded = string.IsNullOrEmpty(scene) || await assets.UnloadScene(scene);
            GameData.Set(Key.CurrentVenue, "");
            Complete(unloaded);
        }
    }
}