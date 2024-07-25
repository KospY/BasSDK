using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Particle Intensity Converter")]
    public class ManikinPropertyParticleEmissionRateConverter : ManikinPropertyConverterBase
    {
        public float min = 0f;
        public float max = 10f;

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if(obj.TryGetComponent(out ParticleSystem particleSystem))
            {
                var emission = particleSystem.emission;
                emission.rateOverTime = Mathf.Lerp(min, max, values[0]);
            }
        }
    }
}
