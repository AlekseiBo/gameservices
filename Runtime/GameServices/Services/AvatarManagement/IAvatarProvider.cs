using System;
using System.Collections.Generic;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface IAvatarProvider : IService
    {
        GameObject GetAvatar(string name = "");
        void SaveAvatarData();
        List<AvatarPersistentData> LoadAvatarData();
        string GetAvatarName(Action<DataEntry<string>> callback);
        void SetGroup(AvatarGroup group);
        void SetPart(Avatar part, int value);
        int GetPart(Avatar part);
        int GetPart(Avatar part, Action<DataEntry<int>> callback);
        void RemoveSubscriber(Avatar part, Action<DataEntry<string>> callback);
        void RemoveSubscriber(Avatar part, Action<DataEntry<int>> callback);
    }
}