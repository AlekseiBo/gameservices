using System;
using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Unload", menuName = "Code Blocks/Asset Management/Unload Asset", order = 0)]
    public class UnloadAsset : CodeBlock
    {
        [SerializeField] private string address;

        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            assets.Unload(address);
            Complete(true);
        }
    }
}