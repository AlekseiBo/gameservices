using System;
using UnityEngine;
using Toolset;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "UnityServicesAuthenticate", menuName = "Code Blocks/Initialization/Authenticate Unity Services", order = 0)]
    public class UnityServicesAuthenticate : CodeBlock
    {
        protected override async void Execute()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                try
                {
                    //var randomId = $"Player-{UnityEngine.Random.Range(100, 1000).ToString()}";

                    if (GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated)
                    {
                        var initOptions = new InitializationOptions()
                            .With(e => e.SetProfile("RELAY_SERVER"));
                        await UnityServices.InitializeAsync(initOptions);
                    }
                    else
                    {
                        await UnityServices.InitializeAsync();
                    }
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
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
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

        private void OnSignedIn()
        {
            AuthenticationService.Instance.SignedIn -= OnSignedIn;
            Debug.Log($"Signed in with Unity Services: {AuthenticationService.Instance.PlayerId}");
            Complete(true);
        }
    }
}