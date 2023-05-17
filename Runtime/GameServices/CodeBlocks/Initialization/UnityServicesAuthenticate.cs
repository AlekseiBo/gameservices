using System;
using UnityEngine;
using Toolset;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "UnityServicesAuthenticate",
        menuName = "Code Blocks/Initialization/Authenticate Unity Services", order = 0)]
    public class UnityServicesAuthenticate : CodeBlock
    {
        [SerializeField] private bool anonymouslyOnly;
        [SerializeField] private string providerName = "oidc-advokate";

        protected override async void Execute()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                try
                {
                    var netState = GameData.Get<NetState>(Key.PlayerNetState);
                    var initOptions = new InitializationOptions()
                        .With(e => e.SetProfile("RELAY_SERVER"), netState == NetState.Dedicated);

                    await UnityServices.InitializeAsync(initOptions);
                }
                catch (Exception e)
                {
                    Runner.LogMessage(e.Message);
                    Complete(false);
                    return;
                }
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignedIn += OnSignedIn;

                try
                {
                    var idToken = GameData.Get<string>(Key.UnityToken);
                    if (!anonymouslyOnly && idToken != "")
                    {
                        Debug.Log("Signing in with Open ID");
                        await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(providerName, idToken);
                    }
                    else
                    {
                        Debug.Log("Signing in anonymously");
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    }
                }
                catch (AuthenticationException e)
                {
                    Runner.LogMessage(e.Message);
                    Complete(false);
                }
            }
            else
            {
                Complete(true);
            }
        }

        private async void OnSignedIn()
        {
            AuthenticationService.Instance.SignedIn -= OnSignedIn;
            var playerName = GameData.Get<string>(Key.PlayerName);
            var authServiceNameBase = AuthenticationService.Instance.PlayerName.Split('#')[0];

            if (authServiceNameBase != playerName)
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);

            Debug.Log($"Signed in with Unity Services: {AuthenticationService.Instance.PlayerName} ({AuthenticationService.Instance.PlayerId})");
            Complete(true);
        }
    }
}