using TMPro;
using Toolset;
using UnityEngine;

namespace GameServices
{
    [RequireComponent((typeof(TMP_InputField)))]
    public class UpdateGameDataText : MonoBehaviour
    {
        [SerializeField] private Key keyCode;

        private TMP_InputField codeText;

        private void Start()
        {
            codeText = GetComponent<TMP_InputField>();
            codeText.text = GameData.Subscribe<string>(keyCode, UpdateText);
        }

        private void OnDestroy() => GameData.RemoveSubscriber<string>(keyCode, UpdateText);

        public void UpdateText(string value) => GameData.Set(keyCode, value);

        private void UpdateText(DataEntry<string> code) => codeText.text = code.Value;

    }
}