using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices
{
    [CreateAssetMenu(fileName = "VenueData", menuName = "Static Data/Venue Data")]
    public class VenueStaticData : ScriptableObject
    {
        public string Name;
        public AssetReference AssetReference;
        public Sprite Icon;
    }
}