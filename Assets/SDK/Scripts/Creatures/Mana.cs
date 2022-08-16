using System.Collections.Generic;
using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Mana")]
    [AddComponentMenu("ThunderRoad/Creatures/Mana")]
    [RequireComponent(typeof(Creature))]
    public class Mana : ThunderBehaviour
    {
        public float currentMana = 50;
        public float maxMana = 50;
        public float manaRegen = 10;

        public float currentFocus = 30;
        public float maxFocus = 30;
        public float focusRegen = 2;

        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public SpellCaster casterLeft;
        [NonSerialized]
        public SpellCaster casterRight;

    }
}