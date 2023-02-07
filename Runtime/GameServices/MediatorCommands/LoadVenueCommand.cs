using System;
using Framework;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GameServices.MediatorCommands
{
    public class LoadVenueCommand : IMediatorCommand
    {
        public AssetReference Asset;
        public LoadSceneMode Mode;
    }
}