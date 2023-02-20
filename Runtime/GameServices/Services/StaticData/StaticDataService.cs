using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameServices
{
    class StaticDataService<TKey> : IStaticDataService<TKey> where TKey : struct, Enum
    {
        private const string VenueDataPath = "StaticData/Venues";
        private const string DataEntryPath = "StaticData/GameData";

        private Dictionary<string, VenueStaticData> venues;
        private Dictionary<TKey, ScriptableObject> dataEntries;

        public StaticDataService() => Load();

        public List<VenueStaticData> AllVenues() => venues.Values.ToList();
        public Dictionary<TKey, ScriptableObject> AllGameData() => dataEntries;

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
            dataEntries = new Dictionary<TKey, ScriptableObject>();

            var gameDataList = Resources
                .LoadAll<ScriptableObject>(DataEntryPath);

            foreach (var dataEntry in gameDataList)
                if (Enum.TryParse<TKey>(dataEntry.name, out var dataKey))
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