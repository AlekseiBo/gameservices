using System;
using System.Collections.Generic;

using Toolset;
using UnityEngine;

namespace GameServices
{
    public class PickColorPanel : MonoBehaviour
    {
        [SerializeField] private GameObject pickerPrefab;
        [SerializeField] private Transform paletteTransform;

        private Avatar part;
        private Action<Avatar, int> pickAction;
        private readonly List<PickColorButton> buttonList = new();

        private void OnEnable()
        {
            BaseCanvas.UpdateLayout(paletteTransform);
        }

        public void Construct(AvatarSelector<Color> selector)
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

        private void UpdatePalette(List<Color> palette)
        {
            buttonList.Clear();
            paletteTransform.ClearChildren();

            for (var i = 0; i < palette.Count; i++)
            {
                Instantiate(pickerPrefab, paletteTransform).GetComponent<PickColorButton>()
                    .With(p => p.Construct(this, i, palette[i]))
                    .With(p => buttonList.Add(p));
            }
        }
    }
}