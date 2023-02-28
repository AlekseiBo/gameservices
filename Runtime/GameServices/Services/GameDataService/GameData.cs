using System;
using System.Collections.Generic;
using UnityEngine;
using Toolset;

namespace GameServices
{
    public class GameData<TKey> where TKey : struct, Enum
    {
        public static bool isInitialized;

        private IStaticDataService staticData;
        private static Dictionary<TKey, ScriptableObject> gameData;

        protected GameData()
        {
            staticData = Services.All.Single<IStaticDataService>();
            gameData = staticData.AllGameData<TKey>();
            isInitialized = true;
        }

        public static T Get<T>(TKey key) => GetEntry<T>(key).Value;

        public static void Set<T>(TKey key, T value) => GetEntry<T>(key).Set(value);

        public static T Subscribe<T>(TKey key, Action<DataEntry<T>> action)
        {
            var dataEntry = GetEntry<T>(key);
            dataEntry.Subscribe(action);
            return dataEntry.Value;
        }

        public static void RemoveSubscriber<T>(TKey key, Action<DataEntry<T>> action)
        {
            if (action != null) GetEntry<T>(key).RemoveSubscriber(action);
        }

        private static DataEntry<T> GetEntry<T>(TKey key)
        {
            if (!gameData.TryGetValue(key, out var dataContainer)) return default;
            return dataContainer as DataEntry<T>;
        }
    }

    public class GameData : GameData<Key>
    {
    }
}