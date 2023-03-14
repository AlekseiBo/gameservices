using System.Threading.Tasks;
using Toolset;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameServices
{
    public class RelayProvider : IRelayProvider
    {
        private Allocation allocation;
        private int maxConnections;
        private bool isHost;

        public async Task<bool> CreateServer(int connections, bool host = false)
        {
            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(connections);
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return false;
            }

            var network = CreateNetworkManager();
            var joinCode = await GetJoinCode();
            var relayServerData = new RelayServerData(allocation, "dtls");
            network.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            var started = host ? network.StartHost() : network.StartServer();
            if (!started) return false;

            maxConnections = connections;
            isHost = host;
            network.OnTransportFailure += OnFailure;
            Command.Publish(new AllocateRelayServer(allocation, joinCode));
            return true;
        }

        public async Task<bool> JoinServer(string joinCode)
        {
            Debug.Log($"Trying to join relay server {joinCode}");
            try
            {
                var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                if (joinAllocation == null) return false;

                var relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                return NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }

        public void StopServer()
        {
            Debug.Log("Stopping relay server");
            allocation = null;

            if (NetworkManager.Singleton == null) return;

            NetworkManager.Singleton.OnTransportFailure -= OnFailure;

            if (NetworkManager.Singleton.IsServer)
                NetworkManager.Singleton.Shutdown(true);

            Object.Destroy(NetworkManager.Singleton.gameObject);
        }

        public NetworkManager CreateNetworkManager()
        {
            if (NetworkManager.Singleton != null) return NetworkManager.Singleton;

            var staticData = Services.All.Single<IStaticDataService>();
            var venueData = staticData.ForVenue(GameData.Get<string>(Key.CurrentVenue));

            if (venueData == null) return null;

            return Object.Instantiate(venueData.NetworkManager)
                .With(e => e.name = "Network Manager")
                .GetComponent<NetworkManager>();
        }

        private async Task<string> GetJoinCode()
        {
            try
            {
                var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return default;
            }
        }

        private void OnFailure()
        {
            Debug.LogError("Restarting relay server");
            StopServer();
            CreateServer(maxConnections, isHost);
        }
    }
}