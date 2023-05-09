using System.Threading.Tasks;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface ICanvasManager : IService
    {
        void ShowCanvas(IMediatorCommand command);
        void ShowCanvas<T>() where T : BaseCanvas;
        void HideCanvas(IMediatorCommand command);
        void HideCanvas<T>() where T : BaseCanvas;
        void HideAllCanvases(bool distinct);
        void CleanUp();
        Task Register<T>(AssetReferenceCanvas asset, Transform parent) where T : IMediatorCommand;
    }
}