using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using UnityEngine;

namespace GameServices
{
    public class FriendsProvider : IFriendsProvider, IDisposable
    {
        public const string CON_SEP = ";;";
        public const string MES_SEP = "::";
        public Dictionary<string, string> FriendConversations { get; }

        private readonly FriendsQuery query;

        public FriendsProvider()
        {
            query = new FriendsQuery();
            FriendConversations = new();
            LoadConversations();
            FriendsService.Instance.MessageReceived += OnMessageReceived;
            FriendsService.Instance.RelationshipAdded += OnRelationshipAdded;
            FriendsService.Instance.RelationshipDeleted += OnRelationshipDeleted;
            FriendsService.Instance.PresenceUpdated += OnPresenceUpdated;

            GameData.Subscribe<string>(Key.CurrentLobbyCode, OnLobbyCodeSet);
        }

        private void OnLobbyCodeSet<T>(DataEntry<T> dataEntry)
        {
            var codeEntry = dataEntry as DataEntry<string>;
            var venue = GameData.Get<string>(Key.CurrentVenue);

            var activity = new FriendActivity
            {
                Venue = string.IsNullOrEmpty(venue) ? "Lobby" : venue,
                Code = codeEntry.Value
            };

            Services.All.Single<IFriendsProvider>().SetPresence(activity);
        }

        public void Dispose()
        {
            FriendsService.Instance.MessageReceived -= OnMessageReceived;
            FriendsService.Instance.RelationshipAdded -= OnRelationshipAdded;
            FriendsService.Instance.RelationshipDeleted -= OnRelationshipDeleted;
            FriendsService.Instance.PresenceUpdated -= OnPresenceUpdated;

            GameData.RemoveSubscriber<string>(Key.CurrentLobbyCode, OnLobbyCodeSet);
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

        public async Task SetPresence(PresenceAvailabilityOptions availability, FriendActivity activity) =>
            await query.SetPresence(availability, activity);

        public async Task SetPresence(PresenceAvailabilityOptions availability) =>
            await query.SetPresence(availability);

        public async Task SetPresence(FriendActivity activity) =>
            await query.SetPresence(activity);

        public async void SendMessage<T>(string playerId, T message) where T : new() =>
            await query.Message(playerId, message);

        public async Task RefreshRelationships() =>
            await query.ForceRelationshipsRefresh();


        public void LoadConversations()
        {
            FriendConversations.Clear();

            var friends = PlayerPrefs.GetString("Friends", "");
            foreach (var friendId in friends.Split(CON_SEP))
            {
                FriendConversations[friendId] = "";
                foreach (var message in PlayerPrefs.GetString(friendId, "").Split(CON_SEP))
                {
                    var messagePair = message.Split(MES_SEP);
                    if (messagePair.Length == 2)
                    {
                        FriendConversations[friendId] += $"{message}{CON_SEP}";
                    }
                }
            }
        }

        public void DeleteConversation(string playerId)
        {
            if (FriendConversations.ContainsKey(playerId))
                FriendConversations.Remove(playerId);

            PlayerPrefs.DeleteKey(playerId);
            SaveConversations();
        }

        public void AddMessageToConversation(string playerId, string message, bool incoming)
        {
            var safeMessage = message.Replace(MES_SEP, ":").Replace(CON_SEP, ";");
            var code = incoming ? "0" : "1";
            var text = $"{code}{MES_SEP}{safeMessage}{CON_SEP}";
            FriendConversations[playerId] = FriendConversations.TryGetValue(playerId, out var conversationText)
                ? text + conversationText
                : text;

            SaveConversations();
        }

        private void SaveConversations()
        {
            var conversationList = "";
            foreach (var item in FriendConversations)
            {
                PlayerPrefs.SetString(item.Key, item.Value);
                conversationList += $"{item.Key}{CON_SEP}";
            }

            PlayerPrefs.SetString("Friends", conversationList);
            PlayerPrefs.Save();
        }

        private void OnMessageReceived(IMessageReceivedEvent messageEvent)
        {
            var message = messageEvent.GetMessageAs<FriendMessageText>();

            if (message != null)
            {
                AddMessageToConversation(messageEvent.UserId, message.Text, true);

                Command.Publish(new FriendMessage
                {
                    Id = messageEvent.UserId,
                    Message = message
                });
            }

            var invitation = messageEvent.GetMessageAs<FriendMessageInvite>();

            if (invitation != null)
            {
                Command.Publish(new ShowDialog("Invitation", $"Would you like to join {invitation.JoinCode}"));
            }
        }

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

        private void OnPresenceUpdated(IPresenceUpdatedEvent obj)
        {
            Debug.Log("Presence updated event");
            Command.Publish(new FriendPresence
            {
                Id = obj.ID,
                Availability = obj.Presence.Availability,
                LastSeen = obj.Presence.LastSeen,
                Activity = obj.Presence.GetActivity<FriendActivity>()
            });
        }
    }
}