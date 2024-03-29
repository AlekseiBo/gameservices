﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;
using Unity.Services.Friends.Models;

namespace GameServices
{
    public interface IFriendsProvider : IService
    {
        Dictionary<string, string> FriendConversations { get; }
        IReadOnlyList<Relationship> GetFriendsList();
        IReadOnlyList<Relationship> GetIncomingRequests();
        IReadOnlyList<Relationship> GetOutgoingRequests();
        IReadOnlyList<Relationship> GetBlockedPlayers();
        void SendRequest(string playerId);
        void SendRequestByName(string playerName);
        void AcceptRequest(string playerId);
        void DeclineRequest(string playerId);
        void Delete(string playerId);
        void DeleteIncomingRequest(string playerId);
        void DeleteOutgoingRequest(string playerId);
        void Block(string playerId);
        void Unblock(string playerId);
        Task SetPresence(PresenceAvailabilityOptions availability, FriendActivity activity);
        Task SetPresence(PresenceAvailabilityOptions availability);
        Task SetPresence(FriendActivity activity);
        void SendMessage<T>(string playerId, T message) where T : new();
        Task RefreshRelationships();
        void AddMessageToConversation(string playerId, string message, bool incoming);
        void LoadConversations();
        void DeleteConversation(string playerId);
        void JoinFriendVenue(string joinCode);
    }
}