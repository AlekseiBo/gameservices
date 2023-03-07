using System;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Canvas Manager", menuName = "Code Blocks/Initialization/Register Canvas Manager", order = 0)]
    public class RegisterCanvasManager : CodeBlock
    {
        [SerializeField] private AssetReferenceGameObject prefab;

        private GameObject canvasContainer;

        protected override async void Execute()
        {
            if (Services.All.Single<ICanvasManager>() == null)
            {
                Services.All.RegisterSingle<ICanvasManager>(new CanvasManager());
            }

            if (canvasContainer != null) Destroy(canvasContainer);

            canvasContainer = await Services.All.Single<IAssetProvider>().Instantiate(prefab.AssetGUID);
            canvasContainer.name = "Canvas Container";

            Complete(true);
        }
    }
}