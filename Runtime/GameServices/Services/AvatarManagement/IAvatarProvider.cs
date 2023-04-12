using System;
using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface IAvatarProvider : IService
    {
        GameObject GetAvatar(string name = "");
        string GetAvatarName(Action<DataEntry<string>> callback);
        int GetPart(Avatar part);
        int GetPart(Avatar part, Action<DataEntry<int>> callback);
        void RemoveSubscriber(Avatar part, Action<DataEntry<string>> callback);
        void RemoveSubscriber(Avatar part, Action<DataEntry<int>> callback);
    }
}