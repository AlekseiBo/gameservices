using Toolset;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Load Requested Venue", menuName = "Code Blocks/Assets/Load Requested Venue",
        order = 0)]
    public class LoadRequestedVenue : CodeBlock
    {
        private IAssetProvider assets;

        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Load();
        }

        private async void Load()
        {
            Command.Publish(new LogMessage(LogType.Log, "Loading venue assets"));
            var newVenue = GameData.Get<string>(Key.RequestedVenue);

            if (!string.IsNullOrEmpty(newVenue))
            {
                await assets.LoadScene(newVenue, LoadSceneMode.Additive);
                Command.Publish(new ShowVenueCanvas());
                GameData.Set(Key.CurrentVenue, newVenue);
                GameData.Set(Key.RequestedVenue, "");
                Complete(true);
                return;
            }

            Complete(false);
        }
    }
}