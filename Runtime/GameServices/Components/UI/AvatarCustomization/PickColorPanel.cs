using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Toolset;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class PickColorPanel : MonoBehaviour
    {
        [SerializeField] private GameObject pickerPrefab;
        [SerializeField] private Transform paletteTransform;

        private Avatar part;
        private Action<Avatar, int> pickAction;
        private List<PickColorButton> buttonList = new();

        public void Construct(AvatarColorPicker colorPicker)
        {
            pickAction = colorPicker.SetAction;
            part = colorPicker.Part;
            UpdatePalette(colorPicker.Palette);
            PickColor(colorPicker.CurrentSkinColor);
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