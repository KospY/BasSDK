using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class FxController : MonoBehaviour
    {
        [Header("General")]
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float speed;
        public bool playOnStart = false;

        [Header("Velocity to Speed")]
        public bool velocityToSpeed;
        public Rigidbody rb;
        public Vector2 velocityToSpeedRange = new Vector2(0,5);
        public float velocityToSpeedDampening = 1f;

        [Header("Detected Modules")]
        public List<FxModule> modules;

        protected float dampenedSpeed;
        protected bool initialized;

        private void OnValidate()
        {
            modules = new List<FxModule>(this.GetComponentsInChildren<FxModule>());
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

        protected void Update()
        {
            if (velocityToSpeed)
            {
                if (!rb.isKinematic && !rb.IsSleeping())
                {
                    Vector3 pointVelocity = rb.GetPointVelocity(this.transform.position);
                    dampenedSpeed = Mathf.Lerp(dampenedSpeed, Mathf.InverseLerp(velocityToSpeedRange.x, velocityToSpeedRange.y, pointVelocity.magnitude), velocityToSpeedDampening);
                    SetSpeed(dampenedSpeed);
                }
                else if (dampenedSpeed > 0)
                {
                    dampenedSpeed = Mathf.Lerp(dampenedSpeed, 0, velocityToSpeedDampening);
                }
            }
        }

        [Button]
        public void Play()
        {
            Refresh();
            foreach (FxModule module in modules)
            {
                module.Play();
            }
        }

        public void SetIntensity(float intensity)
        {
            this.intensity = intensity;
            foreach (FxModule module in modules)
            {
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
                module.SetIntensity(intensity);
                module.SetSpeed(speed);
            }
        }

        [Button]
        public void Stop()
        {
            foreach (FxModule module in modules)
            {
                module.Stop();
            }
        }
    }
}
