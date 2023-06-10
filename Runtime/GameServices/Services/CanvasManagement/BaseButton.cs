using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButton : MonoBehaviour
    {
        protected Button button;

        protected void Awake() => button = GetComponent<Button>();

        protected void Start() => button.onClick.AddListener(OnClick);

        protected void OnDestroy() => button.onClick.RemoveListener(OnClick);

        protected abstract void OnClick();
    }
}