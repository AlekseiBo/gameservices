using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Toolset;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class PickSpritePanel : MonoBehaviour
    {
        [SerializeField] private GameObject pickerPrefab;
        [SerializeField] private Transform paletteTransform;

        private Avatar part;
        private Action<Avatar, int> pickAction;
        private readonly List<PickSpriteButton> buttonList = new();

        public void Construct(AvatarSelector<Sprite> selector)
        {
            pickAction = selector.SetAction;
            part = selector.Part;
            UpdatePalette(selector.Palette);
            BaseCanvas.UpdateLayout(paletteTransform);
            PickColor(selector.CurrentSkinColor);
        }

        public void PickColor(int index)
        {
            for (var i = 0; i < buttonList.Count; i++)
                buttonList[i].Selector.enabled = i == index;

            pickAction?.Invoke(part, index);
        }

        private void UpdatePalette(List<Sprite> palette)
        {
            buttonList.Clear();
            paletteTransform.ClearChildren();

            for (var i = 0; i < palette.Count; i++)
            {
                Instantiate(pickerPrefab, paletteTransform).GetComponent<PickSpriteButton>()
                    .With(p => p.Construct(this, i, palette[i]))
                    .With(p => buttonList.Add(p));
            }
        }
    }
}