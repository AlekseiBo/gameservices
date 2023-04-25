using Toolset;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameServices
{
    public class ShowMessageCanvas : BaseCanvas
    {
        [Space] [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button button;

        private void OnDestroy() => button.onClick.RemoveAllListeners();

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as ShowMessage;
            title.text = data.Title;
            message.text = data.Message;
            UpdateLayout(message);
            UpdateButtonListener(data.Action ?? HideCanvas);
        }

        private void UpdateButtonListener(UnityAction buttonAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(buttonAction);
            button.onClick.AddListener(HideCanvas);
        }
    }
}