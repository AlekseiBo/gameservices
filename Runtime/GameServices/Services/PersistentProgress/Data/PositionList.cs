using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    [Serializable]
    public class PositionList
    {
        public List<PersistentVenueData<Vector3>> List;

        public PositionList()
        {
            List = new List<PersistentVenueData<Vector3>>();
        }

        public void UpdateOrAdd(Vector3 position, string venue, uint id = 0)
        {
            var dataEntry = ForVenue(venue, id);
            if (dataEntry != null)
                dataEntry.Value = position;
            else
                List.Add(new PersistentVenueData<Vector3> { Id = id, Venue = venue, Value = position });
        }

        public PersistentVenueData<Vector3> ForVenue(string venue) => List.Find(data => data.Venue == venue);
        public PersistentVenueData<Vector3> ForVenue(string venue, uint id) => List.Find(data => data.Id == id && data.Venue == venue);
    }
}