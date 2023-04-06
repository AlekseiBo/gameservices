using Toolset;
using UnityEngine;

namespace GameServices
{
    public abstract class BaseCanvas : MonoBehaviour
    {
        public bool Visible { get; private set; }

        public Canvas Canvas;
        public CanvasGroup CanvasGroup;
        public bool Additive;
        public bool Distinct;

        public virtual void ShowCanvas()
        {
            Visible = true;
            Canvas.enabled = true;
            CanvasGroup.interactable = true;
        }

        public virtual void HideCanvas()
        {
            Visible = false;
            Canvas.enabled = false;
            CanvasGroup.interactable = false;
        }

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