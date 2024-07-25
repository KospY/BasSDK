using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
        public float insideParticleRadius = 0f;
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


#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized, GradientUsage(true)]
        private Gradient secondaryGradient;


#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized]
        private Gradient mainGradientNoHdr;


#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        [NonSerialized]
        private Gradient secondaryGradientNoHdr;


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
        [BoxGroup("Collision"), ValueDropdown(nameof(GetAllEffectID))] 
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
        }

        public override void Clean()
        {
        }

    }
}
