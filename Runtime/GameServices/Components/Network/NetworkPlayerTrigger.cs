using System.Threading.Tasks;
using Toolset;
using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    [RequireComponent(typeof(Collider))]
    public class NetworkPlayerTrigger : MonoBehaviour
    {
        private const int ACTIVATION_TIMEOUT = 2000;

        [SerializeField] private ScriptableObject data;
        [SerializeField] private bool localOnly;
        [SerializeField] private bool oneTime;

        private bool activated;

        private void OnTriggerEnter(Collider other)
        {
            if (activated) return;
            if (!other.TryGetComponent<NetworkObject>(out var netObject)) return;
            if ((!localOnly || !netObject.IsLocalPlayer) && (localOnly || !netObject.IsPlayerObject)) return;

            activated = true;
            if (!oneTime) Reactivate();

            Command.Publish(new ActivatePlayerTrigger(netObject, data));
        }

        private async void Reactivate()
        {
            await Task.Delay(ACTIVATION_TIMEOUT);
            activated = false;
        }
    }
}