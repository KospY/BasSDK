using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class SpellCastData : SpellData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Wheel display")]
#endif
        public string wheelDisplayName;

#if ODIN_INSPECTOR
        [BoxGroup("Wheel display")]
#endif
        public bool hasOrder = false;
#if ODIN_INSPECTOR
        [BoxGroup("Wheel display")]
#endif
        public int order;
        
#if ODIN_INSPECTOR
        [HideInInspector]
#endif
        public int? Order => hasOrder ? order : null;
#if ODIN_INSPECTOR
        [BoxGroup("Wheel display"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string iconEffectId;
        [NonSerialized]
        public EffectData iconEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("AI")] 
#endif
        public CastType aiCastType;
#if ODIN_INSPECTOR
        [BoxGroup("AI")]
        [EnableIf("aiCastType", CastType.CastLoop)] 
#endif
        public float loopMaxDuration = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("AI")] 
#endif
        public float aiCastMinDistance = 1;
#if ODIN_INSPECTOR
        [BoxGroup("AI")] 
#endif
        public float aiCastMaxDistance = 20;
#if ODIN_INSPECTOR
        [BoxGroup("AI")] 
#endif
        public float aiCastGestureLength = 0.7f;

        public enum CastType
        {
            CastSimple,
            CastLoop,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Mana")] 
#endif
        public float minMana = 5;

        public new SpellCastData Clone()
        {
            return this.MemberwiseClone() as SpellCastData;
        }


        public override int GetCurrentVersion()
        {
            return 0;
        }
    }
}
