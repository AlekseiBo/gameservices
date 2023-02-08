using System;
using Framework;
using GameServices.GameDataService;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "CreateGameDataService", menuName = "Code Blocks/Create GameData Service", order = 0)]
    public class CreateGameDataService : CodeBlock
    {
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            new GameData();

            Completed?.Invoke(true);

        }
    }
}