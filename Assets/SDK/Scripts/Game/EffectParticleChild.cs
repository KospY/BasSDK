using UnityEngine;
using System;

namespace BS
{
    public class EffectParticleChild : MonoBehaviour
    {
        [Header("Color Gradient")]
        public LinkedGradient linkStartColor = LinkedGradient.None;
        public LinkedGradient linkStartGradient = LinkedGradient.None;
        public LinkedGradient linkEmissionColor = LinkedGradient.None;
        public LinkedGradient linkBaseColor = LinkedGradient.None;

        public enum LinkedGradient
        {
            None,
            Main,
            Secondary,
        }

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

        [Header("Intensity to speed")]
        public bool burst;
        public AnimationCurve curveBurst;
        public short randomRangeBurst;

        [Header("Intensity to light intensity")]
        public bool lightIntensity;
        public AnimationCurve curveLightIntensity;

        [Header("Mesh")]
        public bool mesh;

        [Header("Collider")]
        public new bool collider;

        [Header("Spawn on collision")]
        public string spawnEffectId;
        public LayerMask spawnLayerMask = ~0;
        public float spawnMaxGroundAngle = 45;
        public float spawnEmitRate = 0.1f;
        public float spawnMinIntensity;
        public float spawnMaxIntensity;
        public bool useMainGradient;
        public bool useSecondaryGradient;

        [NonSerialized]
        public new ParticleSystemRenderer renderer;
        [NonSerialized]
        public MaterialPropertyBlock materialPropertyBlock;
        [NonSerialized]
        public new ParticleSystem particleSystem;
    }
}
