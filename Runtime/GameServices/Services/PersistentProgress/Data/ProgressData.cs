using System;

namespace GameServices
{
    [Serializable]
    public class ProgressData
    {
        public PositionList PlayerPosition;
        public PositionList CratePosition;
        public PositionList CoinPosition;
        public FriendList FriendList;

        public ProgressData()
        {
            PlayerPosition = new PositionList();
            CratePosition = new PositionList();
            CoinPosition = new PositionList();
            FriendList = new FriendList();
        }
    }
}