using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Avatar Provider", menuName = "Code Blocks/Initialization/Register Avatar Provider", order = 0)]
    public class RegisterAvatarProvider : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<IAvatarProvider>() == null)
                Services.All.RegisterSingle<IAvatarProvider>(new AvatarProvider());

            Complete(true);
        }
    }
}