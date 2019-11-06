using UnityEngine;
using System;
using System.Collections.Generic;

namespace BS
{
    public class EffectParticle : Effect
    {
        public int poolCount = 50;
        public float lifeTime = 5;

        [NonSerialized]
        public List<EffectParticleChild> childs = new List<EffectParticleChild>();

        [NonSerialized]
        public ParticleSystem rootParticleSystem;

        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();
        protected ParticleSystem.Burst particleBurst = new ParticleSystem.Burst();

        [NonSerialized]
        public float currentValue;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;


        private void OnValidate()
        {
            Init();
        }
#if ProjectCore
        private void Awake()
        {
            Init();
            foreach (EffectParticleChild p in childs)
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
            childs = new List<EffectParticleChild>(this.GetComponentsInChildren<EffectParticleChild>());
            foreach (EffectParticleChild child in childs)
            {
                child.particleSystem = child.GetComponent<ParticleSystem>();
                child.renderer = child.particleSystem.GetComponent<ParticleSystemRenderer>();
                child.materialPropertyBlock = new MaterialPropertyBlock();
            }
        }

        public override void Play()
        {
            rootParticleSystem.Play();
            if (step != Step.Loop)
            {
                Invoke("Despawn", lifeTime);
            }
        }

        public override void Stop()
        {
            rootParticleSystem.Stop();
            Invoke("Despawn", lifeTime);
        }

        public override void SetIntensity(float value)
        {
            currentValue = value;

            foreach (EffectParticleChild p in childs)
            {
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
                if (p.velocityOverLifetime)
                {
                    float rate = p.curvevelocityOverLifetime.Evaluate(value);
                    minMaxCurve.constantMin = Mathf.Clamp(rate, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(rate, 0, Mathf.Infinity);
                    ParticleSystem.VelocityOverLifetimeModule velocityModule = p.particleSystem.velocityOverLifetime;
                    velocityModule.speedModifier = minMaxCurve;
                }
                if (p.lightIntensity)
                {
                    var lights = p.particleSystem.lights;
                    lights.intensityMultiplier = p.curveLightIntensity.Evaluate(value);
                }

                // Set start color gradient
                ParticleSystem.MinMaxGradient minMaxGradient = mainModule.startColor;
                if (p.linkStartGradient == EffectParticleChild.LinkedGradient.Main && currentMainGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
                    minMaxGradient.gradient = currentMainGradient;
                }

                if (p.linkStartGradient == EffectParticleChild.LinkedGradient.Secondary && currentSecondaryGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
                    minMaxGradient.gradient = currentSecondaryGradient;
                }

                // Set start color
                if (p.linkStartColor == EffectParticleChild.LinkedGradient.Main && currentMainGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Color;
                    minMaxGradient.color = currentMainGradient.Evaluate(value);
                }
                if (p.linkStartColor == EffectParticleChild.LinkedGradient.Secondary && currentSecondaryGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Color;
                    minMaxGradient.color = currentSecondaryGradient.Evaluate(value);
                }
                mainModule.startColor = minMaxGradient;

                // Set material color
                bool updatePropertyBlock = false;
                if (p.linkBaseColor == EffectParticleChild.LinkedGradient.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_BaseColor", currentMainGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }
                else if (p.linkBaseColor == EffectParticleChild.LinkedGradient.Secondary && currentSecondaryGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_BaseColor", currentSecondaryGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }

                if (p.linkEmissionColor == EffectParticleChild.LinkedGradient.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor("_EmissionColor", currentMainGradient.Evaluate(value));
                    updatePropertyBlock = true;
                }
                else if (p.linkEmissionColor == EffectParticleChild.LinkedGradient.Secondary && currentSecondaryGradient != null)
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
            SetIntensity(currentValue);
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            currentSecondaryGradient = gradient;
            SetIntensity(currentValue);
        }

        public override void SetMesh(Mesh mesh)
        {
            foreach (EffectParticleChild p in childs)
            {
                if (p.mesh)
                {
                    ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                    shapeModule.mesh = mesh;
                }
            }
        }

        public override void SetCollider(Collider collider)
        {
            foreach (EffectParticleChild p in childs)
            {
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
            rootParticleSystem.Stop();
            CancelInvoke();
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
