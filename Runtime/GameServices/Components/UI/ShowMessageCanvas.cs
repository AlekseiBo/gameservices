using Toolset;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameServices
{
    public class ShowMessageCanvas : BaseCanvas
    {
        [Space]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button button;

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as ShowMessage;
            title.text = data.Title;
            message.text = data.Message;
            button.onClick.RemoveAllListeners();

            if (data.Call != null)
                button.onClick.AddListener(data.Call);
            else
                button.onClick.AddListener(() =>
                    Services.All.Single<ICanvasManager>().HideCanvas(typeof(ShowMessage).ToString()));
        }
    }
}