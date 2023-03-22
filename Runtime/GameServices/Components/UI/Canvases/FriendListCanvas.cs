using System.Threading.Tasks;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public class FriendListCanvas : BaseCanvas
    {
        [Space] [SerializeField] private GameObject friendViewItemPrefab;
        [SerializeField] private Transform friendListContent;

        private IFriendListProvider provider;

        private void Awake()
        {
            provider = Services.All.Single<IFriendListProvider>();
            provider.RegisterFriendListCanvas(this);
        }

        public void CreateFriendListView(FriendList friendList)
        {
            friendListContent.ClearChildren();

            foreach (var friendData in friendList.List)
            {
                var friendViewItem = Instantiate(friendViewItemPrefab, friendListContent)
                    .GetComponent<FriendListViewItem>();

                friendViewItem.UpdateFriendData(this,friendData.Name, friendData.Id);
            }
        }

        public async Task<string> GetPlayerCurrentLobby(string friendId) =>
            await provider.GetPlayerCurrentLobby(friendId);

        public void JoinFriendLobby(string lobbyCode)
        {
            HideCanvas();
            provider.JoinFriendLobby(lobbyCode);
        }
    }
}