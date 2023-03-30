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
            if (props == null) return;

            if (props.font != null) textComponent.font = props.font;
            if (props.material != null) textComponent.fontMaterial = props.material;
            textComponent.fontStyle = props.fontStyle;
            if (props.fontSize != 0f) textComponent.fontSize = props.fontSize;
            textComponent.color = props.color;
        }
    }
}