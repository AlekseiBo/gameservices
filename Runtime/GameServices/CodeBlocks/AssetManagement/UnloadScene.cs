using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Unload", menuName = "Code Blocks/Assets/Unload Scene Asset", order = 0)]
    public class UnloadScene : CodeBlock
    {
        [SerializeField] private AssetReferenceScene reference;

        private IAssetProvider assets;

        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Unload();
        }

        private async void Unload()
        {
            var unloaded = await assets.UnloadScene(reference.AssetGUID);
            Complete(unloaded);
        }
    }
}