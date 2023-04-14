using UnityEngine;

namespace GameServices
{
    public class CategoryPanel : MonoBehaviour
    {
        [SerializeField] private PickSpritePanel hairModelPanel;
        [SerializeField] private PickColorPanel skinColorPanel;
        [SerializeField] private PickColorPanel hairColorPanel;
        [SerializeField] private PickColorPanel eyeColorPanel;
        [SerializeField] private PickColorPanel outfitColorPanel;

        public void Construct(
            AvatarSelector<Sprite> hairModelSelector,
            AvatarSelector<Color> skinSelector,
            AvatarSelector<Color> hairSelector,
            AvatarSelector<Color> eyeSelector,
            AvatarSelector<Color> outfitSelector)
        {
            hairModelPanel.Construct(hairModelSelector);
            skinColorPanel.Construct(skinSelector);
            hairColorPanel.Construct(hairSelector);
            eyeColorPanel.Construct(eyeSelector);
            outfitColorPanel.Construct(outfitSelector);
        }
    }
}