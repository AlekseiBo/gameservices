using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface ICanvasManager : IService
    {
        void ShowCanvas(IMediatorCommand command);
        void ShowCanvas(string commandType);
        void HideCanvas(IMediatorCommand command);
        void HideCanvas(string commandType);
        void HideAllCanvases(bool distinct);
        void Register(IMediatorCommand command, AssetReferenceCanvas asset, Transform parent);
        void CleanUp();
    }
}