using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Instantiate", menuName = "Code Blocks/Asset Management/Instantiate Asset", order = 0)]
    public class Instantiate : CodeBlock
    {
        [SerializeField] private AssetReference asset;
        [SerializeField] private bool asChild;

        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            InstantiateAsset();
        }

        private async void InstantiateAsset()
        {
            var startTime = Time.time;
            var gameObject = asChild ?
                await assets.Instantiate(asset.AssetGUID, Runner.transform) :
                await assets.Instantiate(asset.AssetGUID);

            if (gameObject != null)
            {
                Debug.Log($"Successfully loaded object {gameObject.name} in {Time.time - startTime} sec");
                Completed?.Invoke(true);
            }
            else
            {
                Completed?.Invoke(false);
            }

        }
    }
}