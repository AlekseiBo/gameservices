using System.Collections.Generic;
using Framework;
using GameServices.GameDataService;
using UnityEngine;

namespace GameServices.StaticData
{
    public interface IStaticDataService : IService
    {
        VenueStaticData ForVenue(string sceneAddress);
        List<VenueStaticData> AllVenues();
        Dictionary<Key, ScriptableObject> AllGameData();
    }
}