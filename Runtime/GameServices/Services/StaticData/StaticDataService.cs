using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.StaticData
{
    class StaticDataService : IStaticDataService
    {
        private const string VenueDataPath = "StaticData/Venues";

        private Dictionary<string, VenueStaticData> venues;

        public StaticDataService() => Load();

        public List<VenueStaticData> AllVenues() => venues.Values.ToList();

        public VenueStaticData ForVenue(string sceneAddress) =>
            venues.TryGetValue(sceneAddress, out VenueStaticData staticData)
                ? staticData
                : null;

        private void Load()
        {
            venues = Resources
                .LoadAll<VenueStaticData>(VenueDataPath)
                .ToDictionary(x => x.Name, x => x);
        }


    }
}