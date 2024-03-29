﻿using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Wait For Connection Request",
        menuName = "Code Blocks/Communication/Wait For Connection Request", order = 0)]
    public class WaitForConnectionRequest : CodeBlock
    {
        protected override void Execute()
        {
            Command.RemoveSubscriber<ConnectToLobby>(Requested);

            var requestedVenue = GameData.Get<string>(Key.RequestedVenue);
            if (!string.IsNullOrEmpty(requestedVenue))
            {
                Complete(true);
                return;
            }

            var requestedCode = GameData.Get<string>(Key.RequestedLobbyCode);
            var netState = GameData.Get<NetState>(Key.PlayerNetState);
            if (!string.IsNullOrEmpty(requestedCode) && netState == NetState.Guest)
            {
                Complete(true);
                return;
            }

            WaitForConnection();
        }

        private void WaitForConnection()
        {
            Command.Publish(new ShowVenueSelection());
            Command.Subscribe<ConnectToLobby>(Requested);
        }

        private void Requested(ConnectToLobby c)
        {
            Command.RemoveSubscriber<ConnectToLobby>(Requested);
            Complete(true);
        }
    }
}