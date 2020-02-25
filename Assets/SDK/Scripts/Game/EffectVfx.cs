using UnityEngine;
using System;
using UnityEngine.VFX;
using System.Collections.Generic;

namespace BS
{
    [ExecuteInEditMode]
    public class EffectVfx : Effect
    {
        public VisualEffect vfx;
        public float lifeTime = 5;
        public Transform targetTransform;

        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public bool useSecondaryRenderer;

        public bool usePointCache = false;
        public int pointCacheMapSize = 512;
        public int pointCachePointCount = 4096;
        public int pointCacheSeed = 0;
        public PointCacheGenerator.Distribution pointCacheDistribution = PointCacheGenerator.Distribution.RandomUniformArea;
        public PointCacheGenerator.MeshBakeMode pointCacheBakeMode = PointCacheGenerator.MeshBakeMode.Triangle;

        [NonSerialized]
        public float playTime;

        [Header("Intensity to Emitter Size")]
        public bool emitSize;
        public AnimationCurve curveEmitSize;

        protected bool stopping;
        protected bool hasTarget;
        protected int positionId;
        protected int positionAngles;
        protected int positionScale;

        private void OnValidate()
        {
            vfx = this.GetComponent<VisualEffect>();
            SetTarget(targetTransform);
        }

        private void Awake()
        {
            vfx = this.GetComponent<VisualEffect>();
            if (!vfx) vfx = this.gameObject.AddComponent<VisualEffect>();
            vfx.enabled = false;
        }

        public override void Play()
        {
            vfx.enabled = true;
            vfx.Play();
            if (step != Step.Loop)
            {
                Invoke("Despawn", lifeTime);
            }
            playTime = Time.time;
        }

        public override void Stop(bool loopOnly = false)
        {
            vfx.Stop();
            stopping = true;
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (vfx.HasFloat ("Intensity")
                && (!loopOnly || (loopOnly && step == Step.Loop)))
            {
                vfx.SetFloat("Intensity", intensityCurve.Evaluate(value));
            }

            if (emitSize)
            {
                vfx.SetFloat("Emitter Size", curveEmitSize.Evaluate(intensityCurve.Evaluate(value)));
            }
        }

        public override void SetMainGradient(Gradient gradient)
        {
            if (gradient != null && vfx.HasGradient("MainGradient")) vfx.SetGradient("MainGradient", gradient);
            else vfx.ResetOverride("MainGradient");
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            if (gradient != null && vfx.HasGradient("SecondaryGradient")) vfx.SetGradient("SecondaryGradient", gradient);
            else vfx.ResetOverride("SecondaryGradient");
        }

        public override void SetMesh(Mesh mesh)
        {
            if (usePointCache)
            {
                if (mesh.isReadable)
                {
                    PointCacheGenerator.PCache pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, pointCacheMapSize, pointCachePointCount, pointCacheSeed, pointCacheDistribution, pointCacheBakeMode);
                    vfx.SetTexture("PositionMap", pCache.positionMap);
                    if (vfx.HasTexture("NormalMap")) vfx.SetTexture("NormalMap", pCache.normalMap);
                }
                else
                {
                    Debug.LogError("Cannot access vertices on mesh " + mesh.name + " for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
                }
            }
            else
            {
                vfx.SetMesh("Mesh", mesh);
            }
        }

        public override void SetRenderer(Renderer renderer, bool secondary)
        {
            if ((useSecondaryRenderer && secondary) || (!useSecondaryRenderer && !secondary))
            {
                Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                if (usePointCache)
                {
                    if (mesh.isReadable)
                    {
                        PointCacheGenerator.PCache pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, pointCacheMapSize, pointCachePointCount, pointCacheSeed, pointCacheDistribution, pointCacheBakeMode);
                        vfx.SetTexture("PositionMap", pCache.positionMap);
                        if (vfx.HasTexture("NormalMap")) vfx.SetTexture("NormalMap", pCache.normalMap);
                    }
                    else
                    {
                        Debug.LogError("Cannot access vertices on mesh " + mesh.name + " for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
                    }
                }
                else
                {
                    vfx.SetMesh("Mesh", mesh);
                }
            }
        }

        public override void SetCollider(Collider collider)
        {
            // Computer point cache from collider
        }

        public override void SetTarget(Transform target)
        {
            targetTransform = target;
            if (target && vfx.HasVector3("Target_position"))
            {
                positionId = Shader.PropertyToID("Target_position");
                positionAngles = Shader.PropertyToID("Target_angles");
                positionScale = Shader.PropertyToID("Target_scale");
                hasTarget = true;
                Update();
            }
            else
            {
                hasTarget = false;
            }
        }

        public void Update()
        {
            if (hasTarget)
            {
                if (vfx.HasVector3(positionId)) vfx.SetVector3(positionId, targetTransform.position);
                if (vfx.HasVector3(positionAngles)) vfx.SetVector3(positionAngles, targetTransform.eulerAngles);
                if (vfx.HasVector3(positionScale)) vfx.SetVector3(positionScale, targetTransform.localScale);
            }
            if (stopping && vfx.aliveParticleCount == 0)
            {
                stopping = false;
                Despawn();
            }
        }

        public override void Despawn()
        {
            CancelInvoke();
            vfx.Stop();
            vfx.enabled = false;
            if (Application.isPlaying)
            {
                EffectModuleVfx.Despawn(this);
                InvokeDespawnCallback();
            }
        }
    }
}
