using System.Threading.Tasks;
using Toolset;

namespace GameServices
{
    public interface IRelayProvider : IService
    {
        void CreateRelayServer();
        Task<string> GetJoinCode();
    }
}