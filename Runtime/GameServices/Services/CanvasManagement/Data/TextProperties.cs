using TMPro;
using UnityEngine;

namespace GameServices
{
    [CreateAssetMenu(fileName = "Text Properties", menuName = "Presets/Text Properties", order = 0)]
    public class TextProperties : ScriptableObject
    {
        public TMP_FontAsset font;
        public Material material;
        public FontStyles fontStyle;
        public float fontSize;
        public Color color;
    }
}