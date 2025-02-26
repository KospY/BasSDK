using UnityEngine;
using System;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectVfx.html")]
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

        public bool despawnOnEnd = false;
        public float despawnDelay = 0;
        public bool usePointCache = false;
        public bool pointCacheSkinnedMeshUpdate = false;
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
        public static readonly ExposedProperty p_SkinnedMeshRenderer = "SkinnedMeshRenderer";
        public static readonly ExposedProperty p_Source_position = "Source_position";
        public static readonly ExposedProperty p_Source_angles = "Source_angles";
        public static readonly ExposedProperty p_Source_scale = "Source_scale";
        public static readonly ExposedProperty p_Target_position = "Target_position";
        public static readonly ExposedProperty p_Target_angles = "Target_angles";
        public static readonly ExposedProperty p_Target_scale = "Target_scale";

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif
            if (!gameObject.activeInHierarchy) return;
            vfx = this.GetComponent<VisualEffect>();
            SetTarget(targetTransform);
        }


        public override void Play()
        {
        }

        public override void Stop()
        {
  
        }

        public override void End(bool loopOnly = false)
        {
        }

        public override void SetSize(float value)
        {
            base.SetSize(value);
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            base.SetIntensity(value, loopOnly);

        }

        public override void SetMainGradient(Gradient gradient)
        {
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
        }

        public override void SetMesh(Mesh mesh)
        {
        }

        public override void SetRenderer(Renderer renderer, bool secondary)
        {
        }

        public override void SetCollider(Collider collider)
        {
            // Computer point cache from collider
        }

        public override void SetSource(Transform source)
        {
        }

        public override void SetTarget(Transform target)
        {
        }

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update | ManagedLoops.LateUpdate;
        protected internal override void ManagedUpdate()
        {
        }
        protected internal override void ManagedLateUpdate()
        {
        }

        public void UpdateSource()
        {
        }

        public void UpdateTarget()
        {
        }

        public override void Despawn()
        {
        }

        public void SetProperty<T>(string key, T value)
        {
        }

        public bool TryGetProperty<T>(string key, out T value) where T : class
        {
key = string.Empty;
value = default;
            return false;
        }
    }
}
