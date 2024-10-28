using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad.Modules;
using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class LevelModuleWaveAssault : LevelModule
    {
        public string rewardPillarAddress;

        public int defaultLength = 3;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string textFightGroupId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextFightGroupId))]
#endif
        public string textFightId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextFightGroupId))]
#endif
        public string textWaveId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string textReturnHomeGroupId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextReturnHomeGroupId))]
#endif
        public string textReturnHomeId;

        public Bounds? pillarZone;
        public float returnHomeFadeInDuration = 2.0f;



        #region ODIN_METHODS

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextFightGroupId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(textFightGroupId);
        }

        public List<ValueDropdownItem<string>> GetAllTextReturnHomeGroupId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(textReturnHomeGroupId);
        }
#endif

        #endregion
    }
}