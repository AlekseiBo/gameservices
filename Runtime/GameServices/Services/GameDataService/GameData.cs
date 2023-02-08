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
        private static Dictionary<DataKey, ScriptableObject> gameData;

        public GameData()
        {
            staticData = Framework.Services.All.Single<IStaticDataService>();
            gameData = staticData.AllGameData();

            Debug.Log(Get<string>(DataKey.PlayerName));
            Debug.Log(Get<int>(DataKey.PlayerScore));
        }

        public static T Get<T>(DataKey key) => GetEntry<T>(key).Value;

        public static T Subscribe<T>(DataKey key, Action<DataEntry<T>> action)
        {
            var dataEntry = GetEntry<T>(key);
            dataEntry.Subscribe(action);
            return dataEntry.Value;
        }

        public static void RemoveSubscriber<T>(DataKey key, Action<DataEntry<T>> action) =>
            GetEntry<T>(key).RemoveSubscriber(action);

        private static DataEntry<T> GetEntry<T>(DataKey key)
        {
            if (!gameData.TryGetValue(key, out var dataContainer)) return default;
            return dataContainer as DataEntry<T>;
        }

        public static void Set<T>(DataKey key, T value) => GetEntry<T>(key).Set(value);
    }
}