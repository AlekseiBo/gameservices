using Toolset;
using UnityEngine;

namespace GameServices
{
    public abstract class BaseCanvas : MonoBehaviour
    {
        public Canvas Canvas;
        public CanvasGroup CanvasGroup;
        public bool IsAdditive;

        public virtual void UpdateCanvas(IMediatorCommand command)
        {
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Canvas = GetComponent<Canvas>();
            CanvasGroup = GetComponent<CanvasGroup>();
        }
#endif
    }
}