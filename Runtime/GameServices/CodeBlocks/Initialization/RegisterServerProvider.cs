using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Server Provider",
        menuName = "Code Blocks/Initialization/Register Server Provider", order = 0)]
    public class RegisterServerProvider : CodeBlock
    {
        [SerializeField] private GameObject serverStats;

        protected override void Execute()
        {
            if (Services.All.Single<IServerProvider>() == null)
            {
                Services.All.RegisterSingle<IServerProvider>(new ServerProvider());
                if (GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated && serverStats != null)
                    Instantiate(serverStats).With(s => s.name = "Server Stats");
            }

            Complete(true);
        }
    }
}