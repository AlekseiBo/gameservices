using System;
using System.Collections.Generic;

namespace GameServices
{
    [Serializable]
    public class AvatarList
    {
        public List<AvatarPersistentData> List;

        public AvatarList()
        {
            List = new List<AvatarPersistentData>();
        }

        public void UpdateOrAdd(AvatarPersistentData avatarData)
        {
            var dataIndex = List.FindIndex(data => data.Prefab == avatarData.Prefab);
            if (dataIndex >= 0)
                List[dataIndex] = avatarData;
            else
                List.Add(avatarData);
        }

        public void Remove(string prefab)
        {
            var dataEntry = List.Find(data => data.Prefab == prefab);
            if (dataEntry != null) List.Remove(dataEntry);
        }
    }
}