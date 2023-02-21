using System.Threading.Tasks;
using Toolset;

namespace GameServices
{
    public interface IRelayProvider : IService
    {
        Task<bool> CreateRelayServer();
        Task<bool> JoinRelay(string joinCode);
    }
}