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
    }
}