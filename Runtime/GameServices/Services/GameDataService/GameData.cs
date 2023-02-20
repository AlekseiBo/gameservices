using System;
using System.Collections.Generic;
using UnityEngine;
using Toolset;

namespace GameServices
{
    public class GameData<TKey> where TKey : struct, Enum
    {
        private IStaticDataService<TKey> staticData;
        private static Dictionary<TKey, ScriptableObject> gameData;

        public GameData()
        {
            staticData = Services.All.Single<IStaticDataService<TKey>>();
            gameData = staticData.AllGameData();
        }

        public static T Get<T>(TKey key) => GetEntry<T>(key).Value;

        public static void Set<T>(TKey key, T value) => GetEntry<T>(key).Set(value);

        public static T Subscribe<T>(TKey key, Action<DataEntry<T>> action)
        {
            var dataEntry = GetEntry<T>(key);
            dataEntry.Subscribe(action);
            return dataEntry.Value;
        }

        public static void RemoveSubscriber<T>(TKey key, Action<DataEntry<T>> action) =>
            GetEntry<T>(key).RemoveSubscriber(action);

        private static DataEntry<T> GetEntry<T>(TKey key)
        {
            if (!gameData.TryGetValue(key, out var dataContainer)) return default;
            return dataContainer as DataEntry<T>;
        }
    }
}