using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class EffectGroupData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Parent"), ValueDropdown("GetAllEffectGroupID")] 
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
            if (parentGroupData == null && groupId != null && groupId != "")
            {
                parentGroupData = Catalog.GetData<EffectGroupData>(groupId);
                if (parentGroupData != null)
                {
                    return parentGroupData.GetPath(path.Insert(0, "/" + id));
                }
                else
                {
                    return path.Insert(0, "/" + id);
                }
            }
            else if (parentGroupData != null)
            {
                return parentGroupData.GetPath(path.Insert(0, "/" + id));
            }
            else
            {
                return path.Insert(0, "/" + id);
            }
        }

        public static void GetGroupParents(ref List<EffectGroupData> groups, EffectGroupData effectGroupData)
        {
            groups.Add(effectGroupData);
            if (effectGroupData.parentGroupData != null)
            {
                GetGroupParents(ref groups, effectGroupData.parentGroupData);
            }
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (groupId != null && groupId != "") parentGroupData = Catalog.GetData<EffectGroupData>(groupId);
        }
    }
}