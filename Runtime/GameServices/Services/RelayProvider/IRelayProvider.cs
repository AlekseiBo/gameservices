using System.Threading.Tasks;
using Toolset;

namespace GameServices
{
    public interface IRelayProvider : IService
    {
        Task<bool> CreateServer();
        Task<bool> JoinServer(string joinCode);
        void StopServer();
    }
}