using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Unload", menuName = "Code Blocks/Asset Management/Unload Scene", order = 0)]
    public class UnloadScene : CodeBlock
    {
        [SerializeField] private AssetReference reference;

        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            Unload();
        }

        private async void Unload()
        {
            var startTime = Time.time;
            var unloaded = await assets.UnloadScene(reference.AssetGUID);
            Debug.Log($"Unloading result: {unloaded} in {Time.time - startTime} sec");
            Completed?.Invoke(unloaded);
        }
    }
}