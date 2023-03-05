using System;

namespace GameServices
{
    [Serializable]
    public class ProgressData
    {
        public PositionList PlayerPosition;
        public PositionList CratePosition;

        public ProgressData()
        {
            PlayerPosition = new PositionList();
            CratePosition = new PositionList();
        }
    }
}