using UnityEngine;
using System;
using System.Collections.Generic;

namespace BS
{
    public class ParticleDefinition : MonoBehaviour
    {
        [Range(0, 1)]
        public float intensity;

        [ColorUsage(true, true)]
        public Color mainColor = Color.white;
        [ColorUsage(true, true)]
        public Color secondaryColor = Color.white;

        public Transform target;

        public Mesh mesh;

        public List<ParticleInfo> particles = new List<ParticleInfo>();

        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();
        protected ParticleSystem.Burst particleBurst =new ParticleSystem.Burst();

        [Serializable]
        public class ParticleInfo
        {
            [Header("Reference")]
            public ParticleSystem particleSystem;
            [Header("Color")]
            public bool mainColor;
            public bool secondaryColor;
            [Header("Intensity to duration (0 = disabled)")]
            public float minDuration;
            public float maxDuration;
            [Header("Intensity to lifetime (0 = disabled)")]
            public float minLifeTime;
            public float maxLifeTime;
            public float randomRangeLifeTime;
            [Header("Intensity to speed (0 = disabled)")]
            public float minSpeed;
            public float maxSpeed;
            public float randomRangeSpeed;
            [Header("Intensity to emission rate (0 = disabled)")]
            public float minRate;
            public float maxRate;
            public float randomRangeRate;
            [Header("Intensity to speed (0 = disabled)")]
            public short minBurst;
            public short maxBurst;
            public short randomRangeBurst;
            [Header("Mesh")]
            public bool mesh;
            [Header("Spawn on collision")]
            public string spawnEffectId;
            public float spawnMinIntensity;
            public float spawnMaxIntensity;
            public bool spawnMainColor;
            public bool spawnSecondaryColor;
        }

        private void OnValidate()
        {
            SetIntensity(intensity);
            SetColor(mainColor, secondaryColor);
            SetTarget(target);
            SetMesh(mesh);
        }

        public virtual void Play()
        {
            foreach (ParticleInfo p in particles)
            {
                p.particleSystem.Play();
            }
        }

        public virtual void Stop()
        {
            foreach (ParticleInfo p in particles)
            {
                p.particleSystem.Stop();
            }
        }

        public virtual void SetIntensity(float value)
        {
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                ParticleSystem.MainModule mainModule = p.particleSystem.main;

                if (p.maxDuration > 0 && !p.particleSystem.isPlaying)
                {
                    mainModule.duration = Mathf.Lerp(p.minDuration, p.maxDuration, value);
                }
                if (p.maxLifeTime > 0)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float lifeTime = Mathf.Lerp(p.minLifeTime, p.maxLifeTime, value);
                    minMaxCurve.constantMin = lifeTime - p.randomRangeLifeTime;
                    minMaxCurve.constantMax = lifeTime + p.randomRangeLifeTime;
                    mainModule.startLifetime = minMaxCurve;
                }
                if (p.maxSpeed > 0)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float speed = Mathf.Lerp(p.minSpeed, p.maxSpeed, value);
                    minMaxCurve.constantMin = speed - p.randomRangeSpeed;
                    minMaxCurve.constantMax = speed + p.randomRangeSpeed;
                    mainModule.startSpeed = minMaxCurve;
                }
                if (p.maxRate > 0)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float rate = Mathf.Lerp(p.minRate, p.maxRate, value);
                    minMaxCurve.constantMin = rate - p.randomRangeRate;
                    minMaxCurve.constantMax = rate + p.randomRangeRate;
                    ParticleSystem.EmissionModule particleEmission = p.particleSystem.emission;
                    particleEmission.rateOverTime = minMaxCurve;
                }
                if (p.maxBurst > 0)
                {
                    particleBurst.time = 0;
                    short burst = (short)Mathf.Lerp(p.minBurst, p.maxBurst, value);
                    particleBurst.minCount = (short)(burst - p.randomRangeBurst);
                    particleBurst.maxCount = (short)(burst + p.randomRangeBurst);
                    p.particleSystem.emission.SetBurst(0, particleBurst);
                }
            }
        }

        public virtual void SetColor(Color mainColor)
        {
            foreach (ParticleInfo p in particles)
            {
                ParticleSystem.MainModule mainModule = p.particleSystem.main;
                if (p.mainColor)
                {
                    mainModule.startColor = mainColor;
                }
            }
        }

        public virtual void SetColor(Color mainColor, Color secondaryColor)
        {
            foreach (ParticleInfo p in particles)
            {
                ParticleSystem.MainModule mainModule = p.particleSystem.main;
                if (p.mainColor)
                {
                    mainModule.startColor = mainColor;
                }
                else if (p.secondaryColor)
                {
                    mainModule.startColor = secondaryColor;
                }
            }
        }

        public virtual void SetTarget(Transform transform)
        {
            foreach (ParticleInfo p in particles)
            {

            }
        }

        public virtual void SetMesh(Mesh mesh)
        {
            foreach (ParticleInfo p in particles)
            {
                if (p.mesh)
                {
                    ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                    shapeModule.mesh = mesh;
                }
            }
        }
    }
}
