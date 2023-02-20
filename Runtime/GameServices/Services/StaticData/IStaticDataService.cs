using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface IStaticDataService<TKey> : IService
    {
        VenueStaticData ForVenue(string sceneAddress);
        List<VenueStaticData> AllVenues();
        Dictionary<TKey, ScriptableObject> AllGameData();
    }
}