using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Load Progress", menuName = "Code Blocks/Persistent Progress/Load", order = 0)]
    public class LoadProgress : CodeBlock
    {
        private IProgressProvider progress;
        protected override void Execute()
        {
            progress = Services.All.Single<IProgressProvider>();
            progress.LoadProgress();
            Complete(true);
        }
    }
}