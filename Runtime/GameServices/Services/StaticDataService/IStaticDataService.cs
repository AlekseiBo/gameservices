using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface IStaticDataService : IService
    {
        VenueStaticData ForVenue(string sceneAddress);
        GameObject ForAvatar(string name);
        List<VenueStaticData> AllVenues();
        List<GameObject> AllAvatars();
        Dictionary<T, ScriptableObject> AllGameData<T>(string resourcePath) where T : struct, Enum;
    }
}