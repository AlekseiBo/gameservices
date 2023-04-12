using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    public class AvatarController : MonoBehaviour
    {
        public UnityEngine.Avatar AnimatorAvatar;
        [Space]
        [SerializeField] private List<GameObject> hairList;
        [SerializeField] private List<GameObject> topList;
        [SerializeField] private List<GameObject> bottomList;
        [SerializeField] private List<GameObject> shoesList;
        [Space]
        [SerializeField] private List<Renderer> skinMesh;
        [SerializeField] private List<Color> skinColors;
        [Space]
        [SerializeField] private List<Renderer> hairMesh;
        [SerializeField] private List<Color> hairColors;
        [Space]
        [SerializeField] private List<Renderer> eyeMesh;
        [SerializeField] private List<Color> eyeColors;
        [Space]
        [SerializeField] private List<Renderer> outfitMesh;
        [SerializeField] private List<Color> outfitColors;

        public void UpdateHair(int hair) => UpdatePart(hair, hairList);
        public void UpdateTop(int top) => UpdatePart(top, topList);
        public void UpdateBottom(int bottom) => UpdatePart(bottom, bottomList);
        public void UpdateShoes(int shoes) => UpdatePart(shoes, shoesList);

        public void UpdateSkinColor(int color) => UpdateColor(skinMesh, color, skinColors);
        public void UpdateHairColor(int color) => UpdateColor(hairMesh, color, hairColors);
        public void UpdateEyeColor(int color) => UpdateColor(eyeMesh, color, eyeColors);
        public void UpdateOutfitColor(int color) => UpdateColor(outfitMesh, color, outfitColors);


        private void UpdateColor(List<Renderer> meshes, int color, IReadOnlyList<Color> list)
        {
            if (list.Count > color)
                foreach (var mesh in meshes)
                    mesh.material.color = list[color];
        }

        private void UpdatePart(int index, List<GameObject> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i].SetActive(i == index - 1);
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