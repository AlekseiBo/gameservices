using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface IStaticDataService : IService
    {
        VenueStaticData ForVenue(string sceneAddress);
        List<VenueStaticData> AllVenues();
        Dictionary<T, ScriptableObject> AllGameData<T>() where T : struct, Enum;
    }
}