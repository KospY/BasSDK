using UnityEngine;
using System;
using System.Collections.Generic;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectParticle")]
    public class EffectParticle : Effect
    {

        public int poolCount = 50;
        public float lifeTime = 5;

        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        [NonSerialized]
        public float playTime;

        public EffectLink effectLink = EffectLink.Intensity;

        public bool renderInLateUpdate;
        public bool useScaleCurve;
        public AnimationCurve scaleCurve;

        [NonSerialized]
        public List<EffectParticleChild> childs = new List<EffectParticleChild>();

        [NonSerialized]
        public ParticleSystem rootParticleSystem;

        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();

        [NonSerialized]
        public float currentValue;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;

        protected MaterialPropertyBlock materialPropertyBlock;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Init();
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
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
                child.particleRenderer = child.particleSystem.GetComponent<ParticleSystemRenderer>();
                child.materialPropertyBlock = new MaterialPropertyBlock();
            }
        }

        public override void Play()
        {
            CancelInvoke();
            foreach (EffectParticleChild p in childs)
            {
                if (p == null)
                {
                    Debug.LogError(this.name + " have an EffectParticleChild that has been destroyed! (this could happen if a effectParticle component has been added to one of the childs)");
                    continue;
                }

                ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                if (shapeModule.shapeType == ParticleSystemShapeType.SkinnedMeshRenderer && shapeModule.skinnedMeshRenderer == null)
                {
                    shapeModule.enabled = false;
                }
                else if (shapeModule.shapeType == ParticleSystemShapeType.MeshRenderer && shapeModule.meshRenderer == null)
                {
                    shapeModule.enabled = false;
                }
                else if (shapeModule.shapeType == ParticleSystemShapeType.Mesh && shapeModule.mesh == null)
                {
                    shapeModule.enabled = false;
                }
#if PrivateSDK
                // Set light volume if in dungeon
                if (Level.current?.dungeon)
                {
                    LightVolumeReceiver.ApplyProbeVolume(p.particleRenderer, materialPropertyBlock);
                }
                else
                {
                    LightVolumeReceiver.DisableProbeVolume(p.particleRenderer);
                }
#endif
            }
            rootParticleSystem.Play(true);
            if (step == Step.Start || step == Step.End)
            {
                Invoke("Despawn", lifeTime);
            }
            playTime = Time.time;
        }

        public override void Stop()
        {
            rootParticleSystem.Stop();
        }

        public override void End(bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                CancelInvoke();
                rootParticleSystem.Stop();
                Invoke("Despawn", lifeTime);
            }
        }
        protected override ManagedLoops ManagedLoops => ManagedLoops.LateUpdate;

        protected internal override void ManagedLateUpdate()
        {
            if (renderInLateUpdate && playTime > 0)
            {
                for (int i = 0; i < childs.Count; i++)
                {
                    EffectParticleChild p = childs[i];
                    p.particleSystem.Simulate(Time.deltaTime, true, false, false);
                }
            }
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (effectLink == EffectLink.Intensity)
                {
                    SetVariation(value, loopOnly);
                }
            }
        }

        public override void SetSpeed(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (effectLink == EffectLink.Speed)
                {
                    SetVariation(value, loopOnly);
                }
            }
        }

        public void SetVariation(float value, bool loopOnly = false)
        {
            currentValue = intensityCurve.Evaluate(value);

            if (useScaleCurve)
            {
                float scale = scaleCurve.Evaluate(value);
                transform.localScale = new Vector3(scale, scale, scale);
            }

            int childsCount = childs.Count;
            for (int i = 0; i < childsCount; i++)
            {
                EffectParticleChild p = childs[i];
                ParticleSystem.MainModule mainModule = p.particleSystem.main;

                if (p == null)
                {
                    Debug.LogError($"{this.name} have an EffectParticleChild that has been destroyed! (this could happen if a effectParticle component has been added to one of the childs)");
                    continue;
                }

                if (p.duration && !p.particleSystem.isPlaying)
                {
                    mainModule.duration = p.curveDuration.Evaluate(currentValue);
                }
                if (p.lifeTime)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float lifeTime = p.curveLifeTime.Evaluate(currentValue);
                    minMaxCurve.constantMin = Mathf.Clamp(lifeTime - p.randomRangeLifeTime, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(lifeTime, 0, Mathf.Infinity);
                    mainModule.startLifetime = minMaxCurve;
                }
                if (p.speed)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float speed = p.curveSpeed.Evaluate(currentValue);
                    minMaxCurve.constantMin = minMaxCurve.constantMin = Mathf.Clamp(speed - p.randomRangeSpeed, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = speed;
                    mainModule.startSpeed = minMaxCurve;
                }
                if (p.size)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float size = p.curveSize.Evaluate(currentValue);
                    minMaxCurve.constantMin = Mathf.Clamp(size - p.randomRangeSize, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(size, 0, Mathf.Infinity);
                    mainModule.startSize = minMaxCurve;
                }
                if (p.shapeRadius)
                {
                    var shape = p.particleSystem.shape;
                    shape.radius = p.curveShapeRadius.Evaluate(currentValue);
                }
                if (p.shapeArc)
                {
                    var shape = p.particleSystem.shape;
                    shape.arc = p.curveShapeArc.Evaluate(currentValue);
                }
                if (p.rate)
                {
                    minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
                    float rate = p.curveRate.Evaluate(currentValue);
                    minMaxCurve.constantMin = Mathf.Clamp(rate - p.randomRangeRate, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(rate, 0, Mathf.Infinity);
                    ParticleSystem.EmissionModule particleEmission = p.particleSystem.emission;
                    particleEmission.rateOverTime = minMaxCurve;
                }
                if (p.burst)
                {
                    short burst = (short)p.curveBurst.Evaluate(value);
                    ParticleSystem.Burst particleBurst = new ParticleSystem.Burst(0, (short)Mathf.Clamp(burst - p.randomRangeBurst, 0, Mathf.Infinity), (short)Mathf.Clamp(burst, 0, Mathf.Infinity));
                    ParticleSystem.EmissionModule particleEmission = p.particleSystem.emission;
                    particleEmission.SetBurst(0, particleBurst);
                }
                if (p.velocityOverLifetime)
                {
                    float rate = p.curvevelocityOverLifetime.Evaluate(currentValue);
                    minMaxCurve.constantMin = Mathf.Clamp(rate, 0, Mathf.Infinity);
                    minMaxCurve.constantMax = Mathf.Clamp(rate, 0, Mathf.Infinity);
                    ParticleSystem.VelocityOverLifetimeModule velocityModule = p.particleSystem.velocityOverLifetime;
                    velocityModule.speedModifier = minMaxCurve;
                }
                if (p.lightIntensity)
                {
                    var lights = p.particleSystem.lights;
                    lights.intensityMultiplier = p.curveLightIntensity.Evaluate(currentValue);
                }

                // Set start color gradient
                ParticleSystem.MinMaxGradient minMaxGradient = mainModule.startColor;
                if (p.linkStartGradient == EffectTarget.Main && currentMainGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
                    minMaxGradient.gradient = currentMainGradient;
                }

                if (p.linkStartGradient == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Gradient;
                    minMaxGradient.gradient = currentSecondaryGradient;
                }

                // Set start color
                if (p.linkStartColor == EffectTarget.Main && currentMainGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Color;
                    if (p.ignoreAlpha)
                    {
                        Color newColor = currentMainGradient.Evaluate(currentValue);
                        minMaxGradient.color = new Color(newColor.r, newColor.g, newColor.b, minMaxGradient.color.a);
                    }
                    else
                    {
                        minMaxGradient.color = currentMainGradient.Evaluate(currentValue);
                    }
                }
                if (p.linkStartColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    minMaxGradient.mode = ParticleSystemGradientMode.Color;
                    if (p.ignoreAlpha)
                    {
                        Color newColor = currentSecondaryGradient.Evaluate(currentValue);
                        minMaxGradient.color = new Color(newColor.r, newColor.g, newColor.b, minMaxGradient.color.a);
                    }
                    else
                    {
                        minMaxGradient.color = currentSecondaryGradient.Evaluate(currentValue);
                    }
                }
                mainModule.startColor = minMaxGradient;

                // Set material color
                bool updatePropertyBlock = false;
                if (p.linkBaseColor == EffectTarget.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor(BaseColor, currentMainGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (p.linkBaseColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    p.materialPropertyBlock.SetColor(BaseColor, currentSecondaryGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }

                if (p.linkTintColor == EffectTarget.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor(TintColor, currentMainGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (p.linkTintColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    p.materialPropertyBlock.SetColor(TintColor, currentSecondaryGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }

                if (p.linkEmissionColor == EffectTarget.Main && currentMainGradient != null)
                {
                    p.materialPropertyBlock.SetColor(EmissionColor, currentMainGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (p.linkEmissionColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    p.materialPropertyBlock.SetColor(EmissionColor, currentSecondaryGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                if (updatePropertyBlock) p.particleRenderer.SetPropertyBlock(p.materialPropertyBlock);
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
            if (mesh == null) return;
            foreach (EffectParticleChild p in childs)
            {
                if (p.mesh)
                {
                    ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                    shapeModule.shapeType = ParticleSystemShapeType.Mesh;
                    shapeModule.mesh = mesh;
                    shapeModule.enabled = true;
                }
            }
        }

        public override void SetRenderer(Renderer renderer, bool secondary)
        {
            if (renderer == null) return;
            foreach (EffectParticleChild p in childs)
            {
                ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                if ((p.useRenderer == EffectTarget.Main && !secondary) || (p.useRenderer == EffectTarget.Secondary && secondary))
                {
                    if (renderer is MeshRenderer)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.MeshRenderer;
                        shapeModule.meshRenderer = renderer as MeshRenderer;
                        shapeModule.enabled = true;
                    }
                    if (renderer is SkinnedMeshRenderer)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
                        shapeModule.skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
                        shapeModule.enabled = true;
                    }
                }
            }
        }

        public override void SetCollider(Collider collider)
        {
            foreach (EffectParticleChild p in childs)
            {
                if (p.collider)
                {
                    ParticleSystem.MainModule mainModule = p.particleSystem.main;
                    mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;
                    ParticleSystem.ShapeModule shapeModule = p.particleSystem.shape;
                    if (collider is SphereCollider)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
                        shapeModule.radius = (collider as SphereCollider).radius * collider.transform.lossyScale.magnitude;
                        shapeModule.position = p.transform.InverseTransformPoint(collider.transform.TransformPoint((collider as SphereCollider).center));
                    }
                    else if (collider is CapsuleCollider)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.Box;
                        float height = (collider as CapsuleCollider).height;
                        float radius = (collider as CapsuleCollider).radius;
                        if ((collider as CapsuleCollider).direction == 0) shapeModule.scale = new Vector3(height * collider.transform.lossyScale.x, radius * Mathf.Max(collider.transform.lossyScale.y, collider.transform.lossyScale.z), radius * Mathf.Max(collider.transform.lossyScale.y, collider.transform.lossyScale.z));
                        if ((collider as CapsuleCollider).direction == 1) shapeModule.scale = new Vector3(radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.z), height * collider.transform.lossyScale.y, radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.z));
                        if ((collider as CapsuleCollider).direction == 2) shapeModule.scale = new Vector3(radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.y), radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.y), height * collider.transform.lossyScale.z);
                        shapeModule.position = p.transform.InverseTransformPoint(collider.transform.TransformPoint((collider as CapsuleCollider).center));
                    }
                    else if (collider is BoxCollider)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.Box;
                        shapeModule.scale = new Vector3((collider as BoxCollider).size.x * collider.transform.lossyScale.x, (collider as BoxCollider).size.y * collider.transform.lossyScale.y, (collider as BoxCollider).size.z * collider.transform.lossyScale.z);
                        shapeModule.position = p.transform.InverseTransformPoint(collider.transform.TransformPoint((collider as BoxCollider).center));
                        shapeModule.scale = (collider as BoxCollider).size;
                    }
                    shapeModule.rotation = (Quaternion.Inverse(p.transform.rotation) * collider.transform.rotation).eulerAngles;
                }
            }
        }

        public override void Despawn()
        {
            playTime = 0;
            rootParticleSystem?.Stop();
            CancelInvoke();
            InvokeDespawnCallback();
            if (Application.isPlaying)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }
}
