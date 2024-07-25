using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class EffectGroupData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Parent"), ValueDropdown(nameof(GetAllEffectGroupID))] 
#endif
        public string groupId;
        [NonSerialized]
        public EffectGroupData parentGroupData;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public float globalVolumeDb = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public float globalMinPitch = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public float globalMaxPitch = 1;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectGroupID()
        {
            return Catalog.GetDropdownAllID(Category.EffectGroup);
        } 
#endif

        public string GetPath(string path = "")
        {
return string.Empty;
        }

        public static void GetGroupParents(ref List<EffectGroupData> groups, EffectGroupData effectGroupData)
        {
        }

    }
}