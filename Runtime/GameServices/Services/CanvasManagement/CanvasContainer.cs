using Toolset;
using UnityEngine;

namespace GameServices
{
    public abstract class CanvasContainer : MonoBehaviour
    {
        private static CanvasContainer instance;

        protected ICanvasManager manager;

        protected virtual void Awake()
        {
            if (GameData.isInitialized && GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            if (instance != null)
            {
                MoveCanvases();
                Destroy(instance.gameObject);
            }

            instance = this;
            manager = Services.All.Single<ICanvasManager>();
        }

        protected void Register<T>(AssetReferenceCanvas asset) where T : IMediatorCommand
        {
            manager?.Register<T>(asset, transform);
        }

        private void MoveCanvases()
        {
            for (var i = instance.transform.childCount - 1; i <= 0; i--)
                instance.transform.GetChild(i).SetParent(transform);
        }

    }
}