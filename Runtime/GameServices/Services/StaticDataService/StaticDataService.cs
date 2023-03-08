using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameServices
{
    class StaticDataService : IStaticDataService
    {
        private Dictionary<string, VenueStaticData> venues;
        private IStaticDataService staticDataServiceImplementation;

        public StaticDataService() => Load();

        public List<VenueStaticData> AllVenues() => venues.Values.ToList();

        public VenueStaticData ForVenue(string sceneAddress) =>
            venues.TryGetValue(sceneAddress, out var staticData)
                ? staticData
                : null;

        private void Load()
        {
            LoadVenues();
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

        private void LoadVenues()
        {
            venues = Resources
                .LoadAll<VenueStaticData>("StaticData/Venues")
                .ToDictionary(x => x.Address, x => x);
        }
    }
}