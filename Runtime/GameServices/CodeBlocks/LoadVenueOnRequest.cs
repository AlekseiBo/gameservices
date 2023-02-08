using System;
using Framework;
using GameServices.AssetManagement;
using GameServices.GameDataService;
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
            OnLoadRequest();
        }

        private async void OnLoadRequest()
        {
            var sceneAsset = GameData.Get<string>(Key.SelectedVenue);
            await assets.LoadScene(sceneAsset);
            Completed?.Invoke(true);
        }
    }
}