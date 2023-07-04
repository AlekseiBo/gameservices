using Toolset;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
#if !UNITY_IOS && !UNITY_ANDROID
using Unity.Services.Multiplay;
#endif
using UnityEngine;

namespace GameServices
{
    public class ServerProvider : IServerProvider
    {
        public bool CreateServer()
        {
#if !UNITY_IOS && !UNITY_ANDROID
            var serverConfig = MultiplayService.Instance.ServerConfig;
            var network = CreateNetworkManager();
            network.GetComponent<UnityTransport>().SetConnectionData("0.0.0.0", serverConfig.Port, "0.0.0.0");
            var started = network.StartServer();
            if (!started) return false;
            Command.Publish(new AllocateDedicatedServer(serverConfig.IpAddress, serverConfig.Port.ToString()));
#endif
            return true;
        }

        public bool JoinServer(string ip4address, string port)
        {
            var network = CreateNetworkManager();
            network.GetComponent<UnityTransport>().SetConnectionData(ip4address, ushort.Parse(port));
            var started = network.StartClient();
            return started;
        }

        public void StopServer()
        {
            Debug.Log("Stopping dedicated server");

            if (NetworkManager.Singleton == null) return;

            if (NetworkManager.Singleton.IsServer)
                NetworkManager.Singleton.Shutdown(true);

            Object.Destroy(NetworkManager.Singleton.gameObject);
        }

        private NetworkManager CreateNetworkManager()
        {
            if (NetworkManager.Singleton != null) return NetworkManager.Singleton;

            var staticData = Services.All.Single<IStaticDataService>();
            var venueData = staticData.ForVenue(GameData.Get<string>(Key.CurrentVenue));

            if (venueData == null) return null;

            var networkManager = Object.Instantiate(venueData.ServerManager)
                .With(e => e.name = "Server Manager")
                .GetComponent<NetworkManager>();

            return networkManager;
        }
    }
}