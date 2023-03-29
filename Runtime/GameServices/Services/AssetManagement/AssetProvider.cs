using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameServices.CodeBlocks;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameServices
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, SceneInstance> sceneInstances = new();
        private readonly Dictionary<string, AsyncOperationHandle> completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> handles = new();

        public bool SceneInstance(string address, out SceneInstance instance)
        {
            if (sceneInstances.TryGetValue(address, out var sceneInstance))
            {
                instance = sceneInstance;
                return true;
            }

            instance = default;
            return false;
        }

        public AssetProvider()
        {
            Addressables.InitializeAsync();
        }

        public async Task PreloadAsset()
        {
            var locationsList = await Addressables.LoadResourceLocationsAsync("preload").Task;
            await Addressables.DownloadDependenciesAsync(locationsList).Task;
        }

        public async Task<T> Load<T>(string address, bool persistent = false) where T : class
        {
            if (completedCache.TryGetValue(address, out var completedHandle))
                return completedHandle.Result as T;

            var handle = Addressables.LoadAssetAsync<T>(address);
            CoroutineRunner.Start(WatchLoadingProgress(handle));
            return await RunCachedOnComplete(handle, cacheKey: address, persistent);
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

        public async Task<List<T>> LoadLabel<T>(string label, bool persistent = false) where T : class
        {
            var handle = Addressables.LoadResourceLocationsAsync(label);
            await handle.Task;
            var resultList = new List<T>();
            var assetKey = "";

            foreach (var addressable in handle.Result)
            {
                if (assetKey == addressable.PrimaryKey) continue;

                assetKey = addressable.PrimaryKey;
                var asset = await Load<T>(addressable.PrimaryKey, persistent);
                if (!resultList.Contains(asset)) resultList.Add(asset);
            }

            return resultList;
        }

        public async Task<SceneInstance> LoadScene(string address, LoadSceneMode mode = LoadSceneMode.Single, bool persistent = false)
        {
            var handle = Addressables.LoadSceneAsync(address, mode);
            CoroutineRunner.Start(WatchLoadingProgress(handle));
            var sceneInstance = await RunCachedOnComplete(handle, cacheKey: address, persistent);
            sceneInstances[address] = sceneInstance;
            return sceneInstance;
        }

        public async Task<bool> UnloadScene(string address)
        {
            if (!completedCache.TryGetValue(address, out var completedHandle)) return false;
            var handle = Addressables.UnloadSceneAsync(completedHandle);
            CoroutineRunner.Start(WatchLoadingProgress(handle));
            await handle.Task;
            sceneInstances.Remove(address);
            Unload(address);
            return true;
        }

        public async Task<GameObject> Instantiate(string address, bool persistent = false)
        {
            var prefab = await Load<GameObject>(address, persistent);
            return prefab == null ? null : Object.Instantiate(prefab);
        }

        public async Task<GameObject> Instantiate(string address, Vector3 at, bool persistent = false)
        {
            var prefab = await Load<GameObject>(address, persistent);
            return prefab == null ? null : Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public async Task<GameObject> Instantiate(string address, Transform under, bool persistent = false)
        {
            var prefab = await Load<GameObject>(address, persistent);
            return prefab == null ? null : Object.Instantiate(prefab, under);
        }

        public void ReleaseCachedAssets()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in handles.Values)
            foreach (AsyncOperationHandle handle in resourceHandles)
                ReleaseHandle(handle);

            completedCache.Clear();
            handles.Clear();
        }

        private async Task<T> RunCachedOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey, bool persistent)
            where T : notnull
        {
            handle.Completed += completeHandle => { completedCache[cacheKey] = completeHandle; };
            if (!persistent) AddHandle<T>(cacheKey, handle);
            try
            {
                return await handle.Task;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
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

        private IEnumerator WatchLoadingProgress(AsyncOperationHandle handle)
        {
            if (!CanvasManager.IsInitialized) yield break;
            var canvas = Services.All.Single<ICanvasManager>();
            var progressData = new ShowLoadingProgress(handle.PercentComplete);

            while (handle.IsValid() && !handle.Task.IsCompleted)
            {
                if (progressData.Progress != handle.PercentComplete)
                {
                    progressData.Progress = handle.PercentComplete;
                    Command.Publish(progressData);
                }

                yield return Utilities.WaitFor(0.1f);
            }

            canvas.HideCanvas(progressData);
        }
    }
}