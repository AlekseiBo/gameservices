using Toolset;
using Unity.Services.Relay.Models;

namespace GameServices.Commands
{
    public class RelayServerAllocated : IMediatorCommand
    {
        public readonly Allocation Allocation;
        public readonly string JoinCode;

        public RelayServerAllocated(Allocation allocation, string joinCode)
        {
            Allocation = allocation;
            JoinCode = joinCode;
        }
    }
}