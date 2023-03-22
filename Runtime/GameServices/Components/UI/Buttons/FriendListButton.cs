using Toolset;

namespace GameServices
{
    public class FriendListButton : BaseButton
    {
        protected override void OnClick()
        {
            Command.Publish(new ShowFriendList());
        }
    }
}