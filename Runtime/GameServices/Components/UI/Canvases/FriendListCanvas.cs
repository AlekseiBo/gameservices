using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public class FriendListCanvas : BaseCanvas
    {
        private const float REQUEST_TIMEOUT = 1f;

        [Space] [SerializeField] private GameObject friendViewItemPrefab;
        [SerializeField] private Transform friendListContent;

        private IFriendListProvider provider;
        private List<FriendListViewItem> friendItemList = new();
        private Coroutine scanning;
        private int scanningIndex;
        private bool queryInProgress;

        private void Awake()
        {
            provider = Services.All.Single<IFriendListProvider>();
            provider.RegisterFriendListCanvas(this);

        }

        public void CreateFriendListView(FriendList friendList)
        {
            friendListContent.ClearChildren();
            friendItemList.Clear();
            scanningIndex = 0;

            foreach (var friendData in friendList.List)
            {
                var friendViewItem = Instantiate(friendViewItemPrefab, friendListContent)
                    .GetComponent<FriendListViewItem>();

                friendItemList.Add(friendViewItem);
                friendViewItem.UpdateFriendData(this,friendData.Name, friendData.Id);
            }
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
            scanning = StartCoroutine(Scanning());
        }

        public override void HideCanvas()
        {
            base.HideCanvas();
            StopCoroutine(scanning);
        }

        public async Task<string> GetPlayerCurrentLobby(string friendId)
        {
            var timeout = (int)REQUEST_TIMEOUT * 1000;
            queryInProgress = true;
            await Task.Delay(timeout);
            var result = await provider.GetPlayerCurrentLobby(friendId);
            await Task.Delay(timeout);
            queryInProgress = false;
            return result;
        }

        public void JoinFriendLobby(string lobbyCode)
        {
            HideCanvas();
            provider.JoinFriendLobby(lobbyCode);
        }

        private IEnumerator Scanning()
        {
            while (true)
            {
                yield return Utilities.WaitFor(REQUEST_TIMEOUT);
                friendItemList[scanningIndex].CheckPlayerOnline();
                while (queryInProgress) yield return null;
                scanningIndex = scanningIndex >= friendItemList.Count - 1 ? 0 : scanningIndex + 1;
            }
        }
    }
}