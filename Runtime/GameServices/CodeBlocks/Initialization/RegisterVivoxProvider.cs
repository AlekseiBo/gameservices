using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Vivox Provider", menuName = "Code Blocks/Initialization/Register Vivox Provider", order = 0)]
    public class RegisterVivoxProvider : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<IVivoxProvider>() == null)
                Services.All.RegisterSingle<IVivoxProvider>(new VivoxProvider());

            Complete(true);
        }
    }
}