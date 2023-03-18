using Toolset;
using UnityEngine;

namespace GameServices
{
    [RequireComponent(typeof(CodeRunner))]
    public class GameRunnerListener : MonoBehaviour
    {
        private CodeRunner runner;

        private void Awake()
        {
            runner = GetComponent<CodeRunner>();
            Command.Subscribe<RunGame>(RunGame);
        }

        private void OnDestroy()
        {
            Command.RemoveSubscriber<RunGame>(RunGame);
        }

        private void RunGame(RunGame r)
        {
            runner.Run();
        }
    }
}