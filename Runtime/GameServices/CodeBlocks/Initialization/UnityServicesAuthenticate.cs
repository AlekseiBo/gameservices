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
            try
            {
                var randomId = $"Player-{UnityEngine.Random.Range(100, 1000).ToString()}";
                var initOptions = new InitializationOptions()
                    .With(e => e.SetProfile(randomId));

                await UnityServices.InitializeAsync(initOptions);

                AuthenticationService.Instance.SignedIn += () =>
                {
                    Debug.Log($"Signed in with Unity Services: {AuthenticationService.Instance.PlayerId}");
                    Complete(true);
                };
            }
            catch (Exception e)
            {
                Runner.LogMessage(e.Message);
                Complete(false);
            }

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
    }
}