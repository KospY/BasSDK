using UnityEngine;
using System;
using System.Collections;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectMesh")]
    public class EffectMesh : Effect
    {

        public int poolCount = 20;
        public float lifeTime = 5;
        public float refreshSpeed = 0.1f;

        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        [NonSerialized]
        public float playTime;

        private float aliveTime = 0.0f;

        [Header("Color Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkTintColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;

        public Vector3 meshSize = Vector3.one;
        public float meshSizeFadeDuration;
        protected bool meshSizeFading;
        protected Coroutine meshSizeFadeCoroutine;

        public Vector3 meshRotation;
        public float meshRotationFadeDuration;
        protected bool meshRotationFading;
        protected Coroutine meshRotationFadeCoroutine;

        [Header("Intensity to mesh size")]
        public bool meshSizeFromIntensity;
        public AnimationCurve curveMeshSize;

        [Header("Intensity to mesh rotation Y")]
        public bool meshRotationFromIntensity;
        public AnimationCurve curveMeshrotY;

        [NonSerialized]
        public float currentValue;

        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;

        [NonSerialized]
        public MaterialPropertyBlock materialPropertyBlock;
        [NonSerialized]
        public MeshFilter meshFilter;
        [NonSerialized]
        public MeshRenderer meshRenderer;

    }
}
