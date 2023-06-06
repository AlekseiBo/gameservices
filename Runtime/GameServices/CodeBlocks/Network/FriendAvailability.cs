using UnityEngine;
using Toolset;
using Unity.Services.Friends.Models;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Friend Availability", menuName = "Code Blocks/Network/Friend Availability", order = 0)]
    public class FriendAvailability : CodeBlock
    {
        [SerializeField] private PresenceAvailabilityOptions availability;

        protected override async void Execute()
        {
            var friendsProvider = Services.All.Single<IFriendsProvider>();
            if (friendsProvider != null) await friendsProvider.SetPresence(availability);
            Complete(true);
        }
    }
}