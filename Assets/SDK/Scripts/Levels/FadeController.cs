using UnityEngine;
using UnityEngine.Rendering;

namespace ThunderRoad
{
    public class FadeController : MonoBehaviour
    {
        public float weight = 0;
        public Volume volume;

        protected float currentWeight;

        void Update()
        {
            if (currentWeight != weight)
            {
                currentWeight = weight;
                if (Common.IsAndroid)
                {
                    Shadowood.Tonemapping.SetExposureStatic(-10 * weight);
                }
                else
                {
                    if (volume.enabled)
                    {
                        if (volume.weight == 0)
                        {
                            volume.enabled = false;
                        }
                    }
                    else if (volume.weight > 0)
                    {
                        volume.enabled = true;
                    }
                    volume.weight = weight;
                }
            }
        }

        private void OnDisable()
        {
            if (Common.IsAndroid)
            {
                Shadowood.Tonemapping.SetExposureStatic(-10 * 0);
            }
            else
            {
                volume.weight = 0;
                volume.enabled = false;
            }
        }
    }
}