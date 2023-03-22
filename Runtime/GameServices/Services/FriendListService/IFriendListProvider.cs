using System.Threading.Tasks;
using Toolset;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public interface IFriendListProvider : IService
    {
        void RegisterFriendListCanvas(FriendListCanvas canvas);
        Task<string> GetPlayerCurrentLobby(string friendId);
        void JoinFriendLobby(string friendLobbyCode);
    }
}