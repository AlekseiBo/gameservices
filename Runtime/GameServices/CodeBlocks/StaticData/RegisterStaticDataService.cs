using System;
using Framework;
using GameServices.StaticData;
using UnityEngine;

namespace GameServices.CodeBlocks.StaticData
{
    [CreateAssetMenu(fileName = "RegisterStaticDataService", menuName = "Code Blocks/Static Data/Register Service", order = 0)]
    public class RegisterStaticDataService : CodeBlock
    {
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            if (Services.All.Single<IStaticDataService>() == null)
                Services.All.RegisterSingle<IStaticDataService>(new StaticDataService());

            Completed?.Invoke(true);
        }
    }
}