using UnityEngine;

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
        public AvatarGroup AvatarGroup;
        [TextArea(2, 5)] public string Description;

        private void OnValidate()
        {
            if (AssetReference != null) Address = AssetReference.AssetGUID;
        }
    }
}