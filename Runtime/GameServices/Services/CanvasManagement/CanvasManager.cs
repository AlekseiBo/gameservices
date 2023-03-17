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
        private readonly Dictionary<string, IMediatorCommand> commandQueue = new();

        public CanvasManager() => assets = Services.All.Single<IAssetProvider>();

        public void Dispose() => IsInitialized = false;

        public void ShowCanvas(IMediatorCommand command)
        {
            var commandType = command.GetType().ToString();
            if (!container.TryGetValue(commandType, out var canvas) || canvas == null) return;

            canvas.UpdateCanvas(command);
            ShowCanvas(commandType);
        }

        public void ShowCanvas(string commandType)
        {
            if (!container.TryGetValue(commandType, out var canvas) || canvas == null) return;
            if (!canvas.Additive) HideAllCanvases(canvas.Distinct);
            canvas.ShowCanvas();
        }

        public void HideCanvas(IMediatorCommand command) => HideCanvas(command.GetType().ToString());

        public void HideCanvas(string commandType)
        {
            if (!container.TryGetValue(commandType, out var canvas) || canvas == null) return;
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
            if (!container.TryGetValue(commandType, out var canvas))
            {
                container[commandType] = null;
                var canvasObject = await assets.Instantiate(asset.AssetGUID, parent);
                canvas = canvasObject.GetComponent<BaseCanvas>();
                container[commandType] = canvas;

                if (commandQueue.Remove(commandType, out var laterCommand))
                    command = laterCommand;
            }

            if (canvas == null)
            {
                commandQueue[commandType] = command;
            }
            else
            {
                canvas.Canvas.overrideSorting = true;
                canvas.UpdateCanvas(command);
                ShowCanvas(commandType);
            }
        }
    }
}