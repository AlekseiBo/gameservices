using Toolset;

namespace GameServices
{
    public class UpdateFriendList : IMediatorCommand
    {
        public bool Add;
        public string Name;
        public string Id;

        public UpdateFriendList(bool add, string name, string id)
        {
            Add = add;
            Name = name;
            Id = id;
        }

        public UpdateFriendList(bool add, string id)
        {
            Add = add;
            Id = id;
        }
    }
}