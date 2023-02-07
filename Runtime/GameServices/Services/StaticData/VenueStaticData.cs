using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.StaticData
{
    [CreateAssetMenu(fileName = "VenueData", menuName = "Static Data/Venue Data")]
    public class VenueStaticData : ScriptableObject
    {
        public string Name;
        public AssetReference AssetReference;
        public Sprite Icon;
    }
}