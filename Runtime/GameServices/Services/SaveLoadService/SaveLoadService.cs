using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset;
using Unity.Services.CloudSave;
using UnityEngine;

namespace GameServices
{
    class SaveLoadService : ISaveLoadService
    {
        public async Task<bool> SaveProgress(ProgressData progress)
        {
            try
            {
                var data = new Dictionary<string, object>
                {
                    { "PlayerPosition", JsonUtility.ToJson(progress.PlayerPosition) },
                    { "CratePosition", JsonUtility.ToJson(progress.CratePosition) },
                    { "CoinPosition", JsonUtility.ToJson(progress.CoinPosition) },
                };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
                return true;
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public async Task<ProgressData> LoadProgress()
        {
            try
            {
                var loadingData = new HashSet<string> { "PlayerPosition", "CratePosition", "CoinPosition" };
                var data = await CloudSaveService.Instance.Data.LoadAsync(loadingData);

                var playerPosition = data.ContainsKey("PlayerPosition")
                    ? JsonUtility.FromJson<PositionList>(data["PlayerPosition"])
                    : new PositionList();

                var cratePosition = data.ContainsKey("CratePosition")
                    ? JsonUtility.FromJson<PositionList>(data["CratePosition"])
                    : new PositionList();

                var coinPosition = data.ContainsKey("CoinPosition")
                    ? JsonUtility.FromJson<PositionList>(data["CoinPosition"])
                    : new PositionList();

                Debug.Log("Progress loaded from the cloud");
                return new ProgressData
                {
                    PlayerPosition = playerPosition,
                    CratePosition = cratePosition,
                    CoinPosition = coinPosition
                };
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e.Message);
                return new ProgressData();
            }
        }
    }
}