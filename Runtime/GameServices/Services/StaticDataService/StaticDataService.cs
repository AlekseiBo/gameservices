using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolset;
using UnityEngine;

namespace GameServices
{
    class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider assets;
        private Dictionary<string, VenueStaticData> venues = new();
        private Dictionary<string, GameObject> avatars = new();

        public StaticDataService()
        {
            assets = Services.All.Single<IAssetProvider>();
        }

        public List<VenueStaticData> AllVenues() => venues.Values.ToList();
        public List<GameObject> AllAvatars() => avatars.Values.ToList();

        public VenueStaticData ForVenue(string sceneAddress) =>
            venues.TryGetValue(sceneAddress, out var staticData)
                ? staticData
                : null;

        public GameObject ForAvatar(string name) =>
            avatars.TryGetValue(name, out var gameObject)
                ? gameObject
                : null;

        public async Task LoadData()
        {
            await LoadVenues();
            await LoadAvatars();
        }

        public Dictionary<Tkey, ScriptableObject> AllGameData<Tkey>(string resourcePath) where Tkey : struct, Enum
        {
            var dataEntries = new Dictionary<Tkey, ScriptableObject>();
            var gameDataList = Resources.LoadAll<ScriptableObject>(resourcePath);

            foreach (var dataEntry in gameDataList)
                if (Enum.TryParse<Tkey>(dataEntry.name, out var dataKey))
                    dataEntries[dataKey] = dataEntry;

            return dataEntries;
        }

        private async Task LoadVenues()
        {
            var venueList = await assets.LoadLabel<VenueStaticData>("venueData", true);
            venues = venueList.ToDictionary(x => x.Address, x => x);
        }

        private async Task LoadAvatars()
        {
            var avatarList = await assets.LoadLabel<GameObject>("avatarData", true);
            avatars = avatarList.ToDictionary(x => x.name, x => x);
        }
    }
}