using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class PickColorButton : BaseButton
    {
        public Image Selector;

        private PickColorPanel panel;
        private int index;

        public void Construct(PickColorPanel panel, int index, Color color)
        {
            this.panel = panel;
            this.index = index;
            GetComponent<Image>().color = color;
        }

        protected override void OnClick()
        {
            panel.PickColor(index);
        }
    }
}