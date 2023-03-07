using Toolset;
using Unity.Services.Relay.Models;

namespace GameServices
{
    public class AllocateRelayServer : IMediatorCommand
    {
        public readonly Allocation Allocation;
        public readonly string JoinCode;

        public AllocateRelayServer(Allocation allocation, string joinCode)
        {
            Allocation = allocation;
            JoinCode = joinCode;
        }
    }
}