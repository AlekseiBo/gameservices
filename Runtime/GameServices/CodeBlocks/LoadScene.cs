using System;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadScene", menuName = "Code Blocks/Load Scene", order = 0)]
    public class LoadScene : CodeBlock
    {
        [SerializeField] private string sceneName;
        [SerializeField] private bool additive;

        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            try
            {
                SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single)
                    .completed += OnCompleted;
            }
            catch (Exception)
            {
                Completed?.Invoke(false);
            }
        }

        private void OnCompleted(AsyncOperation result)
        {
            Completed?.Invoke(result.isDone);
        }
    }
}