using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameServices
{
    internal class LobbyQuery
    {
        public async Task<QueryResponse> AllLobbies()
        {
            try
            {
                var response = await Lobbies.Instance.QueryLobbiesAsync();
                return response;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<QueryResponse> ByVenue(string venue)
        {
            try
            {
                var queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                        new(QueryFilter.FieldOptions.S1, venue, QueryFilter.OpOptions.EQ),
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

        public async Task<QueryResponse> ByOwner(string friendId)
        {
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
                Debug.Log($"{lobby.Name}: private = {lobby.IsPrivate}, max players = {lobby.MaxPlayers}");
            }
        }
    }
}