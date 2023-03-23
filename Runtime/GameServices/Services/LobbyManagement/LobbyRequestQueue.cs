using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServices
{
    internal class LobbyRequestQueue
    {
        private readonly Dictionary<LobbyRequestType, int> queue = new();
        private readonly Dictionary<LobbyRequestType, int> cooldown = new();

        public LobbyRequestQueue()
        {
            cooldown[LobbyRequestType.Query] = 1000;
            cooldown[LobbyRequestType.Create] = 3000;
            cooldown[LobbyRequestType.Join] = 3000;
            cooldown[LobbyRequestType.QuickJoin] = 10000;
            cooldown[LobbyRequestType.GetLobby] = 1000;
            cooldown[LobbyRequestType.DeleteLobby] = 500;
            cooldown[LobbyRequestType.UpdateLobby] = 1000;
            cooldown[LobbyRequestType.UpdatePlayer] = 1000;
            cooldown[LobbyRequestType.LeaveLobbyOrRemovePlayer] = 200;
            cooldown[LobbyRequestType.HeartBeat] = 6000;

            queue[LobbyRequestType.Query] = 0;
            queue[LobbyRequestType.Create] = 0;
            queue[LobbyRequestType.Join] = 0;
            queue[LobbyRequestType.QuickJoin] = 0;
            queue[LobbyRequestType.GetLobby] = 0;
            queue[LobbyRequestType.DeleteLobby] = 0;
            queue[LobbyRequestType.UpdateLobby] = 0;
            queue[LobbyRequestType.UpdatePlayer] = 0;
            queue[LobbyRequestType.LeaveLobbyOrRemovePlayer] = 0;
            queue[LobbyRequestType.HeartBeat] = 0;
        }
    }
}