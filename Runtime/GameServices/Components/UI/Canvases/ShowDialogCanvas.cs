using Toolset;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameServices
{
    public class ShowDialogCanvas : BaseCanvas
    {
        [Space] [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button firstButton;
        [SerializeField] private Button secondButton;

        private TextMeshProUGUI firstButtonText;
        private TextMeshProUGUI secondButtonText;

        private void Awake()
        {
            firstButtonText = firstButton.GetComponentInChildren<TextMeshProUGUI>(true);
            secondButtonText = secondButton.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        private void OnDestroy()
        {
            firstButton.onClick.RemoveAllListeners();
            secondButton.onClick.RemoveAllListeners();
        }

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as ShowDialog;
            title.text = data.Title;
            message.text = data.Message;
            firstButtonText.text = data.FirstTitle;
            secondButtonText.text = data.SecondTitle;
            //LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());
            UpdateButtonListener(firstButton, data.FirstAction ?? HideCanvas);

            if (data.SecondAction != null)
                UpdateButtonListener(secondButton, data.SecondAction);
            else if (data.FirstAction != null)
                UpdateButtonListener(secondButton, HideCanvas);
            else
                secondButton.gameObject.SetActive(false);
        }

        private void UpdateButtonListener(Button button, UnityAction buttonAction)
        {
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(buttonAction);
            button.onClick.AddListener(HideCanvas);
        }
    }
}