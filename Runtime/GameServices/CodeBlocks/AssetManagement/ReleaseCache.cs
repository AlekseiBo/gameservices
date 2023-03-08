using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "ReleaseCachedAssets", menuName = "Code Blocks/Assets/Release Cache", order = 0)]
    public class ReleaseCache : CodeBlock
    {
        private IAssetProvider assets;
        protected override async void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            assets.ReleaseCachedAssets();
            Complete(true);
        }
    }
}