using System.Collections.Generic;
using Toolset;
using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    public class NetworkTracker : NetworkBehaviour, IProgressWriter
    {
        [SerializeField] private GameObject coinPrefab;

        private static List<GameObject> coinObjects = new List<GameObject>();

        private PositionList coinPosition = new PositionList();

        private void Awake()
        {


            if (!AsServer) return;
            var reader = this as IProgressReader;
            LoadProgress(reader.RegisterProgress());
        }

        public override void OnDestroy()
        {


            if (!AsServer) return;
            var reader = this as IProgressReader;
            reader.UnregisterProgress();
        }

        public override void OnNetworkSpawn()
        {
            if (!AsServer) return;

            coinObjects = new List<GameObject>();

            foreach (var coin in coinPosition.List)
            {
                if (GameData.Get<string>(Key.CurrentVenue) != coin.Venue) continue;

                var coinObject = Instantiate(coinPrefab, coin.Value, Quaternion.identity);
                coinObjects.Add(coinObject);
                coinObject.GetComponent<NetworkObject>().Spawn(true);
            }
        }

        public static void SaveCoin(GameObject coin) => coinObjects.Add(coin);

        public void SaveProgress(ref ProgressData progress)
        {
            progress.CoinPosition = new PositionList();
            var venue = GameData.Get<string>(Key.CurrentVenue);

            for (var i = 0; i < coinObjects.Count; i++)
            {
                if (coinObjects[i] != null)
                {
                    var position = coinObjects[i].transform.position;
                    progress.CoinPosition.UpdateOrAdd(position, venue, (uint)i);
                }
                else
                {
                    coinObjects.RemoveAt(i);
                }
            }
        }

        public void LoadProgress(ProgressData progress)
        {
            coinPosition = progress.CoinPosition;
        }

        [ClientRpc]
        public void ChangeVenueClientRpc(string address)
        {
            if (NetworkManager.Singleton.IsHost) return;
            ClientTriggerActivated(address);
        }

        private void ClientTriggerActivated(string venueAddress)
        {
            var dialog = new ShowDialog(
                "Teleport",
                $"Do you want to follow the host?",
                "YES",
                () => Command.Publish(new UpdateVenue(VenueAction.Change, venueAddress)),
                "NO",
                () =>
                {
                    GameData.Set(Key.PlayerNetState, NetState.Client);
                    var address = GameData.Get<string>(Key.CurrentVenue);
                    Command.Publish(new UpdateVenue(VenueAction.Exit, address));
                });

            Command.Publish(dialog);
        }

        private bool AsServer => (GameData.Get<NetState>(Key.PlayerNetState) == NetState.Dedicated ||
                                  GameData.Get<NetState>(Key.PlayerNetState) == NetState.Host);
    }
}