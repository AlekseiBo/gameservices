using Toolset;

namespace GameServices
{
    public class FriendMessage : IMediatorCommand
    {
        public string Id;
        public FriendMessageText Message;
    }
}