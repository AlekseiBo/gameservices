using System.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameServices.AssetManagement
{
    public interface IAssetProvider : IService
    {
        Task<T> Load<T>(string address) where T : class;
        void Unload(string address);
        Task<GameObject> Instantiate(string address);
        Task<GameObject> Instantiate(string address, Vector3 at);
        Task<GameObject> Instantiate(string address, Transform under);
        Task<SceneInstance> LoadScene(string address, LoadSceneMode mode = LoadSceneMode.Single);
        Task<bool> UnloadScene(string address);
        void ReleaseCachedAssets();
    }
}