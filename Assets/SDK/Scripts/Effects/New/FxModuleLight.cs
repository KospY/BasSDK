using UnityEngine;
using System.Collections;

namespace ThunderRoad
{
    public class FxModuleLight : FxModule
    {
        public bool flicker;
        public float flickerIntensityReduction = 0.2f;
        public float flickerMinSpeed = 0.01f;
        public float flickerMaxSpeed = 0.1f;

        protected new Light light;
        protected float orgLightIntensity;
        protected float currentLightIntensity;


        private void Awake()
        {
            light = this.GetComponent<Light>();
            orgLightIntensity = light.intensity;
            currentLightIntensity = orgLightIntensity;
            if (flicker)
            {
                StartCoroutine(FlickerCoroutine());
            }
        }

        IEnumerator FlickerCoroutine()
        {
            while (true)
            {
                RefreshLightIntensity();
                yield return new WaitForSeconds(UnityEngine.Random.Range(flickerMinSpeed, flickerMaxSpeed));
            }
        }

        protected void RefreshLightIntensity()
        {
            if (flicker)
            {
                light.intensity = Mathf.Clamp(currentLightIntensity - UnityEngine.Random.Range(0, flickerIntensityReduction), 0, Mathf.Infinity);
            }
            else
            {
                light.intensity = currentLightIntensity;
            }
        }

        public override void Play()
        {
            light.enabled = true;
        }

        public override void SetIntensity(float intensity)
        {
            currentLightIntensity = orgLightIntensity * intensity;
            RefreshLightIntensity();
        }

        public override void SetSpeed(float speed)
        {

        }

        public override void Stop()
        {
            light.enabled = false;
        }
    }
}
