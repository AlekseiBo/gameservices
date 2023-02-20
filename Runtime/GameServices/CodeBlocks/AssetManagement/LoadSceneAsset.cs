using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Load", menuName = "Code Blocks/Asset Management/Load Scene", order = 0)]
    public class LoadSceneAsset : CodeBlock
    {
        [SerializeField] private AssetReference reference;
        [SerializeField] private LoadSceneMode mode;

        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Load();
        }

        private async void Load()
        {
            Debug.Log($"Loading scene with UID: {reference.AssetGUID}");
            await assets.LoadScene(reference.AssetGUID, mode);
            Complete(true);
        }
    }
}