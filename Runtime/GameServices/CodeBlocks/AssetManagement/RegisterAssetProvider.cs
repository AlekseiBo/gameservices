using System;
using Framework;
using GameServices.AssetManagement;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "RegisterAssetProvider", menuName = "Code Blocks/Asset Management/Register Service", order = 0)]
    public class RegisterAssetProvider : CodeBlock
    {
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            if (Services.All.Single<IAssetProvider>() == null)
                Services.All.RegisterSingle<IAssetProvider>(new AssetProvider());

            Completed?.Invoke(true);
        }
    }
}