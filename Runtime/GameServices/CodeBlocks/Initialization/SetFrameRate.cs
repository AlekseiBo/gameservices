using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Set Frame Rate", menuName = "Code Blocks/Initialization/Set Frame Rate", order = 0)]
    public class SetFrameRate : CodeBlock
    {
        [SerializeField] private int frameRate = 60;
        protected override void Execute()
        {
            Application.targetFrameRate = frameRate;
            Complete(true);
        }
    }
}