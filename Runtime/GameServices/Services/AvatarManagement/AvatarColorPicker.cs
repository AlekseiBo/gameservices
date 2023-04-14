using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    public class AvatarColorPicker
    {
        public readonly Avatar Part;
        public readonly int CurrentSkinColor;
        public readonly List<Color> Palette;
        public readonly Action<Avatar, int> SetAction;

        public AvatarColorPicker(Avatar part, int currentSkinColor, List<Color> palette, Action<Avatar, int> setAction)
        {
            Part = part;
            CurrentSkinColor = currentSkinColor;
            Palette = palette;
            SetAction = setAction;
        }
    }
}