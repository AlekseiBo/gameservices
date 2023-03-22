using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Friend List", menuName = "Code Blocks/Initialization/Register Friend List", order = 0)]
    public class RegisterFriendList : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<IFriendListProvider>() == null)
                Services.All.RegisterSingle<IFriendListProvider>(new FriendListProvider());

            Complete(true);
        }
    }
}