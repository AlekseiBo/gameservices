using System.Collections.Generic;
using System.Linq;
using GameServices.GameDataService;
using UnityEngine;

namespace GameServices.StaticData
{
    class StaticDataService : IStaticDataService
    {
        private const string VenueDataPath = "StaticData/Venues";
        private const string DataEntryPath = "StaticData/GameData";

        private Dictionary<string, VenueStaticData> venues;
        private Dictionary<Key, ScriptableObject> dataEntries;

        public StaticDataService() => Load();

        public List<VenueStaticData> AllVenues() => venues.Values.ToList();
        public Dictionary<Key, ScriptableObject> AllGameData() => dataEntries;

        public VenueStaticData ForVenue(string sceneAddress) =>
            venues.TryGetValue(sceneAddress, out VenueStaticData staticData)
                ? staticData
                : null;

        private void Load()
        {
            LoadVenues();
            LoadGameData();
        }

        private void LoadGameData()
        {
            dataEntries = new Dictionary<Key, ScriptableObject>();

            var gameDataList = Resources
                .LoadAll<ScriptableObject>(DataEntryPath);

            foreach (var dataEntry in gameDataList)
                if (Key.TryParse<Key>(dataEntry.name, out var dataKey))
                    dataEntries[dataKey] = dataEntry;
        }

        private void LoadVenues()
        {
            venues = Resources
                .LoadAll<VenueStaticData>(VenueDataPath)
                .ToDictionary(x => x.Name, x => x);
        }
    }
}