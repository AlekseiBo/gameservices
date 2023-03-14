using System;
using System.Collections.Generic;

namespace GameServices
{
    [Serializable]
    public class FriendList
    {
        public List<FriendData> List;

        public FriendList()
        {
            List = new List<FriendData>();
        }

        public void UpdateOrAdd(string name, string id)
        {
            var dataEntry = List.Find(data => data.Id == id);
            if (dataEntry != null)
            {
                dataEntry.Id = id;
                dataEntry.Name = name;
            }
            else
                List.Add(new FriendData { Name = name, Id = id });
        }

        public void Remove(string id)
        {
            var dataEntry = List.Find(data => data.Id == id);
            if (dataEntry != null) List.Remove(dataEntry);
        }
    }
}