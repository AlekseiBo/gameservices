using System;
using Toolset;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadActiveVenue", menuName = "Code Blocks/Assets/Load Active Venue", order = 0)]
    public class LoadActiveVenue : CodeBlock
    {
        private IAssetProvider assets;
        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Load();
        }

        private async void Load()
        {
            await assets.LoadScene(GameData.Get<string>(Key.ActiveVenue));
            Complete(true);
        }
    }
}