using UnityEngine;

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class DisableRendererDuringProbeVolumeGeneration : MonoBehaviour
    {
#if UNITY_EDITOR
        public new Renderer renderer;

        private void OnValidate()
        {
            if (renderer == null)
            {
                renderer = this.gameObject.GetComponent<Renderer>();
            }
            if (renderer == null)
            {
                Debug.LogErrorFormat(this, "No renderer assigned on the component [DisableRendererDuringProbeVolumeGeneration]");
            }
        }

        private void OnEnable()
        {
            LightProbeVolumeGenerator.OnGenerationStarted += OnVolumeGenerationStarted;
            LightProbeVolumeGenerator.OnGenerationEnded += OnVolumeGenerationEnded;
        }

        private void OnDisable()
        {
            LightProbeVolumeGenerator.OnGenerationStarted -= OnVolumeGenerationStarted;
            LightProbeVolumeGenerator.OnGenerationEnded -= OnVolumeGenerationEnded;
        }

        private void OnVolumeGenerationStarted()
        {
            if (renderer != null)
            {
                Debug.Log("Disabled renderer " + renderer.name);
                renderer.enabled = false;
            }
        }

        private void OnVolumeGenerationEnded()
        {
            if (renderer != null)
            {
                Debug.Log("Enabled renderer " + renderer.name);
                renderer.enabled = true;
            }
        }
#endif
    }
}
