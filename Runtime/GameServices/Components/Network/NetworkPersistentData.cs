using GameServices.CodeBlocks;
using Toolset;
using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    public class NetworkPersistentData : NetworkBehaviour, IProgressWriter
    {
        [SerializeField] private uint globalObjectIdHash;

        public override void OnNetworkSpawn() => RegisterProgress();

        public override void OnDestroy() => UnregisterProgress();

        public void SaveProgress(ref ProgressData progress)
        {
            var venue = GameData.Get<string>(Key.CurrentVenue);
            progress.CratePosition.UpdateOrAdd(transform.position, venue, globalObjectIdHash);
        }

        public void LoadProgress(ProgressData progress)
        {
            var venue = GameData.Get<string>(Key.CurrentVenue);
            var dataEntry = progress.CratePosition.ForVenue(venue, globalObjectIdHash);
            if (dataEntry != null)
            {
                var col = GetComponent<Collider>();
                col.enabled = false;
                transform.position = dataEntry.Value;
                col.enabled = true;
            }
        }

        private void RegisterProgress()
        {
            if (!IsServer) return;
            var progress = Services.All.Single<IProgressProvider>().Register(this);
            LoadProgress(progress);
        }

        private void UnregisterProgress()
        {
            if (!IsServer) return;
            Services.All.Single<IProgressProvider>().Unregister(this);
        }
    }
}