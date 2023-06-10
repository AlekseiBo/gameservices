using Toolset;
using VivoxUnity;

namespace GameServices
{
    public class AddChatMessage : IMediatorCommand
    {
        public IChannelTextMessage Message;

        public AddChatMessage(IChannelTextMessage message)
        {
            Message = message;
        }
    }
}