using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    class AvatarProvider : IAvatarProvider
    {
        private readonly IStaticDataService staticData;
        private readonly IProgressProvider progress;

        public AvatarProvider()
        {
            staticData = Services.All.Single<IStaticDataService>();
            progress = Services.All.Single<IProgressProvider>();
        }

        public GameObject GetAvatar(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = AvatarData.Get<string>(Avatar.Prefab);

            return staticData.ForAvatar(name);
        }

        public void SetAvatar(string prefabName)
        {
            AvatarData.Set(Avatar.Prefab, prefabName);
        }

        public void SaveAvatarData()
        {
            var avatarData = new AvatarPersistentData
            {
                Prefab = AvatarData.Get<string>(Avatar.Prefab),
                Group = AvatarData.Get<AvatarGroup>(Avatar.Group),
                Hair = AvatarData.Get<int>(Avatar.Hair),
                Hat = AvatarData.Get<int>(Avatar.Hat),
                Top = AvatarData.Get<int>(Avatar.Top),
                Bottom = AvatarData.Get<int>(Avatar.Bottom),
                Shoes = AvatarData.Get<int>(Avatar.Shoes),
                SkinColor = AvatarData.Get<int>(Avatar.SkinColor),
                HairColor = AvatarData.Get<int>(Avatar.HairColor),
                EyeColor = AvatarData.Get<int>(Avatar.EyeColor),
                OutfitColor = AvatarData.Get<int>(Avatar.OutfitColor),
            };

            progress.ProgressData.AvatarList.UpdateOrAdd(avatarData);
            progress.SaveProgress();
        }

        public List<AvatarPersistentData> LoadAvatarData()
        {
            return progress.ProgressData.AvatarList.List;
        }

        public string GetAvatarName(Action<DataEntry<string>> callback)
        {
            return AvatarData.Subscribe(Avatar.Prefab, callback);
        }

        public void SetGroup(AvatarGroup group)
        {
            AvatarData.Set(Avatar.Group, group);
        }

        public void SetPart(Avatar part, int value)
        {
            AvatarData.Set(part, value);
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