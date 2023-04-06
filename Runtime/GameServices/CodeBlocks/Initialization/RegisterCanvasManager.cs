using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Canvas Manager",
        menuName = "Code Blocks/Initialization/Register Canvas Manager", order = 0)]
    public class RegisterCanvasManager : CodeBlock
    {
        [SerializeField] private AssetReferenceGameObject prefab;

        protected override async void Execute()
        {
            if (Services.All.Single<ICanvasManager>() == null)
            {
                Services.All.RegisterSingle<ICanvasManager>(new CanvasManager());
                var canvasContainer =
                    await Services.All.Single<IAssetProvider>().Instantiate(prefab.AssetGUID, true);

                if (canvasContainer == null)
                {
                    Complete(false);
                    return;
                }

                canvasContainer.name = "Canvas Container";
                Command.Publish(new LogMessage(LogType.Log, "Loading UI assets"));
                Command.Publish(new ShowLoadingProgress(0f));
                CanvasManager.IsInitialized = true;
            }
            
            Complete(true);
        }
    }
}