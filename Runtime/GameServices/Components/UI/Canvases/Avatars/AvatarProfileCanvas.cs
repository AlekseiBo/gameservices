using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public class AvatarProfileCanvas : BaseCanvas
    {
        [Serializable]
        private struct NewProfileButton
        {
            public AvatarGroup Group;
            public int AvatarCounter;
            public GameObject Prefab;
        }

        [SerializeField] private int totalPrefabs;
        [SerializeField] private GameObject profileButtonPrefab;
        [SerializeField] private Transform gridTransform;
        [SerializeField] private Transform previewTransform;
        [SerializeField] private Camera previewCamera;
        [SerializeField] private NewProfileButton[] newProfileButton;

        private IAvatarProvider avatars;
        private AvatarGroup group;
        private readonly List<Texture2D> iconsList = new();

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as SelectAvatarProfile;
            group = data.Group;
        }

        [ContextMenu("Show")]
        public override void ShowCanvas()
        {
            avatars = Services.All.Single<IAvatarProvider>();
            FillProfileGrid();
            base.ShowCanvas();
        }

        [ContextMenu("Hide")]
        public override void HideCanvas()
        {
            ClearIconsList();
            base.HideCanvas();
        }

        private void FillProfileGrid()
        {
            ClearIconsList();
            gridTransform.ClearChildren();
            previewTransform.ClearChildren();
            previewCamera.gameObject.SetActive(true);

            var groupCounters = new Dictionary<AvatarGroup, int>();

            foreach (var avatarData in avatars.LoadAvatarData())
            {
                if (avatarData.Group != group && group != AvatarGroup.Any) continue;

                var preview = Instantiate(avatars.GetAvatar(avatarData.Prefab), previewTransform);
                var controller = preview.GetComponent<AvatarController>();
                UpdateController(controller, avatarData);
                preview.transform.localPosition = controller.Preview;
                preview.transform.localRotation = Quaternion.identity;

                var avatarIcon = GetAvatarIcon();
                iconsList.Add(avatarIcon);
                DestroyImmediate(preview);
                Instantiate(profileButtonPrefab, gridTransform).GetComponent<AvatarProfileButton>()
                    .With(b => b.Construct(avatarData, avatarIcon, group));

                groupCounters.Increment(avatarData.Group, 1);
            }

            foreach (var button in newProfileButton)
            {
                if (button.Group != group && group != AvatarGroup.Any) continue;

                groupCounters.TryGetValue(button.Group, out var counter);
                if (counter < button.AvatarCounter)
                {
                    var avatarData = new AvatarPersistentData { Prefab = avatars.GetAvatar().name };
                    Instantiate(button.Prefab, gridTransform).GetComponent<AvatarProfileButton>()
                        .With(b => b.transform.SetAsFirstSibling())
                        .With(b => b.Construct(avatarData));
                }
            }

            previewCamera.gameObject.SetActive(false);
        }

        private Texture2D GetAvatarIcon()
        {
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = previewCamera.targetTexture;

            previewCamera.Render();
            var texture = previewCamera.targetTexture;
            var avatarIcon = new Texture2D(texture.width, texture.height);
            avatarIcon.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            avatarIcon.Apply();
            RenderTexture.active = activeRenderTexture;
            return avatarIcon;
        }

        private void ClearIconsList()
        {
            foreach (var icon in iconsList)
                Destroy(icon);

            iconsList.Clear();
        }

        private void UpdateController(AvatarController controller, AvatarPersistentData avatar)
        {
            controller.UpdateHair(avatar.Hair);
            controller.UpdateTop(avatar.Top);
            controller.UpdateBottom(avatar.Bottom);
            controller.UpdateShoes(avatar.Shoes);

            controller.UpdateSkinColor(avatar.SkinColor);
            controller.UpdateHairColor(avatar.HairColor);
            controller.UpdateEyeColor(avatar.EyeColor);
            controller.UpdateOutfitColor(avatar.OutfitColor);
        }
    }
}