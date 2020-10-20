using UnityEngine;
using System;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class EffectPaint : Effect
    {
        protected static int baseMapPropertyID;
        protected static int normalPropertyID;
        protected static int emissionPropertyID;

        public Material material;
        public float radius = 0.1f;
        public bool useRadiusCurve;
        public AnimationCurve radiusCurve;
        public float depth = 1;
        public float opacity = 1;
        public float hardness = 1;
        public float wrapping = 1;
        public float normalFront = 1f;
        public float tileTransition = 4f;
        public float normalBack = 0;
        public float normalFade = 0.5f;

        [NonSerialized]
        public float playTime;

        [Header("Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;
        [GradientUsage(true)]
        public Gradient baseColorGradient;
        [GradientUsage(true)]
        public Gradient emissionColorGradient;


        public override void SetMainGradient(Gradient gradient)
        {
            if (linkBaseColor == EffectTarget.Main)
            {
                baseColorGradient = gradient;
            }
            if (linkEmissionColor == EffectTarget.Main)
            {
                emissionColorGradient = gradient;
            }
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            if (linkBaseColor == EffectTarget.Secondary)
            {
                baseColorGradient = gradient;
            }
            if (linkEmissionColor == EffectTarget.Secondary)
            {
                emissionColorGradient = gradient;
            }
        }

        public override void CollisionStay(Vector3 position, Quaternion rotation, float intensity)
        {
            // Prevent decal to move when rubbing
        }

        public override void Despawn()
        {
            CancelInvoke();
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
