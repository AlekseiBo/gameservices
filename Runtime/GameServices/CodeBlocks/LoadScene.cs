﻿using System;
using Toolset;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadScene", menuName = "Code Blocks/Load Scene", order = 0)]
    public class LoadScene : CodeBlock
    {
        [SerializeField] private string sceneName;
        [SerializeField] private bool additive;

        protected override void Execute()
        {
            try
            {
                SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single)
                    .completed += OnCompleted;
            }
            catch (Exception)
            {
                Complete(false);
            }
        }

        private void OnCompleted(AsyncOperation result)
        {
                Complete(result.isDone);
        }
    }
}