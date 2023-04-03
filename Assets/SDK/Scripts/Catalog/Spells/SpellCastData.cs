using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class SpellCastData : SpellData
    {
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
