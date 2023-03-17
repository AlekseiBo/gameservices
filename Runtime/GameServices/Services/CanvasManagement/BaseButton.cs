using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButton : MonoBehaviour
    {
        private Button button;

        private void Awake() => button = GetComponent<Button>();

        private void Start() => button.onClick.AddListener(OnClick);

        private void OnDestroy() => button.onClick.RemoveListener(OnClick);

        protected abstract void OnClick();
    }
}