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
	public class EffectModuleDecal : EffectModule
    {
#if ODIN_INSPECTOR
        [BoxGroup("Decal")] 
#endif
        public string materialAddress;
        [NonSerialized]
        public Material material;
#if ODIN_INSPECTOR
        [BoxGroup("Decal")] 
#endif
        public float baseLifeTime = 60;
#if ODIN_INSPECTOR
        [BoxGroup("Decal")] 
#endif
        public float emissionLifeTime = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Decal")] 
#endif
        public float fadeRefreshSpeed = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Decal")] 
#endif
        public int priority = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Decal")] 
#endif
        public bool allowRagdollPart = false;

#if ODIN_INSPECTOR
        [BoxGroup("Size")] 
#endif
        public Vector3 size = Vector3.one;
#if ODIN_INSPECTOR
        [BoxGroup("Size")] 
#endif
        public float sizeRandomRange = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Size")] 
#endif
        public AnimationCurve sizeCurve;

#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        public EffectTarget linkBaseColor = EffectTarget.None;
#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        public EffectTarget linkEmissionColor = EffectTarget.None;

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

        public static List<EffectDecal> pool = new List<EffectDecal>();
        public static Transform poolRoot;
        public static int poolCount = 100;

        public override void Clean()
        {
        }

        public override void CopyHDRToNonHDR()
        {
        }

    }
}
