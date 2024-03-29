using UnityEngine;

namespace GameServices
{
    [CreateAssetMenu(fileName = "VenueData", menuName = "Static Data/Venue Data")]
    public class VenueStaticData : ScriptableObject
    {
        public string Name;
        public string Address;
        public AssetReferenceScene AssetReference;
        public Material Skybox;
        public GameObject NetworkManager;
        public GameObject ServerManager;
        public Sprite Icon;
        public bool ShowInMenu;
        public bool MoveUp;
        public AvatarGroup AvatarGroup;
        public NetState NetState;
        [TextArea(2, 5)] public string Description;

        private void OnValidate()
        {
            if (AssetReference != null) Address = AssetReference.AssetGUID;
        }
    }
}