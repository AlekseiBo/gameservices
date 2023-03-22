using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameServices
{
    internal class LobbyQuery
    {
        public async Task<QueryResponse> ByVenue(string venue)
        {
            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                        new(QueryFilter.FieldOptions.Name, venue, QueryFilter.OpOptions.CONTAINS),
                    },
                    Order = new List<QueryOrder>
                        { new(true, QueryOrder.FieldOptions.Created) }
                };

                var response = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                LogResponse(response);
                return response;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<QueryResponse> ByFriend(string friendId)
        {
            friendId = friendId[..8];

            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                        new(QueryFilter.FieldOptions.Name, friendId, QueryFilter.OpOptions.CONTAINS),
                    },
                    Order = new List<QueryOrder>
                        { new(false, QueryOrder.FieldOptions.Created) }
                };

                var response = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                LogResponse(response);
                return response;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        private static void LogResponse(QueryResponse response)
        {
            Debug.Log($"Query lobby count is {response.Results.Count}");
            foreach (var lobby in response.Results)
            {
                Debug.Log($"{lobby.Name}: privates = {lobby.IsPrivate}, max players = {lobby.MaxPlayers}");
            }
        }
    }
}