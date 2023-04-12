using System;
using Toolset;
using UnityEngine;

namespace GameServices
{
    class AvatarProvider : IAvatarProvider
    {
        private readonly IStaticDataService staticData;

        public AvatarProvider()
        {
            staticData = Services.All.Single<IStaticDataService>();
        }

        public GameObject GetAvatar(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = AvatarData.Get<string>(Avatar.Prefab);

            return staticData.ForAvatar(name);
        }

        public string GetAvatarName(Action<DataEntry<string>> callback)
        {
            return AvatarData.Subscribe(Avatar.Prefab, callback);
        }

        public int GetPart(Avatar part)
        {
            return AvatarData.Get<int>(part);
        }

        public int GetPart(Avatar part, Action<DataEntry<int>> callback)
        {
            return AvatarData.Subscribe(part, callback);
        }

        public void RemoveSubscriber(Avatar part, Action<DataEntry<string>> callback)
        {
            AvatarData.RemoveSubscriber(part, callback);
        }

        public void RemoveSubscriber(Avatar part, Action<DataEntry<int>> callback)
        {
            AvatarData.RemoveSubscriber(part, callback);
        }
    }
}