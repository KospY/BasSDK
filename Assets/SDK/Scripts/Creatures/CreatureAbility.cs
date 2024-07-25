using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

#if UNITY_EDITOR
using UnityEditor;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif
#endif

namespace ThunderRoad
{
    [System.Serializable]
    //[CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem ability/Generic")]
    public class CreatureAbility : ScriptableObject
    {
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public bool multiHit = true;
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
        [HorizontalGroup("Damage/Horiz1")]
#endif
        public float contactDamage;
#if ODIN_INSPECTOR
        [HorizontalGroup("Damage/Horiz1")]
#endif
        public bool breakBreakables;
#if ODIN_INSPECTOR
        [HorizontalGroup("Damage/Horiz2")]
#endif
        public float contactForce;
#if ODIN_INSPECTOR
        [HorizontalGroup("Damage/Horiz2")]
#endif
        public bool forceUngrip;

        public Creature creature { get; protected set; }

        public virtual void Setup(Creature creature)
        {
            this.creature = creature;
        }
    }
}
