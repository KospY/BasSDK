using UnityEngine;
using System;
using UnityEngine.VFX;
using System.Collections.Generic;
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

    }
}
