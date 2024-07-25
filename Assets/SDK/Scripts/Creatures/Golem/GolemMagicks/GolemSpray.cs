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
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem/Magic spray config")]
    public class GolemSpray : GolemAbility
    {
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetValidSkills()
        {
            List<ValueDropdownItem<string>> result = new List<ValueDropdownItem<string>>() { new ValueDropdownItem<string>("None", "") };
            if (!Catalog.IsJsonLoaded()) return result;
            foreach (SkillData skillData in Catalog.GetDataList<SkillData>())
            {
                if (skillData is IGolemSprayable)
                {
                    result.Add(new ValueDropdownItem<string>(skillData.id, skillData.id));
                }
            }
            return result;
        }

        [ValueDropdown(nameof(GetValidSkills))]
#endif
        public string spraySkillID;
        public List<string> spraySources = new List<string>();
        public GolemController.AttackMotion sprayMotion = GolemController.AttackMotion.Spray;
        public float sprayAngle = 90f;

    }
}
