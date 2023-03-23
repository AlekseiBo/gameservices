using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;

namespace GameServices
{
    public class FriendListProvider : IFriendListProvider, IProgressWriter, IDisposable
    {
        private readonly IProgressProvider progress;
        private readonly ILobbyProvider lobby;
        private FriendList friendList = new();
        private FriendListCanvas canvasView;

        public FriendListProvider()
        {
            progress = Services.All.Single<IProgressProvider>();
            lobby = Services.All.Single<ILobbyProvider>();
            var reader = this as IProgressReader;
            LoadProgress(reader.RegisterProgress());
            Command.Subscribe<UpdateFriendList>(OnFriendListUpdate);
        }

        public void Dispose()
        {
            Command.RemoveSubscriber<UpdateFriendList>(OnFriendListUpdate);
            var reader = this as IProgressReader;
            reader.UnregisterProgress();
        }

        public void RegisterFriendListCanvas(FriendListCanvas canvas)
        {
            canvasView = canvas;
            canvasView.CreateFriendListView(friendList);
        }

        public async Task<string> GetPlayerCurrentLobby(string friendId)
        {
            var friendLobby = await lobby.QueryPlayerOnline(friendId);
            return friendLobby != null && friendLobby.Data != null
                ? friendLobby.Data.GetValueOrDefault("LOBBY_CODE").Value
                : "";
        }

        public void JoinFriendLobby(string friendLobbyCode)
        {
            GameData.Set(Key.RequestedLobbyCode, friendLobbyCode);
            GameData.Set(Key.PlayerNetState, NetState.Guest);

            if (lobby.JoinedLobby != null)
                Command.Publish(new UpdateVenue(VenueAction.Exit, ""));
            else
                Command.Publish(new ConnectToLobby());
        }

        private void OnFriendListUpdate(UpdateFriendList data)
        {
            if (data.Add)
                friendList.UpdateOrAdd(data.Name, data.Id);
            else
                friendList.Remove(data.Id);

            progress.SaveProgress();
            canvasView?.CreateFriendListView(friendList);
        }

        public void SaveProgress(ref ProgressData progress) => progress.FriendList = friendList;

        public void LoadProgress(ProgressData progress) => friendList = progress.FriendList;
    }
}