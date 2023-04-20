using Toolset;

namespace GameServices
{
    public class SelectAvatarProfile : IMediatorCommand
    {
        public readonly AvatarGroup Group;

        public SelectAvatarProfile(AvatarGroup group = AvatarGroup.Any)
        {
            Group = group;
        }
    }
}