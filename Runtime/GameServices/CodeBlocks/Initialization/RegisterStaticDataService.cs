﻿using System;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "RegisterStaticDataService", menuName = "Code Blocks/Initialization/Register Static Data", order = 0)]
    public class RegisterStaticDataService : CodeBlock
    {
        protected override void Execute()
        {
            if (Services.All.Single<IStaticDataService>() == null)
                Services.All.RegisterSingle<IStaticDataService>(new StaticDataService());

            Complete(true);
        }
    }
}