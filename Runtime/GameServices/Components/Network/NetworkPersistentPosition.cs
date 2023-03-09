using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    public abstract class NetworkPersistentPosition : NetworkBehaviour, IProgressWriter
    {
        [SerializeField] protected uint globalObjectIdHash;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            var reader = this as IProgressReader;
            LoadProgress(reader.RegisterProgress());
        }

        public override void OnDestroy()
        {
            if (!IsServer) return;
            var reader = this as IProgressReader;
            reader.UnregisterProgress();
        }

        public abstract void SaveProgress(ref ProgressData progress);

        public abstract void LoadProgress(ProgressData progress);

        protected void SavePositionList(PositionList progressList)
        {
            var venue = GameData.Get<string>(Key.CurrentVenue);
            progressList.UpdateOrAdd(transform.position, venue, globalObjectIdHash);
        }

        protected void LoadPositionList(PositionList progressList)
        {
            var venue = GameData.Get<string>(Key.CurrentVenue);
            var dataEntry = progressList.ForVenue(venue, globalObjectIdHash);
            if (dataEntry != null)
            {
                var col = GetComponent<Collider>();
                col.enabled = false;
                transform.position = dataEntry.Value;
                col.enabled = true;
            }
        }
    }
}