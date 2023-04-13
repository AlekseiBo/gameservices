using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    public class NetworkPlayerAvatar : NetworkBehaviour
    {
        private readonly NetworkVariable<FixedString32Bytes> playerPrefab = new("",
           NetworkVariableReadPermission.Everyone,
          NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> playerHair = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> playerTop = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> playerBottom = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> playerShoes = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> skinColor = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> hairColor = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> eyeColor = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<int> outfitColor = new (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private IAvatarProvider avatars;
        private AvatarController controller;
        private readonly List<GameObject> avatarParts = new();

        private void Awake() => avatars = Services.All.Single<IAvatarProvider>();

        public override void OnNetworkSpawn()
        {
            playerPrefab.OnValueChanged += OnPrefabChanged;

            playerHair.OnValueChanged += OnHairChanged;
            playerTop.OnValueChanged += OnTopChanged;
            playerBottom.OnValueChanged += OnBottomChanged;
            playerShoes.OnValueChanged += OnShoesChanged;

            skinColor.OnValueChanged += OnSkinColorChanged;
            hairColor.OnValueChanged += OnHairColorChanged;
            eyeColor.OnValueChanged += OnEyeColorChanged;
            outfitColor.OnValueChanged += OnOutfitColorChanged;

            if (IsOwner)
            {
                playerPrefab.Value = avatars.GetAvatarName(OnOwnerPrefabChanged);

                playerHair.Value = avatars.GetPart(Avatar.Hair, OnOwnerHairChanged);
                playerTop.Value = avatars.GetPart(Avatar.Top, OnOwnerTopChanged);
                playerBottom.Value = avatars.GetPart(Avatar.Bottom, OnOwnerBottomChanged);
                playerShoes.Value = avatars.GetPart(Avatar.Shoes, OnOwnerShoesChanged);

                skinColor.Value = avatars.GetPart(Avatar.SkinColor, OnOwnerSkinColorChanged);
                hairColor.Value = avatars.GetPart(Avatar.HairColor, OnOwnerHairColorChanged);
                eyeColor.Value = avatars.GetPart(Avatar.EyeColor, OnOwnerEyeColorChanged);
                outfitColor.Value = avatars.GetPart(Avatar.OutfitColor, OnOwnerOutfitColorChanged);
            }
            else
            {
                InstantiateAvatar(playerPrefab.Value.ToString());
            }
        }

        public override void OnNetworkDespawn()
        {
            avatars.RemoveSubscriber(Avatar.Prefab, OnOwnerPrefabChanged);

            avatars.RemoveSubscriber(Avatar.Hair, OnOwnerHairChanged);
            avatars.RemoveSubscriber(Avatar.Top, OnOwnerTopChanged);
            avatars.RemoveSubscriber(Avatar.Bottom, OnOwnerBottomChanged);
            avatars.RemoveSubscriber(Avatar.Shoes, OnOwnerShoesChanged);

            avatars.RemoveSubscriber(Avatar.SkinColor, OnOwnerSkinColorChanged);
            avatars.RemoveSubscriber(Avatar.HairColor, OnOwnerHairColorChanged);
            avatars.RemoveSubscriber(Avatar.EyeColor, OnOwnerEyeColorChanged);
            avatars.RemoveSubscriber(Avatar.OutfitColor, OnOwnerOutfitColorChanged);

            playerPrefab.OnValueChanged -= OnPrefabChanged;

            playerHair.OnValueChanged -= OnHairChanged;
            playerTop.OnValueChanged -= OnTopChanged;
            playerBottom.OnValueChanged -= OnBottomChanged;
            playerShoes.OnValueChanged -= OnShoesChanged;

            skinColor.OnValueChanged -= OnSkinColorChanged;
            hairColor.OnValueChanged -= OnHairColorChanged;
            eyeColor.OnValueChanged -= OnEyeColorChanged;
            outfitColor.OnValueChanged -= OnOutfitColorChanged;

        }

        private void OnOwnerPrefabChanged(DataEntry<string> prefab) => playerPrefab.Value = prefab.Value;

        private void OnOwnerHairChanged(DataEntry<int> part) => playerHair.Value = part.Value;
        private void OnOwnerTopChanged(DataEntry<int> part) => playerTop.Value = part.Value;
        private void OnOwnerBottomChanged(DataEntry<int> part) => playerBottom.Value = part.Value;
        private void OnOwnerShoesChanged(DataEntry<int> part) => playerShoes.Value = part.Value;

        private void OnOwnerSkinColorChanged(DataEntry<int> part) => skinColor.Value = part.Value;
        private void OnOwnerHairColorChanged(DataEntry<int> part) => hairColor.Value = part.Value;
        private void OnOwnerEyeColorChanged(DataEntry<int> part) => eyeColor.Value = part.Value;
        private void OnOwnerOutfitColorChanged(DataEntry<int> part) => outfitColor.Value = part.Value;


        private void OnHairChanged(int previous, int current) => controller.UpdateHair(current);
        private void OnTopChanged(int previous, int current) => controller.UpdateTop(current);
        private void OnBottomChanged(int previous, int current) => controller.UpdateBottom(current);
        private void OnShoesChanged(int previous, int current) => controller.UpdateShoes(current);

        private void OnSkinColorChanged(int previous, int current) => controller.UpdateSkinColor(current);
        private void OnHairColorChanged(int previous, int current) => controller.UpdateHairColor(current);
        private void OnEyeColorChanged(int previous, int current) => controller.UpdateEyeColor(current);
        private void OnOutfitColorChanged(int previous, int current) => controller.UpdateOutfitColor(current);


        private void OnPrefabChanged(FixedString32Bytes previous, FixedString32Bytes current) =>
            InstantiateAvatar(playerPrefab.Value.ToString());



        private async void InstantiateAvatar(string prefabName)
        {
            DeleteAvatarParts();

            var avatarTransform = Instantiate(avatars.GetAvatar(prefabName), transform).transform;
            var animator = GetComponent<Animator>();
            controller = avatarTransform.GetComponent<AvatarController>();
            animator.avatar = controller.AnimatorAvatar;

            for (var i = avatarTransform.childCount - 1; i >= 0; i--)
            {
                var avatarPart = avatarTransform.GetChild(i);
                avatarPart.SetParent(transform);
                avatarPart.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                avatarParts.Add(avatarPart.gameObject);
            }

            if (controller.TryGetComponent<Animator>(out var animatorComponent))
                Destroy(animatorComponent);

            await Task.Delay(1);
            animator.Rebind();
        }

        private void DeleteAvatarParts()
        {
            for (var i = avatarParts.Count - 1; i >= 0; i--) Destroy(avatarParts[i]);
            avatarParts.Clear();
        }
    }
}