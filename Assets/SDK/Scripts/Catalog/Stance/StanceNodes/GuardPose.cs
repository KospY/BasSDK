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
    [System.Serializable]
    public class GuardPose : StanceNode
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetRiposteOptions)), ListDrawerSettings(Expanded = true), FoldoutGroup("$prettifiedID")]
#endif
        public List<string> riposteOptions;

        [NonSerialized]
        public List<AttackMotion> ripostes;

        public override bool customID => false;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetRiposteOptions()
        {
            List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>();
            if (stanceData is MeleeStanceData meleeStanceData)
            {
                for (int i = 0; i < meleeStanceData.ripostes.Count; i++)
                {
                    options.Add(new ValueDropdownItem<string>(meleeStanceData.ripostes[i].id, meleeStanceData.ripostes[i].id));
                }
            }
            return options;
        }
#endif

    }
}
