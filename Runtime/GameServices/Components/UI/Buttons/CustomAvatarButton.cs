using Toolset;

namespace GameServices
{
    public class CustomAvatarButton : BaseButton
    {
        protected override void OnClick()
        {
            Command.Publish(new CustomizeAvatar());
        }
    }
}