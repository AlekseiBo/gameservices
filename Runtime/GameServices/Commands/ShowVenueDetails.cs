using Toolset;
using UnityEngine;

namespace GameServices
{
    public class ShowVenueDetails : IMediatorCommand
    {
        public readonly VenueStaticData VenueData;
        public readonly int PlayerCounter;


        public ShowVenueDetails(VenueStaticData venueData, int playerCounter)
        {
            this.VenueData = venueData;
            this.PlayerCounter = playerCounter;
        }
    }
}