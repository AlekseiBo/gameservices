using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameServices
{
    class StaticDataService : IStaticDataService
    {
        private const string VenueDataPath = "StaticData/Venues";
        private const string DataEntryPath = "StaticData/GameData";

        private Dictionary<string, VenueStaticData> venues;

        public StaticDataService() => Load();

        public List<VenueStaticData> AllVenues() => venues.Values.ToList();

        public VenueStaticData ForVenue(string sceneAddress) =>
            venues.TryGetValue(sceneAddress, out VenueStaticData staticData)
                ? staticData
                : null;

        private void Load()
        {
            LoadVenues();
        }

        public Dictionary<T, ScriptableObject> AllGameData<T>() where T : struct, Enum
        {
            var dataEntries = new Dictionary<T, ScriptableObject>();
            var gameDataList = Resources.LoadAll<ScriptableObject>(DataEntryPath);

            foreach (var dataEntry in gameDataList)
                if (Enum.TryParse<T>(dataEntry.name, out var dataKey))
                    dataEntries[dataKey] = dataEntry;

            return dataEntries;
        }

        private void LoadVenues()
        {
            venues = Resources
                .LoadAll<VenueStaticData>(VenueDataPath)
                .ToDictionary(x => x.Name, x => x);
        }
    }
}