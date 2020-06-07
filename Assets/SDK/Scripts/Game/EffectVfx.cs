using UnityEngine;
using System;
using UnityEngine.VFX;
using System.Collections.Generic;

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class EffectVfx : Effect
    {
        public VisualEffect vfx;
        public float lifeTime = 5;
        public Transform sourceTransform;
        public Transform targetTransform;

        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        public bool useScaleCurve;
        public AnimationCurve scaleCurve;

        public bool useSecondaryRenderer;

        public bool usePointCache = false;
        public bool pointCacheSkinnedMeshUpdate = false;
        public int pointCacheMapSize = 512;
        public int pointCachePointCount = 4096;
        public int pointCacheSeed = 0;
        public PointCacheGenerator.Distribution pointCacheDistribution = PointCacheGenerator.Distribution.RandomUniformArea;
        public PointCacheGenerator.MeshBakeMode pointCacheBakeMode = PointCacheGenerator.MeshBakeMode.Triangle;

        protected PointCacheGenerator.PCache pCache;
        protected SkinnedMeshRenderer pointCacheSkinnedMeshRenderer;

        [NonSerialized]
        public float playTime;

        [Header("Intensity to Emitter Size")]
        public bool emitSize;
        public AnimationCurve curveEmitSize;


        protected bool stopping;

        protected bool hasTarget;
        protected int tgtPositionId;
        protected int tgtPositionAngles;
        protected int tgtPositionScale;

        protected bool hasSource;
        protected int srcPositionId;
        protected int srcPositionAngles;
        protected int srcPositionScale;

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
            if (vfx.HasInt("Seed"))
            {
                vfx.SetInt("Seed", UnityEngine.Random.Range(0, 10000));
            }
            vfx.Play();
            if (step == Step.Start || step == Step.End)
            {
                Invoke("Despawn", lifeTime);
            }
            playTime = Time.time;
        }

        public override void Stop()
        {
            vfx.Stop();
        }

        public override void End(bool loopOnly = false)
        {
            vfx.Stop();
            stopping = true;
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (vfx.HasFloat("Intensity")) vfx.SetFloat("Intensity", intensityCurve.Evaluate(value));
            }

            if (useScaleCurve)
            {
                float scale = scaleCurve.Evaluate(value);
                transform.localScale = new Vector3(scale, scale, scale);
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
                    pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, pointCacheMapSize, pointCachePointCount, pointCacheSeed, pointCacheDistribution, pointCacheBakeMode);
                    vfx.SetTexture("PositionMap", pCache.positionMap);
                    if (vfx.HasTexture("NormalMap")) vfx.SetTexture("NormalMap", pCache.normalMap);
                }
                else
                {
                    Debug.LogError("Cannot access vertices on mesh " + mesh.name + " for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
                }
            }
            else if (vfx.HasMesh("Mesh"))
            {
                vfx.SetMesh("Mesh", mesh);
            }
        }

        public override void SetRenderer(Renderer renderer, bool secondary)
        {
            if ((useSecondaryRenderer && secondary) || (!useSecondaryRenderer && !secondary))
            {
                Mesh mesh = renderer is SkinnedMeshRenderer ? (renderer as SkinnedMeshRenderer).sharedMesh : renderer.GetComponent<MeshFilter>().sharedMesh;
                if (usePointCache)
                {
                    if (mesh.isReadable)
                    {
                        if (renderer is SkinnedMeshRenderer)
                        {
                            mesh = new Mesh();
                            (renderer as SkinnedMeshRenderer).BakeMesh(mesh);
                            pointCacheSkinnedMeshRenderer = renderer as SkinnedMeshRenderer;
                        }
                        pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, pointCacheMapSize, pointCachePointCount, pointCacheSeed, pointCacheDistribution, pointCacheBakeMode);
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

        public override void SetSource(Transform source)
        {
            sourceTransform = source;
            if (source && vfx.HasVector3("Source_position"))
            {
                srcPositionId = Shader.PropertyToID("Source_position");
                srcPositionAngles = Shader.PropertyToID("Source_angles");
                srcPositionScale = Shader.PropertyToID("Source_scale");
                hasSource = true;
                UpdateSource();
            }
            else
            {
                hasSource = false;
            }
        }

        public override void SetTarget(Transform target)
        {
            targetTransform = target;
            if (target && vfx.HasVector3("Target_position"))
            {
                tgtPositionId = Shader.PropertyToID("Target_position");
                tgtPositionAngles = Shader.PropertyToID("Target_angles");
                tgtPositionScale = Shader.PropertyToID("Target_scale");
                hasTarget = true;
                UpdateTarget();
            }
            else
            {
                hasTarget = false;
            }
        }

        public void Update()
        {
            UpdateSource();
            UpdateTarget();
            if (stopping && vfx.aliveParticleCount == 0)
            {
                stopping = false;
                Despawn();
            }
        }

        public void LateUpdate()
        {
            if (pointCacheSkinnedMeshUpdate)
            {
                pCache.Update(pointCacheSkinnedMeshRenderer);
            }
        }

        public void UpdateSource()
        {
            if (hasSource && sourceTransform)
            {
                if (vfx.HasVector3(srcPositionId)) vfx.SetVector3(srcPositionId, sourceTransform.position);
                if (vfx.HasVector3(srcPositionAngles)) vfx.SetVector3(srcPositionAngles, sourceTransform.eulerAngles);
                if (vfx.HasVector3(srcPositionScale)) vfx.SetVector3(srcPositionScale, sourceTransform.localScale);
            }
        }

        public void UpdateTarget()
        {
            if (hasTarget && targetTransform)
            {
                if (vfx.HasVector3(tgtPositionId)) vfx.SetVector3(tgtPositionId, targetTransform.position);
                if (vfx.HasVector3(tgtPositionAngles)) vfx.SetVector3(tgtPositionAngles, targetTransform.eulerAngles);
                if (vfx.HasVector3(tgtPositionScale)) vfx.SetVector3(tgtPositionScale, targetTransform.localScale);
            }
        }

        public override void Despawn()
        {
            if (pCache != null)
            {
                pCache.Dispose();
                pCache = null;
            }
            pointCacheSkinnedMeshRenderer = null;
            CancelInvoke();
            vfx.Stop();
            vfx.enabled = false;
#if ProjectCore
            if (Application.isPlaying)
            {
                EffectModuleVfx.Despawn(this);
                InvokeDespawnCallback();
            }
#endif
        }
    }
}
