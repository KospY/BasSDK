using System;
using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using ThunderRoad.Skill.Spell;
using UnityEngine;

namespace ThunderRoad.Skill.SpellMerge
{
    public class SpellMergeNeutronCore : SpellMergeFire
    {
#if ODIN_INSPECTOR
        [BoxGroup("Physics")]
#endif
        public float drag = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Physics")]
#endif
        public float dragDelay = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float forceMultiplier = -1;
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public AnimationCurve forceDistanceCurve;
        
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public AnimationCurve forceTimeCurve;
        
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float duration = 5;
        
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float guidanceAmount = 20;

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float radius = 4;
    }
}