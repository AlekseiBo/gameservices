using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
        private const float HEARTBEAT_TIMEOUT = 30;
        private Allocation allocation;
        private string joinCode;
        private CancellationTokenSource tokenSource;

        public async Task<bool> CreateServer()
        {
            try
            {
                var network = NetworkManager.Singleton;
                var host = !GameData.Get<bool>(Key.RelayServer);
                var maxConnections = GameData.Get<int>(Key.MaxPlayers) - 1;
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                joinCode = await GetJoinCode();
                var relayServerData = new RelayServerData(allocation, "dtls");
                network.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                var started = host ? network.StartHost() : network.StartServer();
                if (started)
                {
                    if (!host)
                    {
                        tokenSource?.Cancel();
                        tokenSource = new CancellationTokenSource();
                        var token = tokenSource.Token;
                        RunHeartbeat(HEARTBEAT_TIMEOUT, token);
                    }

                    network.OnTransportFailure += OnFailure;
                    Mediator.Publish(new RelayServerAllocated(allocation, joinCode));
                    return true;
                }

                return false;

            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }

        public async Task<bool> JoinServer(string joinCode)
        {
            Debug.Log($"Trying to join {joinCode}");
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
            allocation = null;
            tokenSource?.Cancel();
            if (NetworkManager.Singleton.IsServer)
                NetworkManager.Singleton.Shutdown();
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
                return "";
            }
        }

        private void OnFailure()
        {
            NetworkManager.Singleton.OnTransportFailure -= OnFailure;
            Debug.LogError("Restarting relay server");
            StopServer();
            CreateServer();
        }

        private async void RunHeartbeat(float timeout, CancellationToken token)
        {
            var delayTimeout = (int)timeout * 1000;

            while (!token.IsCancellationRequested)
            {
                //if (joinCode != await GetJoinCode())

                await Task.Delay(delayTimeout, token);
                OnFailure();
            }
        }
    }
}