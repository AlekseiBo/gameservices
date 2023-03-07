using System.Collections.Generic;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices
{
    public class CanvasManager : ICanvasManager
    {
        private readonly IAssetProvider assets;
        private readonly Dictionary<string, BaseCanvas> container = new();

        public CanvasManager()
        {
            assets = Services.All.Single<IAssetProvider>();
        }

        public void ShowCanvas(string commandType)
        {
            if (!container.ContainsKey(commandType)) return;

            var canvas = container[commandType];
            if (!canvas.IsAdditive) HideAllCanvases();
            canvas.Canvas.enabled = true;
        }

        public void HideCanvas(string commandType)
        {
            if (!container.ContainsKey(commandType)) return;

            var canvas = container[commandType];
            canvas.Canvas.enabled = false;
        }

        public void HideAllCanvases()
        {
            foreach (var canvas in container)
            {
                if (canvas.Value != null) canvas.Value.Canvas.enabled = false;
            }
        }

        public async void Register(IMediatorCommand command, AssetReferenceGameObject asset, Transform parent)
        {
            var commandType = command.ToString();
            if (!container.ContainsKey(commandType))
            {
                var canvasObject = await assets.Instantiate(asset.AssetGUID, parent);
                var canvas = canvasObject.GetComponent<BaseCanvas>();
                container.Add(commandType, canvas);
            }
            else if (container[commandType] == null)
            {
                var canvasObject = await assets.Instantiate(asset.AssetGUID, parent);
                container[commandType] = canvasObject.GetComponent<BaseCanvas>();
            }

            container[commandType].UpdateCanvas(command);
            ShowCanvas(commandType);
        }
    }
}