using System.Threading.Tasks;
using Toolset;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public interface ILobbyProvider : IService
    {
        string RelayCode => "";
        string LobbyCode => "";
        string Venue => "";
        Lobby JoinedLobby { get; }

        Task<Lobby> CreateLobby(CreateLobbyData data);
        Task<Lobby> JoinLobbyByVenue(string venue, int attempt = 1);
        Task<Lobby> JoinLobbyByCode(string code);
        Task LeaveConnectedLobby();
        Task UpdateVenue(string venue);
        Task<Lobby> QueryPlayerOnline(string friendId);
    }
}