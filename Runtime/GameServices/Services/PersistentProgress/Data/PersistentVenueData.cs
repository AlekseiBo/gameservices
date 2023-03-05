using System;

namespace GameServices
{
    [Serializable]
    public class PersistentVenueData<T>
    {
        public uint Id;
        public string Venue;
        public T Value;
    }
}