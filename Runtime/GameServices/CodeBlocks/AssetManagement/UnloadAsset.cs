using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Unload", menuName = "Code Blocks/Asset Management/Unload Asset", order = 0)]
    public class UnloadAsset : CodeBlock
    {
        [SerializeField] private string address;

        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            assets.Unload(address);
            Completed?.Invoke(true);
        }
    }
}