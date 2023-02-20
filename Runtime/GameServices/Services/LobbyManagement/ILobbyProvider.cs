using System.Threading.Tasks;
using Toolset;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public interface ILobbyProvider : IService
    {
        Task<Lobby> CreateLobby(bool isPrivate = false);
        Task<Lobby> JoinPublicLobby();
        void LeaveConnectedLobby();
    }
}