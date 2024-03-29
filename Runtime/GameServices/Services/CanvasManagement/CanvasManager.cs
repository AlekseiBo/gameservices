﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
            if (!container.TryGetValue(commandType, out var canvas) || canvas == null)
            {
                commandQueue[commandType] = command;
                return;
            }

            canvas.UpdateCanvas(command);
            ShowCanvas(canvas);
            commandQueue.Remove(commandType);
        }

        public void ShowCanvas<T>() where T : BaseCanvas
        {
            var canvas = container.First(vp => typeof(T) == vp.Value.GetType());
            if (canvas.Value != null) ShowCanvas(canvas.Value);
        }

        private void ShowCanvas(BaseCanvas canvas)
        {
            if (!canvas.Additive) HideAllCanvases(canvas.Distinct);
            canvas.ShowCanvas();
        }

        public void HideCanvas(IMediatorCommand command) => HideCanvas(command.GetType().ToString());

        public void HideCanvas<T>() where T : BaseCanvas
        {
            var canvas = container.First(vp => typeof(T) == vp.Value.GetType());
            if (canvas.Value != null) HideCanvas(canvas.Key);
        }

        private void HideCanvas(string commandType)
        {
            commandQueue.Remove(commandType);
            if (!container.TryGetValue(commandType, out var canvas) || canvas == null) return;
            canvas.HideCanvas();
        }

        public void HideAllCanvases(bool distinct)
        {
            foreach (var canvas in container)
            {
                if (canvas.Value == null || !canvas.Value.Visible) continue;
                if (canvas.Value.Additive && !distinct) continue;
                canvas.Value.HideCanvas();
            }
        }

        public async Task Register<T>(AssetReferenceCanvas asset, Transform parent) where T : IMediatorCommand
        {
            var commandType = typeof(T).ToString();
            container[commandType] = null;

            var canvasObject = await assets.Instantiate(asset.AssetGUID, parent, true);
            if (parent == null)
            {
                Addressables.ReleaseInstance(canvasObject);
                UnityEngine.Object.Destroy(canvasObject);
                container.Remove(commandType);
            }
            else
            {
                var newCanvas = canvasObject.GetComponent<BaseCanvas>();
                newCanvas.Canvas.overrideSorting = true;
                var showNewCanvas = false;

                if (container.TryGetValue(commandType, out var canvas) && canvas != null)
                {
                    showNewCanvas = canvas.Visible;
                    Addressables.ReleaseInstance(canvas.gameObject);
                    UnityEngine.Object.Destroy(canvas.gameObject);
                }

                container[commandType] = newCanvas;

                if (commandQueue.TryGetValue(commandType, out var command)) ShowCanvas(command);
                else if (showNewCanvas) ShowCanvas(newCanvas);
                else HideCanvas(commandType);
            }
        }

        public void CleanUp()
        {
            foreach (var keyValue in container.Where(keyValue => keyValue.Value != null))
                UnityEngine.Object.Destroy(keyValue.Value.gameObject);

            container.Clear();
            commandQueue.Clear();
        }
    }
}