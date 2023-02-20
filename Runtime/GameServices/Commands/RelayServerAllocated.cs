using Toolset;
using Unity.Services.Relay.Models;

namespace GameServices.Commands
{
    public class RelayServerAllocated : IMediatorCommand
    {
        public Allocation Allocation;
        public string JoinCode;

        public RelayServerAllocated(Allocation allocation, string joinCode)
        {
            Allocation = allocation;
            JoinCode = joinCode;
        }
    }
}