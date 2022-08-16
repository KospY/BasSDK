using System.Collections.Generic;
using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxController")]
    public class FxController : MonoBehaviour
    {
        [Header("Variables")]
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float speed;
        public Vector3 direction;

        [Header("Options")]
        public bool playOnStart = false;
        public float lifeTime = 0;

        [Header("Detected Modules")]
        public List<FxModule> modules;

#if ODIN_INSPECTOR
        [NonSerialized, ShowInInspector, ReadOnly]
#endif
        public object source;

        public event Action onLifetimeExpired;

        protected bool initialized;

        private void OnValidate()
        {
            modules = new List<FxModule>(this.GetComponentsInChildren<FxModule>());
            foreach (FxModule module in modules)
            {
                module.controller = this;
            }
            if (Application.isPlaying && initialized)
            {
                Refresh();
            }
        }

        private void Start()
        {
            source = this.GetComponentInParent<Item>();
            if (source == null)
            {
                source = this.GetComponentInParent<Creature>();
            }
            if (playOnStart) Play();
            initialized = true;
        }

        [Button]
        public void Play()
        {
            Refresh();
            foreach (FxModule module in modules)
            {
                module.controller = this;
                module.Play();
            }
            if (lifeTime > 0) Invoke("OnLifetimeExpired", lifeTime);
        }

        public void SetIntensity(float intensity)
        {
            this.intensity = intensity;
            foreach (FxModule module in modules)
            {
                module.controller = this;
                module.SetIntensity(intensity);
            }
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
            foreach (FxModule module in modules)
            {
                module.SetSpeed(speed);
            }
        }

        public void Refresh()
        {
            foreach (FxModule module in modules)
            {
                module.controller = this;
                module.SetIntensity(intensity);
                module.SetSpeed(speed);
            }
        }

        [Button]
        public void Stop()
        {
            foreach (FxModule module in modules)
            {
                module.controller = this;
                module.Stop();
            }
        }

        protected void OnLifetimeExpired()
        {
            if (onLifetimeExpired != null) onLifetimeExpired.Invoke();
        }
    }
}
