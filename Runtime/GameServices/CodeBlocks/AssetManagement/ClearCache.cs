using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;

namespace GameServices.CodeBlocks.AssetManagement
{
    [CreateAssetMenu(fileName = "ReleaseCachedAssets", menuName = "Code Blocks/Asset Management/Release Cached Assets", order = 0)]
    public class ClearCache : CodeBlock
    {
        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            assets.ReleaseCachedAssets();
            Completed?.Invoke(true);
        }
    }
}