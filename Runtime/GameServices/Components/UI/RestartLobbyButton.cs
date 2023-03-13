using Toolset;
using UnityEngine;

namespace GameServices
{
    public class RestartLobbyButton : MonoBehaviour
    {
        public void Run()
        {
            Command.Publish(new RestartLobby());
        }
    }
}