using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Relay Provider", menuName = "Code Blocks/Initialization/Register Relay Provider", order = 0)]
    public class RegisterRelayProvider : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<IRelayProvider>() == null)
                Services.All.RegisterSingle<IRelayProvider>(new RelayProvider());

            Complete(true);
        }
    }
}