using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class FriendListViewItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button checkButton;
        [SerializeField] private Button joinButton;

        private FriendListCanvas canvas;
        private string name;
        private string id;
        private string lobbyCode;

        private void Start()
        {
            joinButton.interactable = false;
            checkButton.onClick.AddListener(CheckPlayerOnline);
            joinButton.onClick.AddListener(JoinLobby);
        }

        private void OnDestroy()
        {
            checkButton.onClick.RemoveListener(CheckPlayerOnline);
            joinButton.onClick.RemoveListener(JoinLobby);
        }

        public void UpdateFriendData(FriendListCanvas canvas, string name, string id)
        {
            this.canvas = canvas;
            this.id = id;
            this.name = name;
            playerNameText.text = this.name;
        }

        private async void CheckPlayerOnline()
        {
            checkButton.interactable = false;
            lobbyCode = await canvas.GetPlayerCurrentLobby(id);

            if (!string.IsNullOrEmpty(lobbyCode))
            {
                statusText.text = "Online";
                joinButton.interactable = true;
            }
            else
            {
                statusText.text = "Offline";
                joinButton.interactable = false;
            }

            checkButton.interactable = true;
        }

        private void JoinLobby()
        {
            if (string.IsNullOrEmpty(lobbyCode)) return;

            joinButton.interactable = false;
            canvas.JoinFriendLobby(lobbyCode);
        }
    }
}