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
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
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

        private void OnClientDisconnect(ulong clientId)
        {
            Debug.LogError($"Client {clientId} disconnected");
            if (clientId == 0 && GameData.Get<bool>(Key.RelayServer))
            {
                OnTransportFailure();
            }
        }

        private void OnTransportFailure()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
            Debug.LogError("Relay transport failure");
            if (GameData.Get<bool>(Key.RelayServer)) Application.Quit();
        }
    }
}