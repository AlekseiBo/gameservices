using Toolset;
using VivoxUnity;

namespace GameServices
{
    public interface IVivoxProvider : IService
    {
        LoginState LoginState { get; }
        void JoinChannel(string channelName, ChannelType channelType, ChatCapability chatCapability, bool transmissionSwitch = true, Channel3DProperties properties = null);
        void DisconnectAllChannels();
        void SendTextMessage(string messageToSend, ChannelId channel = null, string applicationStanzaNamespace = null, string applicationStanzaBody = null);
    }
}