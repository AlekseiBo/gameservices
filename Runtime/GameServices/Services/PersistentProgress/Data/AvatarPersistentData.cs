using System;

namespace GameServices
{
    [Serializable]
    public class AvatarPersistentData
    {
        public string Prefab;
        public AvatarGroup Group;
        public int Hair;
        public int Hat;
        public int Top;
        public int Bottom;
        public int Shoes;
        public int SkinColor;
        public int HairColor;
        public int EyeColor;
        public int OutfitColor;
    }
}