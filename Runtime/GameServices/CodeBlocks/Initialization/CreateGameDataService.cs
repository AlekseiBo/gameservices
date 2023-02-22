using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "CreateGameDataService", menuName = "Code Blocks/Initialization/Create GameData", order = 0)]
    public class CreateGameDataService : CodeBlock
    {
        private bool isInitialized = false;

        protected override void Execute()
        {
            if (!isInitialized)
            {
                new GameData();
                isInitialized = true;
            }

            Complete(true);
        }
    }
}