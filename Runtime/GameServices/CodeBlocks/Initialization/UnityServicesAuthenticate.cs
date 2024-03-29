﻿using System;
using UnityEngine;
using Toolset;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "UnityServicesAuthenticate",
        menuName = "Code Blocks/Initialization/Authenticate Unity Services", order = 0)]
    public class UnityServicesAuthenticate : CodeBlock
    {
        [SerializeField] private bool anonymouslyOnly;
        [SerializeField] private bool clearToken;
        [SerializeField] private string providerName = "oidc-advokate";
        [Space] [SerializeField] private string vivoxServer;
        [SerializeField] private string vivoxDomain;
        [SerializeField] private string vivoxIssuer;
        [SerializeField] private string vivoxKey;

        protected override async void Execute()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                try
                {
                    var netState = GameData.Get<NetState>(Key.PlayerNetState);
                    var initOptions = new InitializationOptions()
                        .With(e => e.SetProfile("RELAY_SERVER"), netState == NetState.Dedicated);

                    if (CheckVivoxCredentials())
                        initOptions.SetVivoxCredentials(vivoxServer, vivoxDomain, vivoxIssuer, vivoxKey);
                    
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
                        if (clearToken) AuthenticationService.Instance.ClearSessionToken();
                        await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(providerName, idToken);
                    }
                    else
                    {
                        Debug.Log("Signing in anonymously");
                        if (clearToken) AuthenticationService.Instance.ClearSessionToken();
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

            var authServiceNameBase = string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName)
                ? ""
                : AuthenticationService.Instance.PlayerName.Split('#')[0];

            if (authServiceNameBase != playerName)
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);

            Debug.Log(
                $"Signed in with Unity Services: {AuthenticationService.Instance.PlayerName} ({AuthenticationService.Instance.PlayerId})");
            Complete(true);
        }

        private bool CheckVivoxCredentials() =>
            !(string.IsNullOrEmpty(vivoxKey) && string.IsNullOrEmpty(vivoxIssuer) &&
              string.IsNullOrEmpty(vivoxDomain) && string.IsNullOrEmpty(vivoxServer));
    }
}