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

        protected Gradient MainGradient
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
        [BoxGroup("Color"), NonSerialized, GradientUsage(true)] 
#endif
        private Gradient secondaryGradient;

        protected Gradient SecondaryGradient
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
        [BoxGroup("Color"), NonSerialized] 
#endif
        private Gradient mainGradientNoHdr;

        protected Gradient MainGradientNoHdr
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
        [BoxGroup("Color"), NonSerialized] 
#endif
        private Gradient secondaryGradientNoHdr;

        protected Gradient SecondaryGradientNoHdr
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
            if (intensityCurve != null && intensityCurve.keys.Length == 0)
            {
                intensityCurve = null;
            }
            if (sizeCurve != null && sizeCurve.keys.Length == 0)
            {
                sizeCurve = null;
            }
            if (rotationYCurve != null && rotationYCurve.keys.Length == 0)
            {
                rotationYCurve = null;
            }
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

        public override void CopyHDRToNonHDR()
        {
            mainGradientNoHdr = mainGradient;
            secondaryGradientNoHdr = secondaryGradient;
        }


        public static void Despawn(EffectMesh effect)
        {
            GameObject.Destroy(effect.gameObject);
        }



        /// <summary>
        /// Configures an EffectMesh with values from this EffectModuleMesh
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public EffectMesh Configure(EffectMesh effect)
        {
            effect.gameObject.SetActive(true);
            if (step == Effect.Step.Custom) effect.stepCustomHashId = stepCustomIdHash;

            effect.meshSize = localScale;
            effect.meshRotation = localRotation;

            if (effect.meshSizeFromIntensity)
            {
                float value = effect.curveMeshSize.Evaluate(0.0f);
                effect.transform.localScale = new Vector3(value, value, value);
            }
            else
            {
                effect.transform.localScale = localScale;
            }
            effect.meshFilter.mesh = mesh;

            int materialCount = materials.Count;
            Material[] meshMaterials = new Material[materialCount];

            for (var i = 0; i < materialCount; i++)
            {
                Materials m = materials[i];
                if (m is Materials.Material material)
                {
                    meshMaterials[i] = material.value;    
                }
                else
                {
                    Debug.LogError($"EffectModuleMesh: {meshAddress} has null or invalid materials for material address: {m.materialAddress} ");
                }
                
            }

            effect.meshRenderer.materials = meshMaterials;

            effect.meshSizeFromIntensity = useSizeCurve;
            effect.curveMeshSize = sizeCurve;
            effect.meshRotationFromIntensity = useRotationYCurve;
            effect.curveMeshrotY = rotationYCurve;
            effect.meshSizeFadeDuration = sizeFadeDuration;
            effect.meshRotationFadeDuration = rotationFadeDuration;

            int propCount = materialProperties.Count;
            for (var index = 0; index < propCount; index++)
            {
                MaterialProperty materialProperty = materialProperties[index];
                switch (materialProperty)
                {
                    case MaterialProperty.Float f:
                        effect.materialPropertyBlock.SetFloat(materialProperty.name, f.value);
                        break;
                    case MaterialProperty.Int i:
                        effect.materialPropertyBlock.SetInt(materialProperty.name, i.value);
                        break;
                    case MaterialProperty.Vector vector:
                        effect.materialPropertyBlock.SetVector(materialProperty.name, vector.value);
                        break;
                    case MaterialProperty.Color color:
                        effect.materialPropertyBlock.SetColor(materialProperty.name, color.value);
                        break;
                }
            }

            effect.intensityCurve = intensityCurve ?? new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
            effect.linkBaseColor = linkBaseColor;
            effect.linkTintColor = linkTintColor;
            effect.linkEmissionColor = linkEmissionColor;
            effect.lifeTime = lifeTime;
            effect.refreshSpeed = refreshSpeed;

            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset srp = QualitySettings.renderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
            if (srp.supportsHDR)
            {
                effect.SetMainGradient(mainGradient);
                effect.SetSecondaryGradient(secondaryGradient);
            }
            else
            {
                effect.SetMainGradient(mainGradientNoHdr);
                effect.SetSecondaryGradient(secondaryGradientNoHdr);
            }

            return effect;
        }
    }
}
