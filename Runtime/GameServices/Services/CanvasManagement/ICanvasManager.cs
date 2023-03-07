using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices
{
    public interface ICanvasManager : IService
    {
        void ShowCanvas(string commandType);
        void HideCanvas(string commandType);
        void HideAllCanvases();
        void Register(IMediatorCommand command, AssetReferenceGameObject asset, Transform parent);
    }
}