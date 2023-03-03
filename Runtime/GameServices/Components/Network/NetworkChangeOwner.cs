using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    [RequireComponent(typeof(Rigidbody))]
    public class NetworkChangeOwner : NetworkBehaviour
    {
        [SerializeField] private bool OnCollision;
        [SerializeField] private bool OnTrigger;

        private NetworkObject netObject;

        private void Awake()
        {
            netObject = GetComponent<NetworkObject>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!OnCollision || !IsOwner) return;
            CheckCollision(with: collision.transform);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!OnTrigger || !IsOwner) return;
            CheckCollision(with: collider.transform);
        }

        private void CheckCollision(Transform with)
        {
            var other = with.transform.GetComponent<NetworkObject>();
            if (other != null && other.OwnerClientId != OwnerClientId)
            {
                ChangeOwnerServerRpc(other.OwnerClientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeOwnerServerRpc(ulong newClientId)
        {
            if (NetworkManager.ConnectedClients.ContainsKey(newClientId))
            {
                netObject.ChangeOwnership(newClientId);
            }
        }
    }
}