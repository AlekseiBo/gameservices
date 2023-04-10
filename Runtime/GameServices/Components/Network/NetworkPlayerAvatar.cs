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

        private IAvatarProvider avatars;

        private void Awake()
        {
            avatars = Services.All.Single<IAvatarProvider>();
        }

        public override void OnDestroy()
        {
            playerPrefab.OnValueChanged -= OnPrefabChanged;
            base.OnDestroy();
        }

        public override void OnNetworkSpawn()
        {
            playerPrefab.OnValueChanged += OnPrefabChanged;
            if (!IsOwner) return;
            InstantiateAvatar("");
        }

        private void OnPrefabChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue) =>
            InstantiateAvatar(newValue.Value);


        private void InstantiateAvatar(string prefabName)
        {
            var prefab = avatars.GetAvatar(prefabName);

            if (!string.IsNullOrEmpty(prefabName))
                playerPrefab.Value = prefabName;

            var avatarTransform = Instantiate(prefab, transform).transform;

            var animator = GetComponent<Animator>();
            animator.avatar = avatarTransform.GetComponent<Animator>().avatar;

            for (var i = avatarTransform.childCount - 1; i >= 0; i--)
            {
                var avatarPart = avatarTransform.GetChild(i);
                avatarPart.SetParent(transform);
                avatarPart.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            Destroy(avatarTransform.gameObject);
        }
    }
}