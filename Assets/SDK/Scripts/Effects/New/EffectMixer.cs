using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectMixer : MonoBehaviour
    {
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float speed;

        public bool playOnStart = false;

        public List<EffectMixerModule> modules;

        protected bool initialized;

        private void OnValidate()
        {
            modules = new List<EffectMixerModule>(this.GetComponentsInChildren<EffectMixerModule>());
            if (Application.isPlaying && initialized)
            {
                Refresh();
            }
        }

        private void Start()
        {
            if (playOnStart) Play();
            initialized = true;
        }

        [Button]
        public void Play()
        {
            Refresh();
            foreach (EffectMixerModule module in modules)
            {
                module.Play();
            }
        }

        public void SetIntensity(float intensity)
        {
            this.intensity = intensity;
            foreach (EffectMixerModule module in modules)
            {
                module.SetIntensity(intensity);
            }
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
            foreach (EffectMixerModule module in modules)
            {
                module.SetSpeed(speed);
            }
        }

        public void Refresh()
        {
            foreach (EffectMixerModule module in modules)
            {
                module.SetIntensity(intensity);
                module.SetSpeed(speed);
            }
        }

        [Button]
        public void Stop()
        {
            foreach (EffectMixerModule module in modules)
            {
                module.Stop();
            }
        }
    }
}
