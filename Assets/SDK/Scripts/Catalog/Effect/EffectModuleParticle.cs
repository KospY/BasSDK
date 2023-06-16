using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectModuleParticle : EffectModule
    {
#if ODIN_INSPECTOR
        [BoxGroup("Particle")] 
#endif
        public EffectLink effectLink = EffectLink.Intensity;
#if ODIN_INSPECTOR
        [BoxGroup("Particle")]
#endif
        public float cullMinIntensity = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Particle")]
#endif
        public float cullMinSpeed = 0.05f;        
#if ODIN_INSPECTOR
        [BoxGroup("Particle")] 
#endif
        public string effectParticleAddress;
        [NonSerialized]
        public EffectParticle effectParticlePrefab;

#if ODIN_INSPECTOR
        [BoxGroup("Particle")] 
#endif
        public new AnimationCurve intensityCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Particle")] 
#endif
        public bool renderInLateUpdate;

#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized, GradientUsage(true)]
        private Gradient mainGradient;

        protected internal Gradient MainGradient
        {
            get
            {
                if (mainGradient != null)
                {
                    mainColorStart = mainGradient.colorKeys[0].color;
                    mainColorStart.a = mainGradient.alphaKeys[0].alpha;
                    mainColorEnd = mainGradient.colorKeys[mainGradient.colorKeys.Length - 1].color;
                    mainColorEnd.a = mainGradient.alphaKeys[mainGradient.alphaKeys.Length - 1].alpha;
                    return mainGradient;
                }

                if (mainColorStart != Color.clear || mainColorEnd != Color.clear)
                {
                    mainGradient = new Gradient();
                    GradientColorKey[] colorKeys = new GradientColorKey[2];
                    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                    colorKeys[0].color = mainColorStart;
                    colorKeys[0].time = 0;
                    alphaKeys[0].alpha = mainColorStart.a;
                    alphaKeys[0].time = 0;
                    colorKeys[1].color = mainColorEnd;
                    colorKeys[1].time = 1;
                    alphaKeys[1].alpha = mainColorEnd.a;
                    alphaKeys[1].time = 1;
                    mainGradient.SetKeys(colorKeys, alphaKeys);
                }
                return mainGradient;
            }
            set
            {
                mainGradient = value;
                if (value != null)
                {
                    mainColorStart = value.colorKeys[0].color;
                    mainColorStart.a = value.alphaKeys[0].alpha;
                    mainColorEnd = value.colorKeys[value.colorKeys.Length - 1].color;
                    mainColorEnd.a = value.alphaKeys[value.alphaKeys.Length - 1].alpha;
                }
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized, GradientUsage(true)]
        private Gradient secondaryGradient;

        protected internal Gradient SecondaryGradient
        {
            get
            {
                if (secondaryGradient != null)
                {
                    secondaryColorStart = secondaryGradient.colorKeys[0].color;
                    secondaryColorStart.a = secondaryGradient.alphaKeys[0].alpha;
                    secondaryColorEnd = secondaryGradient.colorKeys[secondaryGradient.colorKeys.Length - 1].color;
                    secondaryColorEnd.a = secondaryGradient.alphaKeys[secondaryGradient.alphaKeys.Length - 1].alpha;
                    return secondaryGradient;
                }

                if (secondaryColorStart != Color.clear || secondaryColorEnd != Color.clear)
                {
                    secondaryGradient = new Gradient();
                    GradientColorKey[] colorKeys = new GradientColorKey[2];
                    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                    colorKeys[0].color = secondaryColorStart;
                    colorKeys[0].time = 0;
                    alphaKeys[0].alpha = secondaryColorStart.a;
                    alphaKeys[0].time = 0;
                    colorKeys[1].color = secondaryColorEnd;
                    colorKeys[1].time = 1;
                    alphaKeys[1].alpha = secondaryColorEnd.a;
                    alphaKeys[1].time = 1;
                    secondaryGradient.SetKeys(colorKeys, alphaKeys);
                }
                return secondaryGradient;
            }
            set
            {
                secondaryGradient = value;
                if (value != null)
                {
                    secondaryColorStart = value.colorKeys[0].color;
                    secondaryColorStart.a = value.alphaKeys[0].alpha;
                    secondaryColorEnd = value.colorKeys[value.colorKeys.Length - 1].color;
                    secondaryColorEnd.a = value.alphaKeys[value.alphaKeys.Length - 1].alpha;
                }
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized]
        private Gradient mainGradientNoHdr;

        protected internal Gradient MainGradientNoHdr
        {
            get
            {
                if (mainGradientNoHdr != null)
                {
                    mainNoHdrColorStart = mainGradientNoHdr.colorKeys[0].color;
                    mainNoHdrColorStart.a = mainGradientNoHdr.alphaKeys[0].alpha;
                    mainNoHdrColorEnd = mainGradientNoHdr.colorKeys[mainGradientNoHdr.colorKeys.Length - 1].color;
                    mainNoHdrColorEnd.a = mainGradientNoHdr.alphaKeys[mainGradientNoHdr.alphaKeys.Length - 1].alpha;
                    return mainGradientNoHdr;
                }

                if (mainNoHdrColorStart != Color.clear || mainNoHdrColorEnd != Color.clear)
                {
                    mainGradientNoHdr = new Gradient();
                    GradientColorKey[] colorKeys = new GradientColorKey[2];
                    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                    colorKeys[0].color = mainNoHdrColorStart;
                    colorKeys[0].time = 0;
                    alphaKeys[0].alpha = mainNoHdrColorStart.a;
                    alphaKeys[0].time = 0;
                    colorKeys[1].color = mainNoHdrColorEnd;
                    colorKeys[1].time = 1;
                    alphaKeys[1].alpha = mainNoHdrColorEnd.a;
                    alphaKeys[1].time = 1;
                    mainGradientNoHdr.SetKeys(colorKeys, alphaKeys);
                }
                return mainGradientNoHdr;
            }
            set
            {
                mainGradientNoHdr = value;
                if (value != null)
                {
                    mainNoHdrColorStart = value.colorKeys[0].color;
                    mainNoHdrColorStart.a = value.alphaKeys[0].alpha;
                    mainNoHdrColorEnd = value.colorKeys[value.colorKeys.Length - 1].color;
                    mainNoHdrColorEnd.a = value.alphaKeys[value.alphaKeys.Length - 1].alpha;
                }
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized]
        private Gradient secondaryGradientNoHdr;

        protected internal Gradient SecondaryGradientNoHdr
        {
            get
            {
                if (secondaryGradientNoHdr != null)
                {
                    secondaryNoHdrColorStart = secondaryGradientNoHdr.colorKeys[0].color;
                    secondaryNoHdrColorStart.a = secondaryGradientNoHdr.alphaKeys[0].alpha;
                    secondaryNoHdrColorEnd = secondaryGradientNoHdr.colorKeys[secondaryGradientNoHdr.colorKeys.Length - 1].color;
                    secondaryNoHdrColorEnd.a = secondaryGradientNoHdr.alphaKeys[secondaryGradientNoHdr.alphaKeys.Length - 1].alpha;
                    return secondaryGradientNoHdr;
                }

                if (secondaryNoHdrColorStart != Color.clear || secondaryNoHdrColorEnd != Color.clear)
                {
                    secondaryGradientNoHdr = new Gradient();
                    GradientColorKey[] colorKeys = new GradientColorKey[2];
                    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                    colorKeys[0].color = secondaryNoHdrColorStart;
                    colorKeys[0].time = 0;
                    alphaKeys[0].alpha = secondaryNoHdrColorStart.a;
                    alphaKeys[0].time = 0;
                    colorKeys[1].color = secondaryNoHdrColorEnd;
                    colorKeys[1].time = 1;
                    alphaKeys[1].alpha = secondaryNoHdrColorEnd.a;
                    alphaKeys[1].time = 1;
                    secondaryGradientNoHdr.SetKeys(colorKeys, alphaKeys);
                }
                return secondaryGradientNoHdr;
            }
            set
            {
                secondaryGradientNoHdr = value;
                if (value != null)
                {
                    secondaryNoHdrColorStart = value.colorKeys[0].color;
                    secondaryNoHdrColorStart.a = value.alphaKeys[0].alpha;
                    secondaryNoHdrColorEnd = value.colorKeys[value.colorKeys.Length - 1].color;
                    secondaryNoHdrColorEnd.a = value.alphaKeys[value.alphaKeys.Length - 1].alpha;
                }
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("Color (HDR)")]
        [HorizontalGroup("Color (HDR)/MainColor")] 
#endif
        [ColorUsage(true, true), SerializeField]
        public Color mainColorStart;
#if ODIN_INSPECTOR
        [HorizontalGroup("Color (HDR)/MainColor")] 
#endif
        [ColorUsage(true, true), SerializeField]
        public Color mainColorEnd;
#if ODIN_INSPECTOR
        [HorizontalGroup("Color (HDR)/SecondaryColor")] 
#endif
        [ColorUsage(true, true), SerializeField]
        public Color secondaryColorStart;
#if ODIN_INSPECTOR
        [HorizontalGroup("Color (HDR)/SecondaryColor")] 
#endif
        [ColorUsage(true, true), SerializeField]
        public Color secondaryColorEnd;

#if ODIN_INSPECTOR
        [BoxGroup("Color (Non HDR)")]
        [HorizontalGroup("Color (Non HDR)/MainNoHdr")] 
#endif
        [SerializeField]
        public Color mainNoHdrColorStart;
#if ODIN_INSPECTOR
        [HorizontalGroup("Color (Non HDR)/MainNoHdr")] 
#endif
        [SerializeField]
        public Color mainNoHdrColorEnd;
#if ODIN_INSPECTOR
        [HorizontalGroup("Color (Non HDR)/SecondaryNoHdr")] 
#endif
        [SerializeField]
        public Color secondaryNoHdrColorStart;
#if ODIN_INSPECTOR
        [HorizontalGroup("Color (Non HDR)/SecondaryNoHdr")] 
#endif
        [SerializeField]
        public Color secondaryNoHdrColorEnd;

#if ODIN_INSPECTOR
        [BoxGroup("Scale")] 
#endif
        public Vector3 localScale = Vector3.one;
#if ODIN_INSPECTOR
        [BoxGroup("Scale")] 
#endif
        public bool useScaleCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Scale"), ShowIf("useScaleCurve")] 
#endif
        public AnimationCurve scaleCurve;

#if ODIN_INSPECTOR
        [BoxGroup("Rotation")] 
#endif
        public Vector3 localRotation = Vector3.zero;

#if ODIN_INSPECTOR
        [BoxGroup("Collision"), ValueDropdown("GetAllEffectID")] 
#endif
        public string collisionEffectId;
        [NonSerialized]
        public EffectData collisionEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public LayerMask collisionLayerMask = ~0;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public float collisionMaxGroundAngle = 45;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public float collisionEmitRate = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public float collisionMinIntensity = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public float collisionMaxIntensity = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public bool collisionUseMainGradient;
#if ODIN_INSPECTOR
        [BoxGroup("Collision")] 
#endif
        public bool collisionUseSecondaryGradient;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        } 
#endif


        public override void CopyHDRToNonHDR()
        {
            mainGradientNoHdr = mainGradient;
            secondaryGradientNoHdr = secondaryGradient;
        }

        public override void Clean()
        {
            if (intensityCurve != null && intensityCurve.keys.Length == 0) intensityCurve = null;
            if (scaleCurve != null && scaleCurve.keys.Length == 0) scaleCurve = null;
            if (mainGradient != null && mainGradient.alphaKeys.Length == 2 && mainGradient.colorKeys.Length == 2)
            {
                if (mainGradient.alphaKeys[0].alpha == 1 && mainGradient.alphaKeys[1].alpha == 1 && mainGradient.alphaKeys[0].time == 0 && mainGradient.alphaKeys[1].time == 1)
                {
                    if (mainGradient.colorKeys[0].color == Color.white && mainGradient.colorKeys[1].color == Color.white && mainGradient.colorKeys[0].time == 0 && mainGradient.colorKeys[1].time == 1)
                    {
                        mainGradient = null;
                    }
                }
            }
            if (secondaryGradient != null && secondaryGradient.alphaKeys.Length == 2 && secondaryGradient.colorKeys.Length == 2)
            {
                if (secondaryGradient.alphaKeys[0].alpha == 1 && secondaryGradient.alphaKeys[1].alpha == 1 && secondaryGradient.alphaKeys[0].time == 0 && secondaryGradient.alphaKeys[1].time == 1)
                {
                    if (secondaryGradient.colorKeys[0].color == Color.white && secondaryGradient.colorKeys[1].color == Color.white && secondaryGradient.colorKeys[0].time == 0 && secondaryGradient.colorKeys[1].time == 1)
                    {
                        secondaryGradient = null;
                    }
                }
            }
        }


        public static void Despawn(EffectParticle effectParticle)
        {
            GameObject.Destroy(effectParticle.gameObject);
        }



        /// <summary>
        /// Configures an effectParticle with values from this EffectModuleParticle
        /// </summary>
        /// <param name="effectParticle"></param>
        /// <returns></returns>
        public EffectParticle Configure(EffectParticle effectParticle)
        {
            if (step == Effect.Step.Custom) effectParticle.stepCustomHashId = stepCustomIdHash;

            effectParticle.transform.localScale = localScale;
            effectParticle.useScaleCurve = useScaleCurve;
            effectParticle.scaleCurve = scaleCurve;
            effectParticle.renderInLateUpdate = renderInLateUpdate;
            effectParticle.effectLink = effectLink;

            effectParticle.intensityCurve = intensityCurve ?? new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset srp = QualitySettings.renderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
            if (srp.supportsHDR)
            {
                effectParticle.SetMainGradient(mainGradient);
                effectParticle.SetSecondaryGradient(secondaryGradient);
            }
            else
            {
                effectParticle.SetMainGradient(mainGradientNoHdr);
                effectParticle.SetSecondaryGradient(secondaryGradientNoHdr);
            }

            return effectParticle;
        }
    }
}
