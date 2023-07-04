#if !UNITY_IOS && !UNITY_ANDROID
using Toolset;
using Unity.Netcode;
using Unity.Services.Multiplay;
using UnityEngine;

namespace GameServices
{
    public class ServerStats : MonoBehaviour
    {
        [SerializeField] private string serverName = "SERVER";

        private IServerQueryHandler queryHandler;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            var serverConfig = MultiplayService.Instance.ServerConfig;
            Debug.Log($"Server address: {serverConfig.IpAddress}:{serverConfig.Port}");
        }

        private async void Start()
        {
            var maxPlayers = (ushort)GameData.Get<int>(Key.LobbyMaxPlayers);
            var venue = GameData.Get<string>(Key.RequestedVenue);
            var venueData = Services.All.Single<IStaticDataService>().ForVenue(venue);
            var gameType = venueData.AvatarGroup.ToString();
            var map = venueData.Name;

            queryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync (
                maxPlayers,
                serverName,
                gameType,
                Application.version,
                map);
        }

        private void Update()
        {
            if (queryHandler == null || NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer) return;
            queryHandler.CurrentPlayers = (ushort)NetworkManager.Singleton.ConnectedClientsIds.Count;
            queryHandler.UpdateServerCheck();
        }
    }
}
#endif