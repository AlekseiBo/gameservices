using System;
using System.Collections.Generic;

namespace GameServices
{
    public class AvatarSelector<T>
    {
        public readonly Avatar Part;
        public readonly int CurrentIndex;
        public readonly List<T> Palette;
        public readonly Action<Avatar, int> SetAction;
        public readonly bool ResetEnabled;
        public readonly Avatar ResetPart;

        public AvatarSelector(Avatar part, int currentIndex, List<T> palette, Action<Avatar, int> setAction)
        {
            Part = part;
            CurrentIndex = currentIndex;
            Palette = palette;
            SetAction = setAction;
            ResetEnabled = false;
        }

        public AvatarSelector(Avatar part, int currentIndex, List<T> palette, Action<Avatar, int> setAction, Avatar resetPart)
        {
            Part = part;
            CurrentIndex = currentIndex;
            Palette = palette;
            SetAction = setAction;
            ResetEnabled = true;
            ResetPart = resetPart;
        }
    }
}