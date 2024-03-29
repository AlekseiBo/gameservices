﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameServices
{
    public class AvatarController : MonoBehaviour
    {
        public Vector3 Preview;
        public UnityEngine.Avatar AnimatorAvatar;
        public AvatarGroup Group;
        [Space]
        [SerializeField] private List<AvatarItem> hairList;
        [SerializeField] private List<RendererMaterial> hairMesh;
        [SerializeField] private List<Color> hairColors;
        [Space]
        [SerializeField] private List<AvatarItem> hatList;
        [Space]
        [SerializeField] private List<AvatarItem> topList;
        [Space]
        [SerializeField] private List<AvatarItem> bottomList;
        [Space]
        [SerializeField] private List<AvatarItem> shoesList;
        [Space]
        [SerializeField] private List<RendererMaterial> skinMesh;
        [SerializeField] private List<Color> skinColors;
        [Space]
        [SerializeField] private List<RendererMaterial> eyeMesh;
        [SerializeField] private List<Color> eyeColors;
        [Space]
        [SerializeField] private List<RendererMaterial> outfitMesh;
        [SerializeField] private List<Color> outfitColors;

        public List<Sprite> HairIcons => hairList.ConvertAll(i => i.Icon);
        public List<Sprite> HatIcons => hatList.ConvertAll(i => i.Icon);
        public List<Sprite> TopIcons => topList.ConvertAll(i => i.Icon);
        public List<Sprite> BottomIcons => bottomList.ConvertAll(i => i.Icon);
        public List<Sprite> ShoesIcons => shoesList.ConvertAll(i => i.Icon);
        public List<Color> SkinColors => skinColors;
        public List<Color> HairColors => hairColors;
        public List<Color> EyeColors => eyeColors;
        public List<Color> OutfitColors => outfitColors;

        public void UpdateHair(int hair) => UpdatePart(hair, hairList);
        public void UpdateHat(int hat) => UpdatePart(hat, hatList);
        public void UpdateTop(int top) => UpdatePart(top, topList);
        public void UpdateBottom(int bottom) => UpdatePart(bottom, bottomList);
        public void UpdateShoes(int shoes) => UpdatePart(shoes, shoesList);

        public void UpdateSkinColor(int color) => UpdateColor(skinMesh, color, skinColors);
        public void UpdateHairColor(int color) => UpdateColor(hairMesh, color, hairColors);
        public void UpdateEyeColor(int color) => UpdateColor(eyeMesh, color, eyeColors);
        public void UpdateOutfitColor(int color) => UpdateColor(outfitMesh, color, outfitColors);


        private void UpdateColor(List<RendererMaterial> meshes, int color, IReadOnlyList<Color> list)
        {
            if (color < 0) return;

            if (list.Count > color)
                foreach (var mesh in meshes)
                    mesh.SetMaterialColor(list[color]);
        }

        private void UpdatePart(int index, List<AvatarItem> list)
        {
            if (index < 0) return;
            for (var i = 0; i < list.Count; i++)
            {
                list[i].Activate(i == index);
            }
        }

        private void UpdatePart(int index, List<GameObject> list)
        {
            if (index < 0) return;

            for (var i = 0; i < list.Count; i++)
            {
                list[i].SetActive(i == index);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (TryGetComponent<Animator>(out var animator))
            {
                AnimatorAvatar = animator.avatar;
            }
        }
#endif
    }
}