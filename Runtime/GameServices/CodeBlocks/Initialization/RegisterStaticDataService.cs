using System;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "RegisterStaticDataService", menuName = "Code Blocks/Initialization/Register Static Data", order = 0)]
    public class RegisterStaticDataService : CodeBlock
    {
        protected override async void Execute()
        {
            if (Services.All.Single<IStaticDataService>() == null)
            {
                Command.Publish(new LogMessage(LogType.Log, "Loading venues"));
                var staticData = new StaticDataService();
                await staticData.LoadData();
                Services.All.RegisterSingle<IStaticDataService>(staticData);
            }

            Complete(true);
        }
    }
}