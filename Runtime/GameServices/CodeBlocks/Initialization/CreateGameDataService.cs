using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "CreateGameDataService", menuName = "Code Blocks/Initialization/Create GameData Service", order = 0)]
    public class CreateGameDataService : CodeBlock
    {
        protected override void Execute()
        {
            new GameData();
            Complete(true);

        }
    }
}