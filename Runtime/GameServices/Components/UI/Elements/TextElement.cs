using TMPro;
using UnityEngine;

namespace GameServices
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextElement : MonoBehaviour
    {
        [SerializeField] private TextProperties props;

        private TextMeshProUGUI textComponent;

        private void OnValidate()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
            UpdateProperties();
        }

        public void UpdateProperties(TextProperties properties)
        {
            props = properties;
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            textComponent.font = props.font;
            textComponent.fontMaterial = props.material;
            textComponent.fontStyle = props.fontStyle;
            textComponent.fontSize = props.fontSize;
            textComponent.color = props.color;
        }
    }
}