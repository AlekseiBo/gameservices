using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    public class AvatarSelector<T>
    {
        public readonly Avatar Part;
        public readonly int CurrentSkinColor;
        public readonly List<T> Palette;
        public readonly Action<Avatar, int> SetAction;

        public AvatarSelector(Avatar part, int currentSkinColor, List<T> palette, Action<Avatar, int> setAction)
        {
            Part = part;
            CurrentSkinColor = currentSkinColor;
            Palette = palette;
            SetAction = setAction;
        }
    }
}