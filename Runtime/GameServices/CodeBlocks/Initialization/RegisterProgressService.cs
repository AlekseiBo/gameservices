using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Progress Service", menuName = "Code Blocks/Initialization/Register Progress Service", order = 0)]
    public class RegisterProgressService : CodeBlock
    {
        protected override async void Execute()
        {
            var saveLoadService = Services.All.Single<ISaveLoadService>();
            if (Services.All.Single<ISaveLoadService>() == null)
            {
                Services.All.RegisterSingle<ISaveLoadService>(new SaveLoadService());
                saveLoadService = Services.All.Single<ISaveLoadService>();
            }

            if (Services.All.Single<IProgressProvider>() == null)
            {
                Command.Publish(new LogMessage(LogType.Log, "Loading progress data"));
                var progress = new ProgressProvider(saveLoadService);
                await progress.InitializeProgress();
                Services.All.RegisterSingle<IProgressProvider>(progress);
            }

            Complete(true);
        }
    }
}