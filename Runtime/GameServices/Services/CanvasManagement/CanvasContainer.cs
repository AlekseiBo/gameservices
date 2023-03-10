using Toolset;
using UnityEngine;

namespace GameServices
{
    public class CanvasContainer : MonoBehaviour
    {
        [SerializeField] private AssetReferenceCanvas showMessageCanvas;
        [SerializeField] private AssetReferenceCanvas logMessageCanvas;
        [SerializeField] private AssetReferenceCanvas loadingProgressCanvas;
        [SerializeField] private AssetReferenceCanvas selectVenueCanvas;
        [SerializeField] private AssetReferenceCanvas showVenueCanvas;

        private ICanvasManager manager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            manager = Services.All.Single<ICanvasManager>();

            Command.Subscribe<ShowMessage>(ShowMessage);
            Command.Subscribe<LogMessage>(LogMessage);
            Command.Subscribe<ShowLoadingProgress>(ShowProgress);
            Command.Subscribe<SelectVenue>(SelectVenue);
            Command.Subscribe<ShowVenueCanvas>(ShowVenueCanvas);
        }

        private void OnDestroy()
        {
            Command.RemoveSubscriber<ShowMessage>(ShowMessage);
            Command.RemoveSubscriber<LogMessage>(LogMessage);
            Command.RemoveSubscriber<ShowLoadingProgress>(ShowProgress);
            Command.RemoveSubscriber<SelectVenue>(SelectVenue);
            Command.RemoveSubscriber<ShowVenueCanvas>(ShowVenueCanvas);
        }

        private void ShowMessage(ShowMessage command) => Register(command, showMessageCanvas);
        private void LogMessage(LogMessage command) => Register(command, logMessageCanvas);
        private void ShowProgress(ShowLoadingProgress command) => Register(command, loadingProgressCanvas);
        private void ShowVenueCanvas(ShowVenueCanvas command) => Register(command, showVenueCanvas);
        private void SelectVenue(SelectVenue command) => Register(command, selectVenueCanvas);

        private void Register(IMediatorCommand command, AssetReferenceCanvas asset) =>
            manager.Register(command, asset, transform);

    }
}