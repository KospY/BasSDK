using System.Collections.Generic;
using UnityEngine;
using System;
using ThunderRoad.Skill.SpellPower;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public struct SavedSpells
    {
        public SpellCastData left;
        public SpellCastData right;
        public SpellTelekinesis tkLeft;
        public SpellTelekinesis tkRight;

        public SavedSpells(SpellCastData left, SpellCastData right, SpellTelekinesis tkLeft, SpellTelekinesis tkRight)
        {
            this.left = left;
            this.right = right;
            this.tkLeft = tkLeft;
            this.tkRight = tkRight;
        }
    }
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/Mana.html")]
    [AddComponentMenu("ThunderRoad/Creatures/Mana")]
    [RequireComponent(typeof(Creature))]
    public class Mana : ThunderBehaviour
    {
	    public delegate void FocusChangeEvent(float focus, float maxFocus);
	    public event FocusChangeEvent OnFocusChange;


        protected bool focusReady = true;
        protected bool focusFull = true;

        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public SpellCaster casterLeft;
        [NonSerialized]
        public SpellCaster casterRight;

    }
}