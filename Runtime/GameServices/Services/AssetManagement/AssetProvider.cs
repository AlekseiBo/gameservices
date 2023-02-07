using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameServices.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> handles = new();

        public AssetProvider()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (completedCache.TryGetValue(address, out var completedHandle))
                return completedHandle.Result as T;

            var handle = Addressables.LoadAssetAsync<T>(address);
            return await RunCachedOnComplete(handle, cacheKey: address);
        }

        public void Unload(string address)
        {
            if (!completedCache.TryGetValue(address, out var completedHandle)) return;

            if (handles.TryGetValue(address, out var handlesList))
            {
                foreach (AsyncOperationHandle handle in handlesList)
                    ReleaseHandle(handle);

                handlesList.Clear();
                handles.Remove(address);
            }
            else
            {
                ReleaseHandle(completedHandle);
            }

            completedCache.Remove(address);
        }

        public async Task<SceneInstance> LoadScene(string address, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var handle = Addressables.LoadSceneAsync(address, mode);
            return await RunCachedOnComplete(handle, cacheKey: address);
        }

        public async Task<bool> UnloadScene(string address)
        {
            if (!completedCache.TryGetValue(address, out var completedHandle)) return false;
            await Addressables.UnloadSceneAsync(completedHandle).Task;
            Unload(address);
            return true;
        }

        public async Task<GameObject> Instantiate(string address)
        {
            var prefab = await Load<GameObject>(address);
            return Object.Instantiate(prefab);
        }

        public async Task<GameObject> Instantiate(string address, Vector3 at)
        {
            var prefab = await Load<GameObject>(address);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public async Task<GameObject> Instantiate(string address, Transform under)
        {
            var prefab = await Load<GameObject>(address);
            return Object.Instantiate(prefab, under);
        }

        public void ReleaseCachedAssets()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in handles.Values)
            foreach (AsyncOperationHandle handle in resourceHandles)
                ReleaseHandle(handle);

            completedCache.Clear();
            handles.Clear();
        }

        private async Task<T> RunCachedOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : notnull
        {
            handle.Completed += completeHandle => { completedCache[cacheKey] = completeHandle; };
            AddHandle<T>(cacheKey, handle);
            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : notnull
        {
            if (!handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        private void ReleaseHandle(AsyncOperationHandle handle)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
    }
}