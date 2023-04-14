using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class PickSpriteButton : BaseButton
    {
        public Image Selector;

        private PickSpritePanel panel;
        private int index;

        public void Construct(PickSpritePanel panel, int index, Sprite sprite)
        {
            this.panel = panel;
            this.index = index;
            GetComponent<Image>().sprite = sprite;
        }

        protected override void OnClick()
        {
            panel.PickColor(index);
        }
    }
}