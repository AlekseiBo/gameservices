using System.Threading.Tasks;
using GameServices.Commands;
using Toolset;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace GameServices
{
    public class RelayProvider : IRelayProvider
    {
        private Allocation allocation;
        private string joinCode;

        public async Task<bool> CreateRelayServer()
        {
            try
            {
                var maxConnections = GameData.Get<int>(Key.MaxPlayers) - 1;
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                joinCode = await GetJoinCode();
                var relayServerData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                if (GameData.Get<bool>(Key.RelayServer))
                    NetworkManager.Singleton.StartServer();
                else
                    NetworkManager.Singleton.StartHost();

                NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
                Mediator.Publish(new RelayServerAllocated(allocation, joinCode));
                return true;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }

        public async Task<bool> JoinRelay(string joinCode)
        {
            Debug.Log($"Trying to join {joinCode}");
            try
            {
                var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                var relayServerData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
                return true;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }

        private  async Task<string> GetJoinCode()
        {
            try
            {
                var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return "";
            }
        }


        private async void OnTransportFailure()
        {
            NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
            Debug.Log("Relay transport failure");
            await Task.Delay(5000);
            await CreateRelayServer();
        }
    }
}