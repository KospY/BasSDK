using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Reveal;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class EffectModuleReveal : EffectModule
    {
        
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public string maskTextureContainerAddress;
        [NonSerialized]
        public TextureContainer textureContainer;
        

#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public EffectReveal.Direction applyOn = EffectReveal.Direction.Target;

#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public TypeFilter typeFilter = TypeFilter.Default | TypeFilter.Body | TypeFilter.Outfit;

        [Flags]
        public enum TypeFilter
        {
            Default = 1,
            Body = 2,
            Outfit = 4,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public float depth = 1.2f;

#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public float minSize = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public float maxSize = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public float penetrationSize = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public Vector4 minChannelMultiplier = Vector4.one;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public Vector4 maxChannelMultiplier = Vector4.one;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public Vector4 penetrationChannelMultiplier = Vector4.one;

#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public float offsetDistance = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public bool allowItem = false;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public bool allowRagdollPart = false;
#if ODIN_INSPECTOR
        [BoxGroup("Reveal")] 
#endif
        public RevealData[] revealData;

        public static List<EffectReveal> pool = new List<EffectReveal>();
        public static Transform poolRoot;



    }
}