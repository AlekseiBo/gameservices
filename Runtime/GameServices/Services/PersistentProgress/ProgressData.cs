using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    [Serializable]
    public class ProgressData
    {
        public List<PlayerPosition> PlayerVenuePositions;

        public ProgressData()
        {
            PlayerVenuePositions = new List<PlayerPosition>();
        }
    }

    [Serializable]
    public class PlayerPosition
    {
        public string Venue;
        public Vector3 Position;
    }
}