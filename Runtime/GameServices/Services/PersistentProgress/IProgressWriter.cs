namespace GameServices
{
    public interface IProgressWriter : IProgressReader
    {
        void SaveProgress(ref ProgressData progress);
    }
}