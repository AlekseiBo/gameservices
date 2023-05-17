using System;
using System.Collections.Generic;
using Toolset;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;

namespace GameServices
{
    public class FriendsProvider : IFriendsProvider, IDisposable
    {
        private readonly FriendsQuery query;

        public FriendsProvider()
        {
            query = new FriendsQuery();
            FriendsService.Instance.MessageReceived += OnMessageReceived;
            FriendsService.Instance.RelationshipAdded += OnRelationshipAdded;
            FriendsService.Instance.RelationshipDeleted += OnRelationshipDeleted;
            FriendsService.Instance.PresenceUpdated += OnPresenceUpdated;
        }

        public void Dispose()
        {
            FriendsService.Instance.MessageReceived -= OnMessageReceived;
            FriendsService.Instance.RelationshipAdded -= OnRelationshipAdded;
            FriendsService.Instance.RelationshipDeleted -= OnRelationshipDeleted;
            FriendsService.Instance.PresenceUpdated -= OnPresenceUpdated;
        }

        public IReadOnlyList<Relationship> GetFriendsList() =>
            FriendsService.Instance.Friends;

        public IReadOnlyList<Relationship> GetIncomingRequests() =>
            FriendsService.Instance.IncomingFriendRequests;

        public IReadOnlyList<Relationship> GetOutgoingRequests() =>
            FriendsService.Instance.OutgoingFriendRequests;

        public IReadOnlyList<Relationship> GetBlockedPlayers() =>
            FriendsService.Instance.Blocks;

        public async void SendRequest(string playerId)
        {
            var result = await query.SendRequest(playerId);
            Command.Publish(new UpdateFriends());
            Command.Publish(new ShowMessage("Friend Request",
                result ? "Request sent successfully" : "Request failed"));
        }

        public async void SendRequestByName(string playerName)
        {
            var result = await query.SendRequestByName(playerName);
            Command.Publish(new UpdateFriends());
            Command.Publish(new ShowMessage("Friend Request",
                result ? "Request sent successfully" : "Request failed"));
        }

        public async void AcceptRequest(string playerId)
        {
            await query.AcceptRequest(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void DeclineRequest(string playerId)
        {
            await query.DeclineRequest(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void Delete(string playerId)
        {
            await query.Delete(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void DeleteIncomingRequest(string playerId)
        {
            await query.DeleteIncomingRequest(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void DeleteOutgoingRequest(string playerId)
        {
            await query.DeleteOutgoingRequest(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void Block(string playerId)
        {
            await query.Block(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void Unblock(string playerId)
        {
            await query.Unblock(playerId);
            Command.Publish(new UpdateFriends());
        }

        public async void SetPresence(PresenceAvailabilityOptions availability, FriendActivity activity) =>
            await query.SetPresence(availability, activity);

        public async void SetPresence(PresenceAvailabilityOptions availability) =>
            await query.SetPresence(availability);

        public async void SetPresence(FriendActivity activity) =>
            await query.SetPresence(activity);

        public async void SendMessage<T>(string playerId, T message) where T : new() =>
            await query.Message(playerId, message);

        private void OnMessageReceived(IMessageReceivedEvent messageEvent) =>
            Command.Publish(new FriendMessage
            {
                Id = messageEvent.UserId,
                Message = messageEvent.GetMessageAs<FriendMessageText>()
            });

        private void OnRelationshipAdded(IRelationshipAddedEvent obj) =>
            Command.Publish(new FriendRelationship
            {
                Added = true,
                Relationship = obj.Relationship
            });

        private void OnRelationshipDeleted(IRelationshipDeletedEvent obj) =>
            Command.Publish(new FriendRelationship
            {
                Added = false,
                Relationship = obj.Relationship
            });

        private void OnPresenceUpdated(IPresenceUpdatedEvent obj) =>
            Command.Publish(new FriendPresence
            {
                Id = obj.ID,
                Presence = obj.Presence,
                Activity = obj.Presence.GetActivity<FriendActivity>()
            });
    }
}