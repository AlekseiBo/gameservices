using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public class CanvasManager : ICanvasManager, IDisposable
    {
        public static bool IsInitialized;
        private readonly IAssetProvider assets;
        private readonly Dictionary<string, BaseCanvas> container = new();

        public CanvasManager() => assets = Services.All.Single<IAssetProvider>();

        public void Dispose() => IsInitialized = false;

        public void ShowCanvas(IMediatorCommand command)
        {
            var commandType = command.GetType().ToString();
            if (!container.ContainsKey(commandType)) return;

            container[commandType].UpdateCanvas(command);
            ShowCanvas(commandType);
        }

        public void ShowCanvas(string commandType)
        {
            if (!container.ContainsKey(commandType)) return;

            var canvas = container[commandType];
            if (!canvas.Additive) HideAllCanvases(canvas.Distinct);
            canvas.ShowCanvas();
        }

        public void HideCanvas(IMediatorCommand command) => HideCanvas(command.GetType().ToString());

        public void HideCanvas(string commandType)
        {
            if (!container.ContainsKey(commandType)) return;

            var canvas = container[commandType];
            canvas.HideCanvas();
        }

        public void HideAllCanvases(bool distinct)
        {
            foreach (var canvas in container)
            {
                if (canvas.Value == null) continue;
                if (canvas.Value.Additive && !distinct) continue;
                canvas.Value.HideCanvas();
            }
        }

        public async void Register(IMediatorCommand command, AssetReferenceCanvas asset, Transform parent)
        {
            var commandType = command.ToString();
            if (!container.ContainsKey(commandType))
            {
                var canvasObject = await assets.Instantiate(asset.AssetGUID, parent);
                var canvas = canvasObject.GetComponent<BaseCanvas>();
                if (container.ContainsKey(commandType))
                    container[commandType] = canvas;
                else
                    container.Add(commandType, canvas);
            }
            else if (container[commandType] == null)
            {
                var canvasObject = await assets.Instantiate(asset.AssetGUID, parent);
                container[commandType] = canvasObject.GetComponent<BaseCanvas>();
            }

            container[commandType].Canvas.overrideSorting = true;
            container[commandType].UpdateCanvas(command);
            ShowCanvas(commandType);
        }
    }
}