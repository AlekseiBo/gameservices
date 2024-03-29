﻿using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Preload", menuName = "Code Blocks/Assets/Preload Asset", order = 0)]
    public class PreloadAsset : CodeBlock
    {
        [SerializeField] private string address;

        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Preload();
        }

        private async void Preload()
        {
            var gameObject = await assets.Load<GameObject>(address);
            Complete(gameObject != null);
        }
    }
}