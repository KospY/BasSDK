using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/LightController")]
    public class LightController : MonoBehaviour
    {
        [Header("Intensity to intensity (0 = disabled)")]
        public float minIntensity;
        public float maxIntensity;
        public float randomRangeIntensity;

        [Header("Intensity to range (0 = disabled)")]
        public float minRange;
        public float maxRange;
        public float randomRange;

        public virtual void SetActive(bool active)
        {
            Light light = this.GetComponent<Light>();
            light.enabled = active;

        }

        public virtual void SetIntensity(float value)
        {
            Light light = this.GetComponent<Light>();
            if (maxIntensity > 0)
            {
                light.intensity = Mathf.Lerp(minIntensity, maxIntensity, value);
            }
            if (maxRange > 0)
            {
                light.range = Mathf.Lerp(minRange, maxRange, value);
            }
        }

        public virtual void SetColor(Color mainColor)
        {
            Light light = this.GetComponent<Light>();
            light.color = mainColor;
        }
    }
}
