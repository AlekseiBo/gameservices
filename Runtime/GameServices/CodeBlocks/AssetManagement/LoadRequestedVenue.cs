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
                var sceneInstance = await assets.LoadScene(newVenue, LoadSceneMode.Additive);
                var venueData = Services.All.Single<IStaticDataService>().ForVenue(newVenue);
                if (venueData.Skybox != null) RenderSettings.skybox = venueData.Skybox;
                GameData.Set(Key.CurrentVenue, newVenue);
                GameData.Set(Key.RequestedVenue, "");
                Command.Publish(new ShowVenueCanvas());
                Complete(true);
                return;
            }

            Complete(false);
        }
    }
}