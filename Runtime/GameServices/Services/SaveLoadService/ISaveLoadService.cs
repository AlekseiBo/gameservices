using System.Threading.Tasks;
using Toolset;

namespace GameServices
{
    public interface ISaveLoadService : IService
    {
        Task<bool> SaveProgress(ProgressData progress);
        Task<ProgressData> LoadProgress();
    }
}