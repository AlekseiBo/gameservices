using System.Threading.Tasks;
using Toolset;

namespace GameServices
{
    public interface IProgressProvider : IService
    {
        Task InitializeProgress();
        ProgressData ProgressData { get; }
        void LoadProgress();
        Task SaveProgress();
        ProgressData Register(IProgressReader progressReader);
        void Unregister(IProgressReader progressReader);
    }
}