using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public class VenueSelectionList : MonoBehaviour
    {
        [SerializeField] private GameObject venueItemPrefab;
        [SerializeField] private Transform content;

        private IStaticDataService staticData;
        private readonly List<VenueItem> venueList = new();

        [ContextMenu("Build List")]
        public async void BuildList()
        {
            content.ClearChildren();
            venueList.Clear();
            staticData = Services.All.Single<IStaticDataService>();

            foreach (var venue in staticData.AllVenues())
            {
                Instantiate(venueItemPrefab, content).GetComponent<VenueItem>()
                    .With(i => i.Construct(venue))
                    .With(i => venueList.Add(i));
            }

            var playerList = await Services.All.Single<ILobbyProvider>().QueryPlayersOnline();

            foreach (var venue in venueList)
            {
                if (playerList.TryGetValue(venue.Address, out var players))
                {
                    venue.UpdatePlayerCounter(players);
                }
            }
        }
    }
}