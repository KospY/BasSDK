using UnityEngine;
using System;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectVfx")]
    [ExecuteInEditMode]
    public class EffectVfx : Effect
    {
        public VisualEffect vfx;
        public float lifeTime = 5;
        public Transform sourceTransform;
        public Transform targetTransform;


        public SpawnTarget spawnOn;

        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        public bool useScaleCurve;
        public AnimationCurve scaleCurve;

        public bool lookAtTarget;

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
        protected bool hasSource;
        public static readonly ExposedProperty p_Seed = "Seed";
        public static readonly ExposedProperty p_Size = "Size";
        public static readonly ExposedProperty p_Intensity = "Intensity";
        public static readonly ExposedProperty p_Emitter_Size = "Emitter Size";
        public static readonly ExposedProperty p_MainGradient = "MainGradient";
        public static readonly ExposedProperty p_SecondaryGradient = "SecondaryGradient";
        public static readonly ExposedProperty p_PositionMap = "PositionMap";
        public static readonly ExposedProperty p_NormalMap = "NormalMap";
        public static readonly ExposedProperty p_Mesh = "Mesh";
        public static readonly ExposedProperty p_Source_position = "Source_position";
        public static readonly ExposedProperty p_Source_angles = "Source_angles";
        public static readonly ExposedProperty p_Source_scale = "Source_scale";
        public static readonly ExposedProperty p_Target_position = "Target_position";
        public static readonly ExposedProperty p_Target_angles = "Target_angles";
        public static readonly ExposedProperty p_Target_scale = "Target_scale";

        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            vfx = this.GetComponent<VisualEffect>();
            SetTarget(targetTransform);
        }

        private void Awake()
        {
            if (!this.TryGetComponent<VisualEffect>(out vfx))
            {
                vfx = this.gameObject.AddComponent<VisualEffect>();
            }
            vfx.enabled = false;
        }

        public override void Play()
        {
            vfx.enabled = true;
            if (vfx.HasInt(p_Seed))
            {
                vfx.SetInt(p_Seed, UnityEngine.Random.Range(0, 10000));
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

        public override void SetSize(float value)
        {
            if (vfx.HasFloat(p_Size)) vfx.SetFloat(p_Size, value);
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (vfx.HasFloat(p_Intensity)) vfx.SetFloat(p_Intensity, intensityCurve.Evaluate(value));
            }

            if (useScaleCurve)
            {
                float scale = scaleCurve.Evaluate(value);
                transform.localScale = new Vector3(scale, scale, scale);
            }

            if (emitSize)
            {
                vfx.SetFloat(p_Emitter_Size, curveEmitSize.Evaluate(intensityCurve.Evaluate(value)));
            }
        }

        public override void SetMainGradient(Gradient gradient)
        {
            if (gradient != null && vfx.HasGradient(p_MainGradient)) vfx.SetGradient(p_MainGradient, gradient);
            else vfx.ResetOverride(p_MainGradient);
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            if (gradient != null && vfx.HasGradient(p_SecondaryGradient)) vfx.SetGradient(p_SecondaryGradient, gradient);
            else vfx.ResetOverride(p_SecondaryGradient);
        }

        public override void SetMesh(Mesh mesh)
        {
            if (usePointCache)
            {
                if (mesh.isReadable)
                {
                    pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, pointCacheMapSize, pointCachePointCount, pointCacheSeed, pointCacheDistribution, pointCacheBakeMode);
                    if(vfx.HasTexture(p_PositionMap)) vfx.SetTexture(p_PositionMap, pCache.positionMap);
                    if (vfx.HasTexture(p_NormalMap)) vfx.SetTexture(p_NormalMap, pCache.normalMap);
                    if (!pointCacheSkinnedMeshUpdate)
                    {
                        pCache.Dispose();
                        pCache = null;
                    }
                }
                else
                {
                    Debug.LogError($"Cannot access vertices on mesh {mesh.name} for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
                }
            }
            else if (vfx.HasMesh(p_Mesh))
            {
                vfx.SetMesh(p_Mesh, mesh);
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
                        if (vfx.HasTexture(p_PositionMap)) vfx.SetTexture(p_PositionMap, pCache.positionMap);
                        if (vfx.HasTexture(p_NormalMap)) vfx.SetTexture(p_NormalMap, pCache.normalMap);
                        if (!pointCacheSkinnedMeshUpdate)
                        {
                            pCache.Dispose();
                            pCache = null;
                        }
                    }
                    else
                    {
                        Debug.LogError($"Cannot access vertices on mesh {mesh.name} for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
                    }
                }
                else
                {
                    vfx.SetMesh(p_Mesh, mesh);
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
            if (source && vfx.HasVector3(p_Source_position))
            {
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
            if (target && vfx.HasVector3(p_Target_position))
            {
                hasTarget = true;
                UpdateTarget();
            }
            else
            {
                hasTarget = false;
            }
        }

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update | ManagedLoops.LateUpdate;
        protected internal override void ManagedUpdate()
        {
            if (lookAtTarget)
            {
                this.transform.LookAt(2 * this.transform.position - targetTransform.position, Vector3.up);
            }
            UpdateSource();
            UpdateTarget();
            if (stopping && vfx.aliveParticleCount == 0)
            {
                stopping = false;
                Despawn();
            }
        }
        protected internal override void ManagedLateUpdate()
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
                if (vfx.HasVector3(p_Source_position)) vfx.SetVector3(p_Source_position, sourceTransform.position);
                if (vfx.HasVector3(p_Source_angles)) vfx.SetVector3(p_Source_angles, sourceTransform.eulerAngles);
                if (vfx.HasVector3(p_Source_scale)) vfx.SetVector3(p_Source_scale, sourceTransform.localScale);
                if (spawnOn == SpawnTarget.Source)
                {
                    vfx.transform.SetPositionAndRotation(sourceTransform.position, sourceTransform.rotation);
                }
            }
        }

        public void UpdateTarget()
        {
            if (hasTarget && targetTransform)
            {
                if (vfx.HasVector3(p_Target_position)) vfx.SetVector3(p_Target_position, targetTransform.position);
                if (vfx.HasVector3(p_Target_angles)) vfx.SetVector3(p_Target_angles, targetTransform.eulerAngles);
                if (vfx.HasVector3(p_Target_scale)) vfx.SetVector3(p_Target_scale, targetTransform.localScale);
                if (spawnOn == SpawnTarget.Target)
                {
                    vfx.transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
                }
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
            lookAtTarget = false;
            vfx.enabled = false;
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
