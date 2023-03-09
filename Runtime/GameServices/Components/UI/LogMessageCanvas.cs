using System.Collections;
using Toolset;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameServices
{
    public class LogMessageCanvas : BaseCanvas
    {
        [Space]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI message;
        [Space]
        [SerializeField] private float visibilityTimer = 3f;
        [Space]
        [SerializeField] private Sprite green;
        [SerializeField] private Sprite yellow;
        [SerializeField] private Sprite red;

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as LogMessage;
            message.text = data.Message;

            icon.sprite = data.LogType switch
            {
                LogType.Error => red,
                LogType.Assert => red,
                LogType.Warning => yellow,
                LogType.Log => green,
                LogType.Exception => red,
                _ => icon.sprite
            };
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
            StopAllCoroutines();
            StartCoroutine(HideAfterTimeout());
        }

        private IEnumerator HideAfterTimeout()
        {
            yield return Utilities.WaitFor(visibilityTimer);
            HideCanvas();
        }
    }
}