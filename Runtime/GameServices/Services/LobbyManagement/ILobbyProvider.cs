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

        Task<Lobby> CreateLobby(CreateLobbyData data);
        Task<Lobby> JoinLobbyByVenue(string venue, int attempt = 1);
        Task<Lobby> JoinLobbyByCode(string code);
        Task LeaveConnectedLobby();
    }
}