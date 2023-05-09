using UnityEngine;

namespace GameServices
{
    public class RendererMaterial : MonoBehaviour
    {
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private Material rendererMaterial;

        public void SetMaterialColor(Color color)
        {
            foreach (var mat in meshRenderer.materials)
            {
                if (mat.name.Contains(rendererMaterial.name))
                    mat.color = color;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            meshRenderer = meshRenderer ? meshRenderer : GetComponent<Renderer>();
            rendererMaterial = rendererMaterial ? rendererMaterial : meshRenderer.sharedMaterial;
        }
#endif
    }
}