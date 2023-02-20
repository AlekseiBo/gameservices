using System;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "RegisterLobbyProvider", menuName = "Code Blocks/Initialization/Register Lobby Provider", order = 0)]
    public class RegisterLobbyProvider : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<ILobbyProvider>() == null)
                Services.All.RegisterSingle<ILobbyProvider>(
                    new LobbyProvider(Runner, Services.All.Single<IRelayProvider>()));

            Complete(true);
        }
    }
}