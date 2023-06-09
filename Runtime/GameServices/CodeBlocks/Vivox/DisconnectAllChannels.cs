using UnityEngine;
using Toolset;
using Unity.Services.Authentication;
using VivoxUnity;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Disconnect All", menuName = "Code Blocks/Vivox/Disconnect All", order = 0)]
    public class DisconnectAllChannels : CodeBlock
    {
        protected override void Execute()
        {
            Services.All.Single<IVivoxProvider>()?.DisconnectAllChannels();
            Complete(true);
        }
    }
}