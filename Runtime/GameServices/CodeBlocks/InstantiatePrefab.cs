using System;
using Framework;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "InstantiatePrefab", menuName = "Code Blocks/Instantiate Prefab", order = 0)]
    public class InstantiatePrefab : CodeBlock
    {
        [SerializeField] private GameObject prefab;

        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            if (prefab != null)
            {
                Instantiate(prefab);
                Completed?.Invoke(true);
            }
            else
            {
                Completed?.Invoke(false);
            }
        }
    }
}