using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using System.Collections;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
        public bool despawnOnEnd = false;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public float despawnDelay = 0;
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

        [BoxGroup("General"), ValueDropdown(nameof(LayerOverrideOptions))]
#endif
        public LayerName layerOverride = LayerName.None;

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
        }

        public override void CopyHDRToNonHDR()
        {
        }

    }
}
