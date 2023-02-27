using System.Threading.Tasks;
using Toolset;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameServices
{
    public interface ILobbyProvider : IService
    {
        string RelayCode => "";
        string LobbyCode => "";
        string Venue => "";

        Task<Lobby> CreateLobby(bool isPrivate = false);
        Task<Lobby> JoinPublicLobby(int attempt = 0, string address = "");
        void LeaveConnectedLobby();

        Task<string> GetLobbyVenue(string lobbyCode);
        Task<Lobby> JoinLobby(string code);
    }
}