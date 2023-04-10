using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "CreateGameData", menuName = "Code Blocks/Initialization/Create GameData", order = 0)]
    public class CreateGameData : CodeBlock
    {
        [SerializeField] private string resourcePath = "GameData";
        protected override void Execute()
        {
            if (!GameData.isInitialized) new GameData(resourcePath);
            Complete(true);
        }
    }
}