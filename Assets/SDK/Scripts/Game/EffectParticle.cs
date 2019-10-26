using UnityEngine;
using System;
using System.Collections.Generic;

namespace BS
{
    public class EffectParticle : Effect
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
            [Header("Spawn on collision")]
            public string spawnEffectId;
            public LayerMask spawnLayerMask = ~0;
            public float spawnMaxGroundAngle = 45;
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
            }
        }

        public override void SetColor(Color mainColor)
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

        public override void SetColor(Color mainColor, Color secondaryColor)
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
                    p.particleSystem.GetComponent<Renderer>().sharedMaterial.SetColor("_BaseColor", finalColor);
            }
        }

        public override void SetTarget(Transform transform)
        {
            foreach (ParticleInfo p in particles)
            {

            }
        }

        public override void SetMesh(Mesh mesh)
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

        public override void Despawn()
        {
#if ProjectCore
            if (Application.isPlaying)
            {
                foreach (ParticleInfo p in particles)
                {
                    p.particleSystem.Stop();
                }
                EffectInstance orgEffectInstance = effectInstance;
                effectInstance = null;
                //EffectModuleParticle.Despawn(this);
                orgEffectInstance.OnEffectDespawn();
            }
#endif
        }
    }
}
