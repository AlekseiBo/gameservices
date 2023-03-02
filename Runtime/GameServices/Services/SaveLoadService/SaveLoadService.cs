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
                var dataName = "ProgressData";
                var dataContent = JsonUtility.ToJson(progress);
                var data = new Dictionary<string, object> { { dataName, dataContent } };
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
                var dataName = "ProgressData";
                var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { dataName });

                if (!data.ContainsKey(dataName)) return default;

                Debug.Log("Progress loaded from the cloud");
                return JsonUtility.FromJson<ProgressData>(data[dataName]);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }
    }
}