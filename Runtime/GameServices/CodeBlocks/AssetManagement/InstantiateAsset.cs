﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Instantiate", menuName = "Code Blocks/Assets/Instantiate Asset", order = 0)]
    public class InstantiateAsset : CodeBlock
    {
        [SerializeField] private AssetReference asset;
        [SerializeField] private bool asChild;
        [SerializeField] private bool persistent;

        private IAssetProvider assets;

        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Instantiate();
        }

        private async void Instantiate()
        {
            var startTime = Time.time;
            var gameObject = asChild ?
                await assets.Instantiate(asset.AssetGUID, Runner.transform, persistent) :
                await assets.Instantiate(asset.AssetGUID, persistent);

            if (gameObject != null)
            {
                Debug.Log($"Successfully loaded object {gameObject.name} in {Time.time - startTime} sec");
                Complete(true);
            }
            else
            {
                Complete(false);
            }

        }
    }
}