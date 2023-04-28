using Toolset;

namespace GameServices
{
    public class CustomizeAvatar : IMediatorCommand
    {
        public AvatarGroup CurrentGroup;

        public CustomizeAvatar(AvatarGroup currentGroup)
        {
            CurrentGroup = currentGroup;
        }
    }
}