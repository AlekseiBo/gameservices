using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    public abstract class NetworkPersistentPosition : NetworkBehaviour, IProgressWriter
    {
        [SerializeField] protected uint globalObjectIdHash;

        private bool registered;

        private void Awake()
        {
            var reader = this as IProgressReader;
            LoadProgress(reader.RegisterProgress());
            registered = true;
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer || registered) return;
            var reader = this as IProgressReader;
            LoadProgress(reader.RegisterProgress());
        }

        public override void OnDestroy()
        {
            var reader = this as IProgressReader;
            reader.UnregisterProgress();
            registered = false;
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
                var colliders = GetComponents<Collider>();
                foreach (var col in colliders) col.enabled = false;
                transform.position = dataEntry.Value;
                foreach (var col in colliders) col.enabled = true;
            }
        }
    }
}