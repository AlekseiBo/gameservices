using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Vivox Provider", menuName = "Code Blocks/Initialization/Register Vivox Provider", order = 0)]
    public class RegisterVivoxProvider : CodeBlock
    {
        [SerializeField] private bool excludeOSX = true;

        protected override void Execute()
        {
            if (excludeOSX && Application.platform == RuntimePlatform.OSXEditor)
            {
                Complete(true);
                return;
            }

            if (Application.platform == RuntimePlatform.LinuxServer)
            {
                Complete(true);
                return;
            }

            if (Services.All.Single<IVivoxProvider>() == null)
                Services.All.RegisterSingle<IVivoxProvider>(new VivoxProvider());

            Complete(true);
        }
    }
}