using Toolset;

namespace GameServices
{
    public interface IProgressReader
    {
        void LoadProgress(ProgressData progress)
        {
        }

        ProgressData RegisterProgress()
        {
            return Services.All.Single<IProgressProvider>().Register(this);
        }

        void UnregisterProgress()
        {
            Services.All.Single<IProgressProvider>().Unregister(this);
        }
    }
}