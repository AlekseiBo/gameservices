using UnityEngine;

namespace GameServices
{
    public class CategoryPanel : MonoBehaviour
    {
        [SerializeField] private PickColorPanel skinColorPanel;
        [SerializeField] private PickColorPanel hairColorPanel;
        [SerializeField] private PickColorPanel eyeColorPanel;
        [SerializeField] private PickColorPanel outfitColorPanel;

        public void Construct(
            AvatarColorPicker skinColorPicker,
            AvatarColorPicker hairColorPicker,
            AvatarColorPicker eyeColorPicker,
            AvatarColorPicker outfitColorPicker)
        {
            skinColorPanel.Construct(skinColorPicker);
            hairColorPanel.Construct(hairColorPicker);
            eyeColorPanel.Construct(eyeColorPicker);
            outfitColorPanel.Construct(outfitColorPicker);
        }
    }
}