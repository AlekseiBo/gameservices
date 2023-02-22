using UnityEngine;

namespace GameServices
{
    public class CoroutineComponent : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}