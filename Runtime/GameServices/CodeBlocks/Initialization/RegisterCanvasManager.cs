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
        [SerializeField] private string failMessage = "Failed to update app content. Please try again.";

        protected override async void Execute()
        {
            if (Services.All.Single<ICanvasManager>() == null)
            {
                Services.All.RegisterSingle<ICanvasManager>(new CanvasManager());
                var canvasContainer =
                    await Services.All.Single<IAssetProvider>().Instantiate(prefab.AssetGUID, true);

                if (canvasContainer == null)
                {
                    OnFail();
                    return;
                }

                canvasContainer.name = "Canvas Container";
                Command.Publish(new LogMessage(LogType.Log, "Loading UI assets"));
                Command.Publish(new ShowLoadingProgress(0f));
                CanvasManager.IsInitialized = true;
            }
            
            Complete(true);
        }

        private void OnFail()
        {
            Complete(false);
            Command.Publish(new ShowMessage("Error", failMessage, Application.Quit));
        }
    }
}