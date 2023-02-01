using System;
using Framework;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "RegisterSampleService", menuName = "Code Blocks/Register Sample Service", order = 0)]
    public class RegisterSampleService : CodeBlock
    {
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            string message = "This is simple service";
            Services.All.RegisterSingle<ISampleService>(new SampleService(message));
            Completed?.Invoke(true);
        }
    }
}