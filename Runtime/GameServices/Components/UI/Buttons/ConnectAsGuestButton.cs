using Toolset;

namespace GameServices
{
    public class ConnectAsGuestButton  : BaseButton
    {
        protected override void OnClick()
        {
            GameData.Set(Key.PlayerNetState, NetState.Guest);
            Command.Publish(new ConnectToLobby());
        }
    }
}