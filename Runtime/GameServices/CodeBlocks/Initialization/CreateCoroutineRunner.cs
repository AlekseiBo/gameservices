using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "CreateCoroutineRunner",
        menuName = "Code Blocks/Initialization/Create Coroutine Runner", order = 0)]
    public class CreateCoroutineRunner : CodeBlock
    {
        [SerializeField] private GameObject prefab;

        protected override void Execute()
        {
            if (!CoroutineRunner.isInitialized)
                new CoroutineRunner(Instantiate(prefab)
                    .With(e => e.name = "Coroutine Runner")
                    .GetComponent<CoroutineComponent>());

            Complete(true);
        }
    }
}