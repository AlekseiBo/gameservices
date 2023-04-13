using System.Threading.Tasks;
using Toolset;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameServices
{
    public class AvatarCustomizationCanvas : BaseCanvas
    {
        [SerializeField] private Transform previewTransform;
        [SerializeField] private Camera previewCamera;
        [SerializeField] private Transform customizeCategory;
        [SerializeField] private Transform customizePanel;

        private IAvatarProvider avatars;
        private AvatarController controller;

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as CustomizeAvatar;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
        }

        [ContextMenu("Show")]
        public override void ShowCanvas()
        {
            avatars = Services.All.Single<IAvatarProvider>();

            base.ShowCanvas();
            InstantiateAvatar();
            previewCamera.gameObject.SetActive(true);
            avatars.GetAvatarName(OnOwnerPrefabChanged);
            ActivatePanel(0);
        }

        [ContextMenu("Hide")]
        public override void HideCanvas()
        {
            base.HideCanvas();
            previewCamera.gameObject.SetActive(false);
            previewTransform.ClearChildren();

            avatars.RemoveSubscriber(Avatar.Prefab, OnOwnerPrefabChanged);

            UnsubscribeParts();
        }

        public void ActivatePanel(int index)
        {
            customizePanel.SetActiveChild(index);
        }

        private void InstantiateAvatar()
        {
            UnsubscribeParts();
            previewTransform.ClearChildren();

            Instantiate(avatars.GetAvatar(), previewTransform).transform
                .With(a => a.localPosition = Vector3.zero)
                .With(a => a.localRotation = Quaternion.identity)
                .With(a => controller = a.GetComponent<AvatarController>());

            SubscribeParts();
        }

        private void SubscribeParts()
        {
            controller.UpdateHair(avatars.GetPart(Avatar.Hair, OnHairChanged));
            controller.UpdateTop(avatars.GetPart(Avatar.Top, OnTopChanged));
            controller.UpdateBottom(avatars.GetPart(Avatar.Bottom, OnBottomChanged));
            controller.UpdateShoes(avatars.GetPart(Avatar.Shoes, OnShoesChanged));

            controller.UpdateSkinColor(avatars.GetPart(Avatar.SkinColor, OnSkinColorChanged));
            controller.UpdateHairColor(avatars.GetPart(Avatar.HairColor, OnHairColorChanged));
            controller.UpdateEyeColor(avatars.GetPart(Avatar.EyeColor, OnEyeColorChanged));
            controller.UpdateOutfitColor(avatars.GetPart(Avatar.OutfitColor, OnOutfitColorChanged));
        }

        private void UnsubscribeParts()
        {
            avatars.RemoveSubscriber(Avatar.Hair, OnHairChanged);
            avatars.RemoveSubscriber(Avatar.Top, OnTopChanged);
            avatars.RemoveSubscriber(Avatar.Bottom, OnBottomChanged);
            avatars.RemoveSubscriber(Avatar.Shoes, OnShoesChanged);

            avatars.RemoveSubscriber(Avatar.SkinColor, OnSkinColorChanged);
            avatars.RemoveSubscriber(Avatar.HairColor, OnHairColorChanged);
            avatars.RemoveSubscriber(Avatar.EyeColor, OnEyeColorChanged);
            avatars.RemoveSubscriber(Avatar.OutfitColor, OnOutfitColorChanged);
        }


        private void OnOwnerPrefabChanged(DataEntry<string> prefab) => InstantiateAvatar();

        private void OnHairChanged(DataEntry<int> current) => controller.UpdateHair(current.Value);
        private void OnTopChanged(DataEntry<int> current) => controller.UpdateTop(current.Value);
        private void OnBottomChanged(DataEntry<int> current) => controller.UpdateBottom(current.Value);
        private void OnShoesChanged(DataEntry<int> current) => controller.UpdateShoes(current.Value);

        private void OnSkinColorChanged(DataEntry<int> current) => controller.UpdateSkinColor(current.Value);
        private void OnHairColorChanged(DataEntry<int> current) => controller.UpdateHairColor(current.Value);
        private void OnEyeColorChanged(DataEntry<int> current) => controller.UpdateEyeColor(current.Value);
        private void OnOutfitColorChanged(DataEntry<int> current) => controller.UpdateOutfitColor(current.Value);
    }
}