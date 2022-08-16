using UnityEngine;
using System;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectParticleChild")]
    public class EffectParticleChild : MonoBehaviour
    {
        [Header("Color Gradient")]
        public EffectTarget linkStartColor = EffectTarget.None;
        public EffectTarget linkStartGradient = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkTintColor = EffectTarget.None;

        public bool emitEffectOnCollision;

        public bool ignoreAlpha;

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

        [Header("Intensity to shape arc")]
        public bool shapeArc;
        public AnimationCurve curveShapeArc;

        [Header("Intensity to burst")]
        public bool burst;
        public AnimationCurve curveBurst;
        public short randomRangeBurst;

        [Header("Intensity to velocity over lifetime")]
        public bool velocityOverLifetime;
        public AnimationCurve curvevelocityOverLifetime;

        [Header("Intensity to light intensity")]
        public bool lightIntensity;
        public AnimationCurve curveLightIntensity;

        [Header("Mesh")]
        public bool mesh;

        [Header("Renderer")]
        public EffectTarget useRenderer = EffectTarget.None;

        [Header("Collider")]
        public new bool collider;
  
        [NonSerialized]
        public ParticleSystemRenderer particleRenderer;
        [NonSerialized]
        public MaterialPropertyBlock materialPropertyBlock;
        [NonSerialized]
        public new ParticleSystem particleSystem;

    }
}
