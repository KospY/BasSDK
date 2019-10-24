using UnityEngine;
using System;
using System.Collections.Generic;

namespace BS
{
    public class ParticleController : MonoBehaviour
    {
        public List<ParticleInfo> particles = new List<ParticleInfo>();

        [NonSerialized]
        public ParticleSystem rootParticleSystem;

        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();
        protected ParticleSystem.Burst particleBurst = new ParticleSystem.Burst();

        [Serializable]
        public class ParticleInfo
        {
            [Header("Reference")]
            public ParticleSystem particleSystem;
            [Header("Color")]
            public bool mainColor;
            public bool secondaryColor;
            [Header("Color Parameter To Change")]
            public bool startColor;
            public bool emissionColor;
            public bool baseMapColor;
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
            [Header("Intensity to size (0 = disabled)")]
            public float minSize;
            public float maxSize;
            public Vector2 maxRandomConstantSizes;
            [Header("Intensity to emission rate (0 = disabled)")]
            public float minRate;
            public float maxRate;
            public float randomRangeRate;
            [Header("Intensity to shape radius (0 = disabled)")]
            public float minShapeRadius;
            public float maxShapeRadius;
            [Header("Intensity to speed (0 = disabled)")]
            public short minBurst;
            public short maxBurst;
            public short randomRangeBurst;
            [Header("Intensity to light intensity (0 = disabled)")]
            public float minLightIntensity;
            public float maxLightIntensity;
            [Header("Mesh")]
            public bool mesh;
            [Header("Spawn on collision")]
            public string spawnEffectId;
            public LayerMask spawnLayerMask = ~0;
            public float SpawnMaxGroundAngle = 45;
            public float spawnEmitRate = 0.1f;
            public float spawnMinIntensity;
            public float spawnMaxIntensity;
            public bool spawnMainColor;
            public bool spawnSecondaryColor;
        }

        private void OnValidate()
        {
            rootParticleSystem = this.GetComponent<ParticleSystem>();
            if (!rootParticleSystem)
            {
                rootParticleSystem = this.gameObject.AddComponent<ParticleSystem>();
                ParticleSystem.EmissionModule emissionModule = rootParticleSystem.emission;
                emissionModule.enabled = false;
                ParticleSystem.ShapeModule shapeModule = rootParticleSystem.shape;
                shapeModule.enabled = false;
                rootParticleSystem.GetComponent<ParticleSystemRenderer>().enabled = false;
            }
        }

        public virtual void Play()
        {
            rootParticleSystem.Play();
        }

        public virtual void Stop()
        {
            rootParticleSystem.Stop();
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
                if (p.maxSize > 0)
                {
                    mainModule.startSize = Mathf.Lerp(p.minSize, p.maxSize, value);
                }
                else if (Vector3.SqrMagnitude (p.maxRandomConstantSizes) > 0)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    minMaxCurve.constantMin = Mathf.Lerp(p.minSize, p.maxRandomConstantSizes.x, value);
                    minMaxCurve.constantMax = Mathf.Lerp(p.minSize, p.maxRandomConstantSizes.y, value);
                    mainModule.startSize = minMaxCurve;
                }
                if (p.maxShapeRadius > 0)
                {
                    var shape = p.particleSystem.shape;
                    shape.radius = Mathf.Lerp(p.minShapeRadius, p.maxShapeRadius, value);
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
                if (p.maxLightIntensity > 0)
                {
                    var lights = p.particleSystem.lights;
                    lights.intensityMultiplier = Mathf.Lerp(p.minLightIntensity, p.maxLightIntensity, value);
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
                Color finalColor = new Color();

                if (p.mainColor)
                    finalColor = mainColor;
                else if (p.secondaryColor)
                    finalColor = secondaryColor;

                if (p.startColor)
                    mainModule.startColor = finalColor;
                else if (p.emissionColor)
                    p.particleSystem.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", finalColor);
                else if (p.baseMapColor)
                    p.particleSystem.GetComponent<Renderer>().sharedMaterial.SetColor("_BaseMap", finalColor);
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
