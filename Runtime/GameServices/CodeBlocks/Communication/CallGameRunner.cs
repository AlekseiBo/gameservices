using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Call Game Runner", menuName = "Code Blocks/Communication/Call Game Runner", order = 0)]
    public class CallGameRunner : CodeBlock
    {
        protected override void Execute()
        {
            Command.Publish(new RunGame());
        }
    }
}