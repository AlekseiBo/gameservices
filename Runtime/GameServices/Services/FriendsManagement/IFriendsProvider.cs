using System.Collections.Generic;
using Toolset;
using Unity.Services.Friends.Models;

namespace GameServices
{
    public interface IFriendsProvider : IService
    {
        IReadOnlyList<Relationship> GetFriendsList();
        IReadOnlyList<Relationship> GetIncomingRequests();
        IReadOnlyList<Relationship> GetOutgoingRequests();
        IReadOnlyList<Relationship> GetBlockedPlayers();
        void SendRequest(string playerId);
        void AcceptRequest(string playerId);
        void DeclineRequest(string playerId);
        void Delete(string playerId);
        void DeleteIncomingRequest(string playerId);
        void DeleteOutgoingRequest(string playerId);
        void Block(string playerId);
        void Unblock(string playerId);
        void SetPresence(PresenceAvailabilityOptions availability, FriendActivity activity);
        void SetPresence(PresenceAvailabilityOptions availability);
        void SetPresence(FriendActivity activity);
        void SendMessage<T>(string playerId, T message) where T : new();
    }
}