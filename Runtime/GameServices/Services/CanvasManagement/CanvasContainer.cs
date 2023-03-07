using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices
{
    public class CanvasContainer : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject showMessageCanvas;
        [SerializeField] private AssetReferenceGameObject selectVenueCanvas;
        [SerializeField] private AssetReferenceGameObject showVenueCanvas;

        private ICanvasManager manager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            manager = Services.All.Single<ICanvasManager>();

            Command.Subscribe<ShowMessage>(ShowMessage);
            Command.Subscribe<SelectVenue>(SelectVenue);
            Command.Subscribe<ShowVenueCanvas>(ShowVenueCanvas);
        }

        private void OnDestroy()
        {
            Command.RemoveSubscriber<ShowMessage>(ShowMessage);
            Command.RemoveSubscriber<SelectVenue>(SelectVenue);
            Command.RemoveSubscriber<ShowVenueCanvas>(ShowVenueCanvas);
        }

        private void ShowVenueCanvas(ShowVenueCanvas command) => Register(command, showVenueCanvas);
        private void SelectVenue(SelectVenue command) => Register(command, selectVenueCanvas);
        private void ShowMessage(ShowMessage command) => Register(command, showMessageCanvas);

        private void Register(IMediatorCommand command, AssetReferenceGameObject asset) =>
            manager.Register(command, asset, transform);

    }
}