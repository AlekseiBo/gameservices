using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace GameServices
{
    internal class FriendsQuery
    {
        public async Task<bool> SendRequest(string playerId)
        {
            try
            {
                var relationship = await FriendsService.Instance.AddFriendAsync(playerId);
                Debug.Log($"Friend request sent to {playerId}.");
                return relationship.Type == RelationshipType.FRIEND_REQUEST;
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to Request {playerId} - {e}");
                return false;
            }
        }

        public async Task<bool> SendRequestByName(string playerName)
        {
            try
            {
                var relationship = await FriendsService.Instance.AddFriendByNameAsync(playerName);
                Debug.Log($"Friend request sent to {playerName}.");
                return relationship.Type == RelationshipType.FRIEND_REQUEST;
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to Request {playerName} - {e}");
                return false;
            }
        }

        public async Task Block(string playerId)
        {
            try
            {
                await FriendsService.Instance.AddBlockAsync(playerId);
                Debug.Log($"{playerId} was blocked");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to block {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task Unblock(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteBlockAsync(playerId);
                Debug.Log($"{playerId} was unblocked");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to unblock {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task AcceptRequest(string playerId)
        {
            try
            {
                await SendRequest(playerId);
                Debug.Log($"Friend request from {playerId} was accepted");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to accept request from {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task DeclineRequest(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteIncomingFriendRequestAsync(playerId);
                Debug.Log($"Friend request from {playerId} was declined");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to decline request from {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task Delete(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteFriendAsync(playerId);
                Debug.Log($"Friend {playerId} was removed");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to remove friend {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task DeleteIncomingRequest(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteIncomingFriendRequestAsync(playerId);
                Debug.Log($"Friend request from {playerId} was deleted");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to delete request from {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task DeleteOutgoingRequest(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteOutgoingFriendRequestAsync(playerId);
                Debug.Log($"Friend request for {playerId} was deleted");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to delete request for {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task Message<T>(string playerId, T message) where T : new()
        {
            try
            {
                await FriendsService.Instance.MessageAsync(playerId, message);
                Debug.Log($"Message was sent to {playerId}");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to send a message to {playerId}");
                Debug.LogError(e);
            }
        }

        public async Task SetPresence(PresenceAvailabilityOptions availability, FriendActivity activity)
        {
            try
            {
                await FriendsService.Instance.SetPresenceAsync(availability, activity);
                Debug.Log($"Presence updated");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to update presence");
                Debug.LogError(e);
            }
        }

        public async Task SetPresence(PresenceAvailabilityOptions availability)
        {
            try
            {
                await FriendsService.Instance.SetPresenceAvailabilityAsync(availability);
                Debug.Log($"Availability updated");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to update availability");
                Debug.LogError(e);
            }
        }

        public async Task SetPresence(FriendActivity activity)
        {
            try
            {
                await FriendsService.Instance.SetPresenceActivityAsync(activity);
                Debug.Log($"Activity updated");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to update activity");
                Debug.LogError(e);
            }
        }
    }
}