using System.Runtime.InteropServices.WindowsRuntime;
using GameServices.Commands;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Wait For Connection Request", menuName = "Code Blocks/Communication/Wait For Connection Request", order = 0)]
    public class WaitForConnectionRequest : CodeBlock
    {
        [SerializeField] private AssetReference menuAsset;

        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();

            if (GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated)
                Complete(true);
            else
                WaitForConnection();
        }

        private async void WaitForConnection()
        {
            var menu = await assets.Instantiate(menuAsset.AssetGUID);

            if (menu != null)
                Mediator.Subscribe<ConnectionRequest>(Requested);
            else
                Complete(false);

        }

        private void Requested(ConnectionRequest c)
        {
            Mediator.RemoveSubscriber<ConnectionRequest>(Requested);
            Complete(true);
        }
    }
}