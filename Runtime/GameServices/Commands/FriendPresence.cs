using System;
using Toolset;
using Unity.Services.Friends.Models;

namespace GameServices
{
    public class FriendPresence : IMediatorCommand
    {
        public string Id;
        public PresenceAvailabilityOptions Availability;
        public DateTime LastSeen;
        public FriendActivity Activity;
    }
}