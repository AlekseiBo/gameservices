using System;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "ReleaseCachedAssets", menuName = "Code Blocks/Asset Management/Release Cached Assets", order = 0)]
    public class ClearCache : CodeBlock
    {
        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            assets.ReleaseCachedAssets();
            Complete(true);
        }
    }
}