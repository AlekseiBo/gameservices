using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButton : MonoBehaviour
    {
        protected Button button;

        protected virtual void Awake() => button = GetComponent<Button>();

        protected virtual void Start() => button.onClick.AddListener(OnClick);

        protected virtual void OnDestroy() => button.onClick.RemoveListener(OnClick);

        protected abstract void OnClick();
    }
}