using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    [Serializable]
    public class AvatarItem
    {
        public Sprite Icon;
        public List<GameObject> ObjectList;
        public List<GameObject> ExcludeList;

        public void Activate(bool activate = true)
        {
            if (activate) ExcludeList.ForEach(o => o.SetActive(false));
            ObjectList.ForEach(o => o.SetActive(activate));
        }
    }
}