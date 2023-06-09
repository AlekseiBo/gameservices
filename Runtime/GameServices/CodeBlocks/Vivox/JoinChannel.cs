using UnityEngine;
using Toolset;
using Unity.Services.Authentication;
using VivoxUnity;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Join Channel", menuName = "Code Blocks/Vivox/Join Channel", order = 0)]
    public class JoinChannel : CodeBlock
    {
        [SerializeField] private ChannelType channelType;
        [SerializeField] private ChatCapability chatCapability;

        protected override void Execute()
        {
            var provider = Services.All.Single<IVivoxProvider>();
            if (provider != null)
            {
                var channelName = Services.All.Single<ILobbyProvider>()?.JoinedLobby.Id;
                provider.JoinChannel(channelName, channelType, chatCapability);
            }

            Complete(true);
        }
    }
}