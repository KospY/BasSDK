using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectParticle.html")]
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

        // private List<EffectParticleChild> movedChildren = new List<EffectParticleChild>();

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
        private Coroutine despawnCoroutine;
        [FormerlySerializedAs("customStepIsLoop")]
        public bool loopCustomStep;

    }
}
