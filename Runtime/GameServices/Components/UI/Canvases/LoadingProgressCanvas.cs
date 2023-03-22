using Toolset;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class LoadingProgressCanvas : BaseCanvas
    {
        [Space]
        [SerializeField] private Slider progressSlider;

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as ShowLoadingProgress;
            progressSlider.value = data.Progress;
        }
    }
}