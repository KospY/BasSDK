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
	public class EffectModuleMesh : EffectModule
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public new AnimationCurve intensityCurve;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float lifeTime = 5;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float refreshSpeed = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        public EffectTarget linkBaseColor = EffectTarget.None;
#if ODIN_INSPECTOR
        [BoxGroup("Color")] 
#endif
        public EffectTarget linkTintColor = EffectTarget.None;
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
        [BoxGroup("Color"), NonSerialized, GradientUsage(true)] 
#endif
        private Gradient secondaryGradient;


#if ODIN_INSPECTOR
        [BoxGroup("Color"), NonSerialized] 
#endif
        private Gradient mainGradientNoHdr;


#if ODIN_INSPECTOR
        [BoxGroup("Color"), NonSerialized] 
#endif
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
        [BoxGroup("Mesh")] 
#endif
        public Vector3 localScale = Vector3.one;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public bool useSizeCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh"), ShowIf("useSizeCurve")] 
#endif
        public AnimationCurve sizeCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public float sizeFadeDuration;

#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public Vector3 localPosition = Vector3.zero;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public Vector3 localRotation = Vector3.zero;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public bool useRotationYCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh"), ShowIf("useRotationYCurve")] 
#endif
        public AnimationCurve rotationYCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public float rotationFadeDuration;

#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public string meshAddress;
        [NonSerialized]
        public Mesh mesh;

#if ODIN_INSPECTOR
        [BoxGroup("Mesh"), ShowInInspector, TableList(AlwaysExpanded = true)] 
#endif
        public List<Materials> materials = new List<Materials>();

#if ODIN_INSPECTOR
        [BoxGroup("Mesh"), ShowInInspector, TableList(AlwaysExpanded = true)] 
#endif
        public List<MaterialProperty> materialProperties = new List<MaterialProperty>();


        public class Materials
        {
            public string materialAddress;

            public class Material : Materials 
            {
#if ODIN_INSPECTOR
                [ShowInInspector, ReadOnly] 
#endif
                [NonSerialized]
                public UnityEngine.Material value; 
            }
        }

        public class MaterialProperty
        {
            public string name;
            [Serializable]
            public class Vector : MaterialProperty { public Vector4 value; }
            [Serializable]
            public class Float : MaterialProperty { public float value; }
            [Serializable]
            public class Int : MaterialProperty { public int value; }
            [Serializable]
            public class Color : MaterialProperty { public UnityEngine.Color value; }
        }


        public override void Clean()
        {
        }

        public override void CopyHDRToNonHDR()
        {
        }


    }
}
