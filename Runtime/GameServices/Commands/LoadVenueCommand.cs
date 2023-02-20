using Toolset;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GameServices.Commands
{
    public class LoadVenueCommand : IMediatorCommand
    {
        public AssetReference Asset;
        public LoadSceneMode Mode;
    }
}