using UnityEngine;
using System;
using System.Collections.Generic;

namespace BS
{
    public class EffectParticle : Effect
    {
        public int poolCount = 50;
        public List<ParticleInfo> particles = new List<ParticleInfo>();

        [NonSerialized]
        public ParticleSystem rootParticleSystem;

        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();
        protected ParticleSystem.Burst particleBurst = new ParticleSystem.Burst();

        [NonSerialized]
        public float currentValue;
        [NonSerialized]
        public Gradient currentMainGradient;
        [NonSerialized]
        public Gradient currentSecondaryGradient;

        [Serializable]
        public class ParticleInfo
        {
            [Header("Reference")]
            public ParticleSystem particleSystem;

            [Header("Color Gradient")]
            public LinkedGradient linkStartColor = LinkedGradient.None;
            public LinkedGradient linkEmissionColor = LinkedGradient.None;
            public LinkedGradient linkBaseColor = LinkedGradient.None;
            [NonSerialized]
            public ParticleSystemRenderer renderer;
            [NonSerialized]
            public MaterialPropertyBlock materialPropertyBlock;

            public enum LinkedGradient
            {
                None,
                Main,
                Secondary,
            }

            [Header("Main Gradient to ")]
            public bool mainColor;
            public bool secondaryColor;

            [Header("Color Parameter To Change")]
            public bool startColor;
            public bool emissionColor;
            public bool baseMapColor;

            [Header("Intensity to duration")]
            public bool duration;
            public AnimationCurve curveDuration;

            [Header("Intensity to lifetime")]
            public bool lifeTime;
            public AnimationCurve curveLifeTime;
            public float randomRangeLifeTime;

            [Header("Intensity to speed")]
            public bool speed;
            public AnimationCurve curveSpeed;
            public float randomRangeSpeed;

            [Header("Intensity to size")]
            public bool size;
            public AnimationCurve curveSize;
            public float randomRangeSize;

            [Header("Intensity to emission rate")]
            public bool rate;
            public AnimationCurve curveRate;
            public float randomRangeRate;

            [Header("Intensity to shape radius")]
            public bool shapeRadius;
            public AnimationCurve curveShapeRadius;

            [Header("Intensity to speed")]
            public bool burst;
            public AnimationCurve curveBurst;
            public short randomRangeBurst;

            [Header("Intensity to light intensity")]
            public bool lightIntensity;
            public AnimationCurve curveLightIntensity;

            [Header("Mesh")]
            public bool mesh;

            [Header("Collider")]
            public bool collider;

            [Header("Spawn on collision")]
            public string spawnEffectId;
            public LayerMask spawnLayerMask = ~0;
            public float spawnMaxGroundAngle = 45;
            public float spawnEmitRate = 0.1f;
            public float spawnMinIntensity;
            public float spawnMaxIntensity;
            public bool useMainGradient;
            public bool useSecondaryGradient;
        }

        private void OnValidate()
        {
            Init();
        }
#if ProjectCore
        private void Awake()
        {
            Init();
            foreach (ParticleInfo p in particles)
            {
                if (p.spawnEffectId != null && p.spawnEffectId != "")
                {
                    ParticleCollisionSpawner particleCollisionSpawner = p.particleSystem.gameObject.AddComponent<ParticleCollisionSpawner>();
                    particleCollisionSpawner.Init(p);
                }
            }
        }
#endif
        private void Init()
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
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                p.renderer = p.particleSystem.GetComponent<ParticleSystemRenderer>();
                p.materialPropertyBlock = new MaterialPropertyBlock();
            }
        }

        public override void Play()
        {
            rootParticleSystem.Play();
        }

        public override void Stop()
        {
            rootParticleSystem.Stop();
        }

        public override void SetIntensity(float value)
        {
            currentValue = value;
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                ParticleSystem.MainModule mainModule = p.particleSystem.main;

                if (p.duration && !p.particleSystem.isPlaying)
                {
                    mainModule.duration = p.curveDuration.Evaluate(value);
                }
                if (p.lifeTime)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float lifeTime = p.curveLifeTime.Evaluate(value);
                    minMaxCurve.constantMin = Mathf.Clamp(lifeTime - p.randomRangeLifeTime, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(lifeTime, 0, Mathf.Infinity);
                    mainModule.startLifetime = minMaxCurve;
                }
                if (p.speed)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float speed = p.curveSpeed.Evaluate(value);
                    minMaxCurve.constantMin = speed - p.randomRangeSpeed;
                    minMaxCurve.constantMax = speed;
                    mainModule.startSpeed = minMaxCurve;
                }
                if (p.size)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float size = p.curveSize.Evaluate(value);
                    minMaxCurve.constantMin = Mathf.Clamp(size - p.randomRangeSize, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(size, 0, Mathf.Infinity);
                    mainModule.startSize = minMaxCurve;
                }
                if (p.shapeRadius)
                {
                    var shape = p.particleSystem.shape;
                    shape.radius = p.curveShapeRadius.Evaluate(value);
                }
                if (p.rate)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float rate = p.curveRate.Evaluate(value);
                    minMaxCurve.constantMin = Mathf.Clamp(rate - p.randomRangeRate, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(rate, 0, Mathf.Infinity);
                    ParticleSystem.EmissionModule particleEmission = p.particleSystem.emission;
                    particleEmission.rateOverTime = minMaxCurve;
                }
                if (p.burst)
                {
                    particleBurst.time = 0;
                    short burst = (short)p.curveBurst.Evaluate(value);
                    particleBurst.minCount = (short)Mathf.Clamp(burst - p.randomRangeBurst, 0, Mathf.Infinity);
                    particleBurst.maxCount = (short)Mathf.Clamp(burst, 0, Mathf.Infinity);
                    p.particleSystem.emission.SetBurst(0, particleBurst);
                }
                if (p.lightIntensity)
                {
                    var lights = p.particleSystem.lights;
                    lights.intensityMultiplier = p.curveLightIntensity.Evaluate(value);
                }
                // Set material color
                bool updatePropertyBlock = false;
                if (p.linkBaseColor == ParticleInfo.LinkedGradient.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_BaseColor", currentMainGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }
                else if (p.linkBaseColor == ParticleInfo.LinkedGradient.Secondary && currentSecondaryGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_BaseColor", currentSecondaryGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }

                if (p.linkEmissionColor == ParticleInfo.LinkedGradient.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_EmissionColor", currentMainGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }
                else if (p.linkEmissionColor == ParticleInfo.LinkedGradient.Secondary && currentSecondaryGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_EmissionColor", currentSecondaryGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }
                if (updatePropertyBlock) p.renderer.SetPropertyBlock(p.materialPropertyBlock);
            }
        }

        public override void SetMainGradient(Gradient gradient)
        {
            currentMainGradient = gradient;
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                ParticleSystem.MainModule mainModule = p.particleSystem.main;
                if (p.linkStartColor == ParticleInfo.LinkedGradient.Main)
                {
                    ParticleSystem.MinMaxGradient minMaxGradient = mainModule.startColor;
                    minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
                    minMaxGradient.gradient = gradient;
                }
            }
            SetIntensity(currentValue);
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            currentSecondaryGradient = gradient;
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                ParticleSystem.MainModule mainModule = p.particleSystem.main;
                if (p.linkStartColor == ParticleInfo.LinkedGradient.Secondary)
                {
                    ParticleSystem.MinMaxGradient minMaxGradient = mainModule.startColor;
                    minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
                    minMaxGradient.gradient = gradient;
                }
            }
            SetIntensity(currentValue);
        }

        public override void SetMesh(Mesh mesh)
        {
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                if (p.mesh)
                {
                    ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                    shapeModule.mesh = mesh;
                }
            }
        }

        public override void SetCollider(Collider collider)
        {
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                if (p.mesh)
                {
                    ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                    if (collider is SphereCollider)
                    {
                        shapeModule.radius = (collider as SphereCollider).radius * collider.transform.lossyScale.magnitude;
                    }
                    else if (collider is CapsuleCollider)
                    {
                        float height = (collider as CapsuleCollider).height;
                        float radius = (collider as CapsuleCollider).radius;
                        if ((collider as CapsuleCollider).direction == 0) shapeModule.scale = new Vector3(height * collider.transform.lossyScale.x, radius * collider.transform.lossyScale.y, radius * collider.transform.lossyScale.z);
                        if ((collider as CapsuleCollider).direction == 1) shapeModule.scale = new Vector3(radius * collider.transform.lossyScale.x, height * collider.transform.lossyScale.y, radius * collider.transform.lossyScale.z);
                        if ((collider as CapsuleCollider).direction == 2) shapeModule.scale = new Vector3(radius * collider.transform.lossyScale.x, radius * collider.transform.lossyScale.y, height * collider.transform.lossyScale.z);
                    }
                    else if (collider is BoxCollider)
                    {
                        shapeModule.scale = new Vector3((collider as BoxCollider).size.x * collider.transform.lossyScale.x, (collider as BoxCollider).size.y * collider.transform.lossyScale.y, (collider as BoxCollider).size.z * collider.transform.lossyScale.z);
                    }
                }
            }
        }

        public override void Despawn()
        {
            foreach (ParticleInfo p in particles)
            {
                if (!p.particleSystem) continue;
                p.particleSystem.Stop();
            }
#if ProjectCore
            if (Application.isPlaying)
            {
                EffectInstance orgEffectInstance = effectInstance;
                effectInstance = null;
                EffectModuleParticle.Despawn(this);
                orgEffectInstance.OnEffectDespawn();
            }
#endif
        }
    }
}
