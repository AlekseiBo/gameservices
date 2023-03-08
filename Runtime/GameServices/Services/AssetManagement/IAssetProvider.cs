using System.Threading.Tasks;
using Toolset;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameServices
{
    public interface IAssetProvider : IService
    {
        Task<T> Load<T>(string address, bool persistent = false) where T : class;
        void Unload(string address);
        Task<GameObject> Instantiate(string address, bool persistent = false);
        Task<GameObject> Instantiate(string address, Vector3 at, bool persistent = false);
        Task<GameObject> Instantiate(string address, Transform under, bool persistent = false);
        Task<SceneInstance> LoadScene(string address, LoadSceneMode mode = LoadSceneMode.Single);
        Task<bool> UnloadScene(string address);
        void ReleaseCachedAssets();
    }
}