using System.Threading.Tasks;
using Toolset;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameServices
{
    public interface ILobbyProvider : IService
    {
        Task<Lobby> CreateLobby(bool isPrivate = false);
        Task<Lobby> JoinPublicLobby(int attempt = 0, string address = "");
        void LeaveConnectedLobby();
        string GetRelayCode();
    }
}