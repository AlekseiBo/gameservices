using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameServices
{
    public class ProgressProvider : IProgressProvider
    {
        public ProgressData ProgressData => progressData;

        private ProgressData progressData;
        private List<IProgressReader> ProgressReaders { get; } = new ();
        private List<IProgressWriter> ProgressWriters { get; } = new ();

        private readonly ISaveLoadService saveService;

        public ProgressProvider(ISaveLoadService saveService)
        {
            this.saveService = saveService;
            InitializeProgress();
        }

        public void LoadProgress()
        {
            foreach (var reader in ProgressReaders) reader?.LoadProgress(progressData);
        }

        public async Task SaveProgress()
        {
            foreach (var reader in ProgressWriters) reader?.SaveProgress(ref progressData);
            await saveService.SaveProgress(progressData);
        }

        public ProgressData Register(IProgressReader progressReader)
        {
            ProgressReaders.Add(progressReader);

            if (progressReader is IProgressWriter progressWriter)
                ProgressWriters.Add(progressWriter);

            return progressData;
        }

        public void Unregister(IProgressReader progressReader)
        {
            ProgressReaders.Remove(progressReader);

            if (progressReader is IProgressWriter progressWriter)
                ProgressWriters.Remove(progressWriter);
        }

        private async void InitializeProgress() => progressData = await saveService.LoadProgress() ?? NewProgress();

        private ProgressData NewProgress() => new ProgressData();
    }
}