using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Wrist stats")]
    public class WristStats : ThunderBehaviour
    {
        public float showDistance = 0.31f;
        public float showAngle = 40.0f;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string healthEffectId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string manaEffectId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string focusEffectId;

        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public bool isShown = true;

        
#if ODIN_INSPECTOR && UNITY_EDITOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }
#endif
        
    }
}