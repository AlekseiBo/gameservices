using Toolset;
using Unity.Services.Relay.Models;

namespace GameServices
{
    public class AllocateDedicatedServer : IMediatorCommand
    {
        public readonly string IP4address;
        public readonly string Port;

        public AllocateDedicatedServer(string ip4Address, string port)
        {
            IP4address = ip4Address;
            Port = port;
        }
    }
}