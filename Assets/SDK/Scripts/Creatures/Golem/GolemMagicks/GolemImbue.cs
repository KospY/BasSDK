using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem/Self-imbue config")]
    public class GolemImbue : GolemAbility
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
#endif
        public float selfImbueDuration = 10f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
#endif
        public float contactDamage = 5f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
#endif
        public float timeBetweenDamage = 0.5f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
#endif
        public bool ungripOnDamage = false;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
#endif
        public bool disarmAllowed = false;
#if ODIN_INSPECTOR && UNITY_EDITOR
        protected List<ValueDropdownItem<string>> GetAllEffectIDs() => Catalog.GetDropdownAllID(Category.Effect);

        [BoxGroup("Self-imbue")]
        [ValueDropdown(nameof(GetAllEffectIDs))]
#endif
        public string bodyEffectID;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
        [ValueDropdown(nameof(GetAllEffectIDs))]
#endif
        public string startEffectID;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Self-imbue")]
#endif
        public List<Golem.InflictedStatus> appliedStatuses = new();

    }
}
