using System;
using System.Collections;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "GameDataTest", menuName = "Code Blocks/Game Data/Test Block", order = 0)]
    public class GameDataTest : CodeBlock
    {
        protected override void Execute()
        {
            Runner.StartCoroutine(TestRun());
        }

        private IEnumerator TestRun()
        {
            Debug.Log(GameData.Subscribe<string>(Key.PlayerName, OnPlayerNameChanged));
            yield return Utilities.WaitFor(2f);
            GameData.Set(Key.PlayerName, "Bob");
            yield return Utilities.WaitFor(2f);
            GameData.RemoveSubscriber<string>(Key.PlayerName, OnPlayerNameChanged);
            yield return Utilities.WaitFor(2f);
            GameData.Set(Key.PlayerName, "Carl");
        }

        private void OnPlayerNameChanged(DataEntry<string> playerName)
        {
            Debug.Log(playerName.Value);
        }
    }
}