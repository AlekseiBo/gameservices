using System;
using System.Collections;
using Framework;
using GameServices.GameDataService;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "GameDataTest", menuName = "Code Blocks/Game Data/Test Block", order = 0)]
    public class GameDataTest : CodeBlock
    {
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            Runner.StartCoroutine(TestRun());
        }

        private IEnumerator TestRun()
        {
            Debug.Log(GameData.Subscribe<string>(DataKey.PlayerName, OnPlayerNameChanged));
            yield return Utilities.WaitFor(2f);
            GameData.Set<string>(DataKey.PlayerName, "Bob");
            yield return Utilities.WaitFor(2f);
            GameData.RemoveSubscriber<string>(DataKey.PlayerName, OnPlayerNameChanged);
            yield return Utilities.WaitFor(2f);
            GameData.Set<string>(DataKey.PlayerName, "Carl");
        }

        private void OnPlayerNameChanged(DataEntry<string> playerName)
        {
            Debug.Log(playerName.Value);
        }
    }
}