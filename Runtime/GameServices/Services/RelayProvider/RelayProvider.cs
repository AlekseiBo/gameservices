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

        public async void CreateRelayServer()
        {
            try
            {
                var maxConnections = GameData<Key>.Get<int>(Key.MaxPlayers) - 1;
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                joinCode = await GetJoinCode();
                var relayServerData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                if (GameData<Key>.Get<bool>(Key.RelayServer))
                    NetworkManager.Singleton.StartServer();
                else
                    NetworkManager.Singleton.StartHost();

                NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
                Mediator.Publish(new RelayServerAllocated(allocation, joinCode));

            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
            }
        }

        public async Task<string> GetJoinCode()
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
            CreateRelayServer();
        }
    }
}