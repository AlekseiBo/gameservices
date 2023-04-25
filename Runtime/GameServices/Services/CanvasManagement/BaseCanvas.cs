using System.Collections;
using Toolset;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public abstract class BaseCanvas : MonoBehaviour
    {
        public bool Visible { get; private set; }

        public Canvas Canvas;
        public CanvasGroup CanvasGroup;
        [Space]
        public ScreenOrientation Orientation = ScreenOrientation.AutoRotation;
        public bool ForceOrientation;
        public bool Additive;
        public bool Distinct;

        public virtual void ShowCanvas()
        {
            UpdateScreenOrientation();
            UpdateLayout();
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

        protected void UpdateLayout()
        {
            StartCoroutine(UpdateLayoutCoroutine());
        }

        private IEnumerator UpdateLayoutCoroutine()
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        }

        private void UpdateScreenOrientation()
        {
            if (ForceOrientation || Orientation != ScreenOrientation.AutoRotation)
                Screen.orientation = Orientation;
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