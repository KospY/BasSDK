using UnityEngine;

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class DisableRendererDuringProbeVolumeGeneration : MonoBehaviour
    {
#if UNITY_EDITOR
        public new Renderer renderer;

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
            Debug.Log("Disabled renderer " + renderer.name);
            renderer.enabled = false;
        }

        private void OnVolumeGenerationEnded()
        {
            Debug.Log("Enabled renderer " + renderer.name);
            renderer.enabled = true;
        }
#endif
    }
}
