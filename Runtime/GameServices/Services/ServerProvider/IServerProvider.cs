using Toolset;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Multiplay;
using UnityEngine;

namespace GameServices
{
    public interface IServerProvider : IService
    {
        bool CreateServer();
        bool JoinServer(string ip4address, string port);
        void StopServer();
    }
}