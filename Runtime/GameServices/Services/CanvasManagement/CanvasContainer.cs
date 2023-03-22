using Toolset;
using UnityEngine;

namespace GameServices
{
    public class CanvasContainer : MonoBehaviour
    {
        [SerializeField] private AssetReferenceCanvas showMessageCanvas;
        [SerializeField] private AssetReferenceCanvas showDialogCanvas;
        [SerializeField] private AssetReferenceCanvas logMessageCanvas;
        [SerializeField] private AssetReferenceCanvas loadingProgressCanvas;
        [SerializeField] private AssetReferenceCanvas friendListCanvas;
        [SerializeField] private AssetReferenceCanvas selectVenueCanvas;
        [SerializeField] private AssetReferenceCanvas showVenueCanvas;

        private ICanvasManager manager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            manager = Services.All.Single<ICanvasManager>();

            Command.Subscribe<ShowMessage>(ShowMessage);
            Command.Subscribe<ShowDialog>(ShowDialog);
            Command.Subscribe<LogMessage>(LogMessage);
            Command.Subscribe<ShowLoadingProgress>(ShowProgress);
            Command.Subscribe<ShowFriendList>(ShowFriendList);
            Command.Subscribe<SelectVenue>(SelectVenue);
            Command.Subscribe<ShowVenueCanvas>(ShowVenueCanvas);
        }

        private void OnDestroy()
        {
            Command.RemoveSubscriber<ShowMessage>(ShowMessage);
            Command.RemoveSubscriber<ShowDialog>(ShowDialog);
            Command.RemoveSubscriber<LogMessage>(LogMessage);
            Command.RemoveSubscriber<ShowLoadingProgress>(ShowProgress);
            Command.RemoveSubscriber<ShowFriendList>(ShowFriendList);
            Command.RemoveSubscriber<SelectVenue>(SelectVenue);
            Command.RemoveSubscriber<ShowVenueCanvas>(ShowVenueCanvas);
        }

        private void ShowMessage(ShowMessage command) => Register(command, showMessageCanvas);
        private void ShowDialog(ShowDialog command) => Register(command, showDialogCanvas);
        private void LogMessage(LogMessage command) => Register(command, logMessageCanvas);
        private void ShowProgress(ShowLoadingProgress command) => Register(command, loadingProgressCanvas);
        private void ShowFriendList(ShowFriendList command) => Register(command, friendListCanvas);
        private void ShowVenueCanvas(ShowVenueCanvas command) => Register(command, showVenueCanvas);
        private void SelectVenue(SelectVenue command) => Register(command, selectVenueCanvas);

        private void Register(IMediatorCommand command, AssetReferenceCanvas asset) =>
            manager.Register(command, asset, transform);

    }
}