using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public struct CreateLobbyData
    {
        public readonly string Name;
        public readonly string Venue;
        public readonly int MaxPlayers;
        public readonly bool IsPrivate;
        public readonly CreateLobbyOptions Options;

        public CreateLobbyData(string name, string venue, int maxPlayers, bool isPrivate)
        {
            Name = name;
            Venue = venue;
            MaxPlayers = maxPlayers;
            IsPrivate = isPrivate;

            Options = new CreateLobbyOptions
            {
                IsPrivate = IsPrivate,
                Data = new Dictionary<string, DataObject>
                    { { "VENUE", new DataObject(DataObject.VisibilityOptions.Public, Venue, DataObject.IndexOptions.S1) } }
            };
        }
    }
}