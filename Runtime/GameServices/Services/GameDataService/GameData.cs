using System;
using System.Collections.Generic;
using GameServices.StaticData;
using UnityEngine;
using Framework;

namespace GameServices.GameDataService
{
    public class GameData
    {
        private IStaticDataService staticData;
        private static Dictionary<Key, ScriptableObject> gameData;

        public GameData()
        {
            staticData = Services.All.Single<IStaticDataService>();
            gameData = staticData.AllGameData();
        }

        public static T Get<T>(Key key) => GetEntry<T>(key).Value;

        public static void Set<T>(Key key, T value) => GetEntry<T>(key).Set(value);

        public static T Subscribe<T>(Key key, Action<DataEntry<T>> action)
        {
            var dataEntry = GetEntry<T>(key);
            dataEntry.Subscribe(action);
            return dataEntry.Value;
        }

        public static void RemoveSubscriber<T>(Key key, Action<DataEntry<T>> action) =>
            GetEntry<T>(key).RemoveSubscriber(action);

        private static DataEntry<T> GetEntry<T>(Key key)
        {
            if (!gameData.TryGetValue(key, out var dataContainer)) return default;
            return dataContainer as DataEntry<T>;
        }
    }
}