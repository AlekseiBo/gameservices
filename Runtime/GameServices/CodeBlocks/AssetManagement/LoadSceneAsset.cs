using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Load", menuName = "Code Blocks/Asset Management/Load Scene", order = 0)]
    public class LoadSceneAsset : CodeBlock
    {
        [SerializeField] private AssetReference reference;
        [SerializeField] private LoadSceneMode mode;

        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            Load();
        }

        private async void Load()
        {
            var startTime = Time.time;
            await assets.LoadScene(reference.AssetGUID, mode);
            Debug.Log($"Successfully loaded scene in {Time.time - startTime} sec");
            Completed?.Invoke(true);
        }
    }
}