﻿using System;
using System.ComponentModel;
using Toolset;
using Unity.Services.Vivox;
using UnityEngine;
using VivoxUnity;

namespace GameServices
{
    public class VivoxProvider : IVivoxProvider, IDisposable
    {
        public LoginState LoginState { get; private set; }

        private Account account;
        private Client client => VivoxService.Instance.Client;
        private ILoginSession loginSession;
        private ChannelId currentChannel;

        public VivoxProvider()
        {
            VivoxService.Instance.Initialize();
            Login();
        }

        public void JoinChannel(string channelName, ChannelType channelType, ChatCapability chatCapability,
            bool transmissionSwitch = true, Channel3DProperties properties = null)
        {
            if (LoginState == LoginState.LoggedIn)
            {
                Channel channel = new Channel(channelName, channelType, properties);

                IChannelSession channelSession = loginSession.GetChannelSession(channel);
                channelSession.PropertyChanged += OnChannelPropertyChanged;
                channelSession.MessageLog.AfterItemAdded += OnMessageLogReceived;
                channelSession.BeginConnect(chatCapability != ChatCapability.TextOnly,
                    chatCapability != ChatCapability.AudioOnly, transmissionSwitch, channelSession.GetConnectToken(),
                    ar =>
                    {
                        try
                        {
                            channelSession.EndConnect(ar);
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Could not connect to voice channel: {e.Message}");
                            channelSession.PropertyChanged -= OnChannelPropertyChanged;
                            channelSession.MessageLog.AfterItemAdded -= OnMessageLogReceived;
                            return;
                        }

                        currentChannel = channel;
                    });
            }
            else
            {
                Debug.Log("Cannot join a channel when not logged in.");
            }
        }

        public void DisconnectAllChannels()
        {
            if (!(loginSession?.ChannelSessions.Count > 0)) return;

            foreach (var channelSession in loginSession.ChannelSessions)
            {
                channelSession?.Disconnect();
            }
        }

        public void SendTextMessage(string messageToSend, ChannelId channel = null,
            string applicationStanzaNamespace = null, string applicationStanzaBody = null)
        {
            if (string.IsNullOrEmpty(messageToSend)) return;
            if (ChannelId.IsNullOrEmpty(channel)) channel = currentChannel;

            var channelSession = loginSession.GetChannelSession(channel);
            channelSession.BeginSendText(null, messageToSend, applicationStanzaNamespace, applicationStanzaBody, ar =>
            {
                try
                {
                    channelSession.EndSendText(ar);
                }
                catch (Exception e)
                {
                    Debug.Log($"SendTextMessage failed with exception {e.Message}");
                }
            });
        }

        public void Dispose()
        {
            if (loginSession != null && LoginState != LoginState.LoggedOut && LoginState != LoginState.LoggingOut)
            {
                loginSession.PropertyChanged -= OnLoginSessionPropertyChanged;
                loginSession.Logout();
            }
        }

        private void Login()
        {
            account = new Account(GameData.Get<string>(Key.PlayerName));
            loginSession = client.GetLoginSession(account);
            loginSession.PropertyChanged += OnLoginSessionPropertyChanged;
            loginSession.BeginLogin(loginSession.GetLoginToken(), SubscriptionMode.Accept, null, null, null, ar =>
            {
                try
                {
                    loginSession.EndLogin(ar);
                }
                catch (Exception e)
                {
                    Debug.Log(nameof(e));
                    loginSession.PropertyChanged -= OnLoginSessionPropertyChanged;
                    return;
                }
            });
        }

        private void OnLoginSessionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "RecoveryState") return;

            if (propertyChangedEventArgs.PropertyName != "State") return;

            var loginSession = (ILoginSession)sender;
            LoginState = loginSession.State;
            Debug.Log("Detecting login session change");
            switch (LoginState)
            {
                case LoginState.LoggingIn:
                {
                    Debug.Log("Logging in");
                    break;
                }
                case LoginState.LoggedIn:
                {
                    Debug.Log("Connected to voice server and logged in.");
                    break;
                }
                case LoginState.LoggingOut:
                {
                    Debug.Log("Logging out");
                    break;
                }
                case LoginState.LoggedOut:
                {
                    Debug.Log("Logged out");
                    this.loginSession.PropertyChanged -= OnLoginSessionPropertyChanged;
                    break;
                }
                default:
                    break;
            }
        }

        private void OnChannelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var channelSession = (IChannelSession)sender;

            if ((propertyChangedEventArgs.PropertyName == "AudioState" ||
                 propertyChangedEventArgs.PropertyName == "TextState") &&
                channelSession.AudioState == ConnectionState.Disconnected &&
                channelSession.TextState == ConnectionState.Disconnected)
            {
                Debug.Log($"Unsubscribing from: {channelSession.Key.Name}");
                channelSession.PropertyChanged -= OnChannelPropertyChanged;
                channelSession.MessageLog.AfterItemAdded -= OnMessageLogReceived;
                currentChannel = null;

                var user = client.GetLoginSession(account);
                user.DeleteChannelSession(channelSession.Channel);
            }
        }

        private void OnMessageLogReceived(object sender, QueueItemAddedEventArgs<IChannelTextMessage> textMessage) =>
            Command.Publish(new AddChatMessage(textMessage.Value));
    }
}