using Toolset;

namespace GameServices
{
    public class ShowLoadingProgress : IMediatorCommand
    {
        public float Progress;

        public ShowLoadingProgress(float progress)
        {
            Progress = progress;
        }
    }
}