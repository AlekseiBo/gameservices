using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Preload Assets", menuName = "Code Blocks/Initialization/Preload Assets", order = 0)]
    public class PreloadAssets : CodeBlock
    {
        protected override async void Execute()
        {
            var assetProvider = Services.All.Single<IAssetProvider>();
            await assetProvider.PreloadAsset();
            Complete(true);
        }
    }
}