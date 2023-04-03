using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using System.Collections;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public enum SpawnTarget
    {
        Default,
        Source,
        Target
    }

    public class EffectModuleVfx : EffectModule
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public string vfxAddress;
        [NonSerialized]
        public VisualEffectAsset vfxAsset;
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
        public bool useSecondaryRenderer;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool lookAtTarget;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public SpawnTarget spawnOn = SpawnTarget.Default;

#if ODIN_INSPECTOR
        protected List<ValueDropdownItem<LayerName>> LayerOverrideOptions()
        {
            List<ValueDropdownItem<LayerName>> options = new List<ValueDropdownItem<LayerName>>();
            LayerName[] layerNames = (LayerName[])Enum.GetValues(typeof(LayerName));
            for (int i = 0; i < layerNames.Length; i++)
            {
                LayerName lName = layerNames[i];
                string text = lName == LayerName.None ? "Don't override" : $"Override: {lName.ToString()}";
                options.Add(new ValueDropdownItem<LayerName>(text, lName));
            }
            return options;
        }

        [BoxGroup("General"), ValueDropdown("LayerOverrideOptions")]
#endif
        public LayerName layerOverride = LayerName.None;

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
        [BoxGroup("Color")] 
#endif
        [NonSerialized, GradientUsage(true)]
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
        [BoxGroup("Color")] 
#endif
        [NonSerialized]
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
        [BoxGroup("Color")] 
#endif
        [NonSerialized]
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
        [BoxGroup("PointCache")] 
#endif
        public bool usePointCache = false;
#if ODIN_INSPECTOR
        [BoxGroup("PointCache"), ShowIf("usePointCache")] 
#endif
        public bool pointCacheSkinnedMeshUpdate = false;
#if ODIN_INSPECTOR
        [BoxGroup("PointCache"), ShowIf("usePointCache")] 
#endif
        public int pointCacheMapSize = 512;
#if ODIN_INSPECTOR
        [BoxGroup("PointCache"), ShowIf("usePointCache")] 
#endif
        public int pointCachePointCount = 4096;
#if ODIN_INSPECTOR
        [BoxGroup("PointCache"), ShowIf("usePointCache")] 
#endif
        public int pointCacheSeed = 0;
#if ODIN_INSPECTOR
        [BoxGroup("PointCache"), ShowIf("usePointCache")] 
#endif
        public PointCacheGenerator.Distribution pointCacheDistribution = PointCacheGenerator.Distribution.RandomUniformArea;
#if ODIN_INSPECTOR
        [BoxGroup("PointCache"), ShowIf("usePointCache")] 
#endif
        public PointCacheGenerator.MeshBakeMode pointCacheBakeMode = PointCacheGenerator.MeshBakeMode.Triangle;

#if ODIN_INSPECTOR
        [BoxGroup("Mesh")] 
#endif
        public bool useMesh = false;
#if ODIN_INSPECTOR
        [ShowIf("useMesh")]
        [BoxGroup("Mesh")] 
#endif
        public string meshAddress;
        [NonSerialized]
        public Mesh meshAsset;

#if ODIN_INSPECTOR
        [BoxGroup("Vfx"), TableList(AlwaysExpanded = true)] 
#endif
        public List<VfxProperty> materialProperties = new List<VfxProperty>();


        [Serializable]
        public class VfxProperty
        {
            public string name;

            public class Vector2 : VfxProperty
            {
                public UnityEngine.Vector2 value;
            }

            public class Vector3 : VfxProperty
            {
                public UnityEngine.Vector3 value;
            }

            public class Float : VfxProperty
            {
                public float value;
            }

            public class Int : VfxProperty
            {
                public int value;
            }

            public class Gradient : VfxProperty
            {
                public UnityEngine.Gradient value;
            }
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

        public override void CopyHDRToNonHDR()
        {
            mainGradientNoHdr = mainGradient;
            secondaryGradientNoHdr = secondaryGradient;
        }



        public static void Despawn(EffectVfx effect)
        {
            GameObject.Destroy(effect.gameObject);
        }


    }
}
