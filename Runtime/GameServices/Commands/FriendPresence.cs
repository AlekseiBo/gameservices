using Toolset;
using Unity.Services.Friends.Models;

namespace GameServices
{
    public class FriendPresence : IMediatorCommand
    {
        public string Id;
        public Presence Presence;
        public FriendActivity Activity;
    }
}