using Toolset;
using UnityEngine;

namespace GameServices
{
    public abstract class CanvasContainer : MonoBehaviour
    {
        private static CanvasContainer instance;

        private ICanvasManager manager;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance != null)
                Destroy(instance.gameObject);

            instance = this;
            manager = Services.All.Single<ICanvasManager>();
            manager.CleanUp();
        }

        protected void Register(IMediatorCommand command, AssetReferenceCanvas asset) =>
            manager.Register(command, asset, transform);

    }
}