﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameServices
{
    public interface IAssetProvider : IService
    {
        bool SceneInstance(string address, out SceneInstance instance);
        Task PreloadAsset();
        Task<T> Load<T>(string address, bool persistent = false) where T : class;
        void Unload(string address);
        Task<List<T>> LoadLabel<T>(string label, bool persistent = false) where T : class;
        Task<SceneInstance> LoadScene(string address, LoadSceneMode mode = LoadSceneMode.Single, bool persistent = false);
        Task<bool> UnloadScene(string address);
        Task<GameObject> Instantiate(string address, bool persistent = false);
        Task<GameObject> Instantiate(string address, Vector3 at, bool persistent = false);
        Task<GameObject> Instantiate(string address, Transform under, bool persistent = false);
        void ReleaseCachedAssets();
    }
}