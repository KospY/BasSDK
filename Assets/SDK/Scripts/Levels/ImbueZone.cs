using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using ThunderRoad.Skill;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/ImbueZone.html")]
    [RequireComponent(typeof(Collider))]
    public class ImbueZone : MonoBehaviour
    {
        public float transferRate = 0.1f;
        public float transferMaxPercent = 50f;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllSpellCastChargeID))]
#endif
        public string imbueSpellId;
#if ODIN_INSPECTOR
        [FormerlySerializedAs("imbueSkillIds")]
        [ValueDropdown(nameof(GetAllSkillID))]
#endif
        public List<string> imbueSkillIDs;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSpellCastChargeID()
            => Catalog.GetDropdownAllID<SpellCastCharge>();
        public List<ValueDropdownItem<string>> GetAllSkillID()
            => Catalog.GetDropdownAllID<SpellSkillData>();
#endif

    }
}