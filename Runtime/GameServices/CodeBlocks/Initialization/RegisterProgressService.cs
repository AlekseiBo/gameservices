using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Progress Service", menuName = "Code Blocks/Initialization/Register Progress Service", order = 0)]
    public class RegisterProgressService : CodeBlock
    {
        protected override void Execute()
        {
            var saveLoadService = Services.All.Single<ISaveLoadService>();
            if (Services.All.Single<ISaveLoadService>() == null)
            {
                Services.All.RegisterSingle<ISaveLoadService>(new SaveLoadService());
                saveLoadService = Services.All.Single<ISaveLoadService>();
            }

            if (Services.All.Single<IProgressProvider>() == null)
            {
                Services.All.RegisterSingle<IProgressProvider>(new ProgressProvider(saveLoadService));
            }

            Complete(true);
        }
    }
}