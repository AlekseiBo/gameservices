using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "RegisterAssetProvider", menuName = "Code Blocks/Initialization/Register Asset Provider", order = 0)]
    public class RegisterAssetProvider : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<IAssetProvider>() == null)
                Services.All.RegisterSingle<IAssetProvider>(new AssetProvider());

            Complete(true);
        }
    }
}