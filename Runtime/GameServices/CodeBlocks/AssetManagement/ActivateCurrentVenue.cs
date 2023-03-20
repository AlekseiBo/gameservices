using System.Collections;
using System.Threading.Tasks;
using Toolset;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Activate Current Venue", menuName = "Code Blocks/Assets/Activate Current Venue",
        order = 0)]
    public class ActivateCurrentVenue : CodeBlock
    {
        private IAssetProvider assets;

        protected override void Execute()
        {
            assets = Services.All.Single<IAssetProvider>();
            Runner.StartCoroutine(Activate());
        }

        private IEnumerator Activate()
        {
            var sceneAddress = GameData.Get<string>(Key.CurrentVenue);
            if (assets.SceneInstance(sceneAddress, out var instance))
            {
                var asyncOperation = instance.ActivateAsync();
                while (!asyncOperation.isDone) yield return null;
                Complete(true);
                //Complete(SceneManager.SetActiveScene(instance.Scene));
                yield break;
            }

            Complete(false);
        }
    }
}