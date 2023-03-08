using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices
{
    [CreateAssetMenu(fileName = "VenueData", menuName = "Static Data/Venue Data")]
    public class VenueStaticData : ScriptableObject
    {
        public string Name;
        public string Address;
        public AssetReferenceScene AssetReference;
        public GameObject NetworkManager;
        public Sprite Icon;

        private void OnValidate()
        {
            if (AssetReference != null) Address = AssetReference.AssetGUID;
        }
    }
}