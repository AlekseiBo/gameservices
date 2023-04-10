using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "CreateAvatarData", menuName = "Code Blocks/Initialization/Create AvatarData", order = 0)]
    public class CreateAvatarData : CodeBlock
    {
        [SerializeField] private string resourcePath = "AvatarData";
        protected override void Execute()
        {
            if (!AvatarData.isInitialized) new AvatarData(resourcePath);
            Complete(true);
        }
    }
}