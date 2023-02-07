using System.Collections.Generic;
using Framework;

namespace GameServices.StaticData
{
    public interface IStaticDataService : IService
    {
        VenueStaticData ForVenue(string sceneAddress);
        List<VenueStaticData> AllVenues();
    }
}