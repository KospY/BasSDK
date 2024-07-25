using UnityEngine;
using System;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectDecal.html")]
    [ExecuteInEditMode]
    public class EffectDecal : Effect
    {
        [NonSerialized]
        public MeshRenderer meshRenderer;

        protected static int colorPropertyID;
        protected static int emissionPropertyID;
        protected static int intensityPropertyID;
        protected static Mesh defaultCubeMesh;

        protected MaterialPropertyBlock materialPropertyBlock;

        public float baseLifeTime = 60;
        public float emissionLifeTime = 10;
        public float fadeRefreshSpeed = 0.1f;

        [NonSerialized]
        public float playTime;

        [Header("Size")]
        public Vector3 size = Vector3.one;
        public float sizeRandomRange = 0;
        public bool useSizeCurve;
        public AnimationCurve sizeCurve;

        [Header("Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;
        [GradientUsage(true)]
        public Gradient baseColorGradient;
        [GradientUsage(true)]
        public Gradient emissionColorGradient;

    }
}
