using System;
using Toolset;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadScene", menuName = "Code Blocks/Assets/Load Scene Prefab", order = 0)]
    public class LoadScenePrefab : CodeBlock
    {
        [SerializeField] private string sceneName;
        [SerializeField] private bool additive;
        [SerializeField] private bool immediately;

        protected override void Execute()
        {
            if (immediately) Complete(true);

            try
            {
                SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single)
                    .completed += OnCompleted;
            }
            catch (Exception)
            {
                if (!immediately) Complete(false);
            }
        }

        private void OnCompleted(AsyncOperation result)
        {
            if (!immediately) Complete(result.isDone);
        }
    }
}