using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Save Progress", menuName = "Code Blocks/Persistent Progress/Save", order = 0)]
    public class SaveProgress : CodeBlock
    {
        private IProgressProvider progress;
        protected override async void Execute()
        {
            progress = Services.All.Single<IProgressProvider>();
            await progress.SaveProgress();
            Complete(true);
        }
    }
}