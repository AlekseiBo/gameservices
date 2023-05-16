using Toolset;
using Unity.Services.Friends.Models;

namespace GameServices
{
    public class FriendRelationship : IMediatorCommand
    {
        public bool Added;
        public Relationship Relationship;
    }
}