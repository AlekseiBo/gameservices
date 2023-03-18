using Toolset;
using Unity.Netcode;
using UnityEngine;

namespace GameServices
{
    public class ActivatePlayerTrigger : IMediatorCommand
    {
        public readonly NetworkObject Player;
        public readonly ScriptableObject Data;

        public ActivatePlayerTrigger(NetworkObject playerObject, ScriptableObject triggerData)
        {
            Player = playerObject;
            Data = triggerData;
        }
    }
}