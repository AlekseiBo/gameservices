using Toolset;

namespace GameServices
{
    public class UpdatePlayerAllocation : IMediatorCommand
    {
        public readonly string Allocation;

        public UpdatePlayerAllocation(string allocation)
        {
            Allocation = allocation;
        }
    }
}