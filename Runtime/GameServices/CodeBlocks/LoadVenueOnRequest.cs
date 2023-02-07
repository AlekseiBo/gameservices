using System;
using Framework;
using GameServices.AssetManagement;
using GameServices.MediatorCommands;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadVenueOnRequest", menuName = "Code Blocks/Asset Management/Load Venue On Request", order = 0)]
    public class LoadVenueOnRequest : CodeBlock
    {
        private IAssetProvider assets;
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            assets = Services.All.Single<IAssetProvider>();
            Mediator.Subscribe<LoadVenueCommand>(OnLoadRequest);
        }

        private async void OnLoadRequest(LoadVenueCommand command)
        {
            await assets.LoadScene(command.Asset.AssetGUID, command.Mode);
            Mediator.RemoveSubscriber<LoadVenueCommand>(OnLoadRequest);
            Completed?.Invoke(true);
        }
    }
}