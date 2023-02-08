using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Preload", menuName = "Code Blocks/Asset Management/Preload Asset", order = 0)]
    public class PreloadAsset : CodeBlock
    {
        [SerializeField] private string address;

        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            Preload();
        }

        private async void Preload()
        {
            var startTime = Time.time;
            var gameObject = await assets.Load<GameObject>(address);

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