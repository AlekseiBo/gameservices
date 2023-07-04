using Toolset;

namespace GameServices
{
    public interface IServerProvider : IService
    {
        bool CreateServer();
        bool JoinServer(string ip4address, string port);
        void StopServer();
    }
}