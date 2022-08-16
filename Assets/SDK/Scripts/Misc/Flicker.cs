using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThunderRoad
{
    [RequireComponent(typeof(Light))]
    public class Flicker : MonoBehaviour
    {
        public float targetIntensity = 1;
        public float lerpRate = 10;
        public float flickerRate = 10;
        public float flickerAmount = 0.5f;
        public bool lightActive;

        public float flicker;
        
        private Light lightSource;
        private float actualIntensity;
        private float seed = 0;

        public void On(bool lerped = true)
        {
            lightActive = true;
            if (!lerped) lightSource.intensity = targetIntensity;
        }

        public void Off(bool lerped = true)
        {
            lightActive = false;
            if (!lerped) lightSource.intensity = 0;
        }

        private void Start()
        {
            lightSource = GetComponent<Light>();
            seed = Random.Range(0, 10000);
        }

        private void Update()
        {
            float target = lightActive ? targetIntensity : 0;
            actualIntensity = Mathf.Lerp(actualIntensity, target, Time.deltaTime * lerpRate);
            flicker = 1
                            + flickerAmount
                            * (2f * Mathf.Clamp01(Mathf.PerlinNoise(Time.time * flickerRate, seed)) - 1f);
            lightSource.intensity = actualIntensity * flicker;

        }
    }
}