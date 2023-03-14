using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace GameServices
{
    public struct CreateLobbyData
    {
        public readonly string Name;
        public readonly int MaxPlayers;
        public readonly bool IsPrivate;
        public readonly CreateLobbyOptions Options;

        public CreateLobbyData(string name, int maxPlayers, bool isPrivate)
        {
            Name = name;
            MaxPlayers = maxPlayers;
            IsPrivate = isPrivate;

            Options = new CreateLobbyOptions
            {
                IsPrivate = IsPrivate,
            };
        }
    }
}