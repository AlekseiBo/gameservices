using Toolset;
using UnityEngine;

namespace GameServices
{
    public interface IAvatarProvider : IService
    {
        GameObject GetAvatar(string name = "");
    }
}