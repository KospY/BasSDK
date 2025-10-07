using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class ItemModuleCrystal : ItemModule
    {
        [NonSerialized]
        public static float mergeEffectDelay = 2f;
        public static float glowMultiplier = 3f;
        
        [Header("skill tree")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllSkillTreeID))]
#endif
        public string treeName;
        
        [Header("Custom")]
        public bool overrideCrystalColors = false;

        [ColorUsage(true, true)]
#if ODIN_INSPECTOR
        [ShowIf("overrideCrystalColors")]
#endif
        public Color baseColor;
        [ColorUsage(true, true)]
#if ODIN_INSPECTOR
        [ShowIf("overrideCrystalColors")]
#endif
        public Color internalColor;
        [ColorUsage(true, true)]
#if ODIN_INSPECTOR
        [ShowIf("overrideCrystalColors")]
#endif
        public Color animatedColor;
        [ColorUsage(true, true)]
#if ODIN_INSPECTOR
        [ShowIf("overrideCrystalColors")]
#endif
        public Color emissionColor;
        [ColorUsage(true, true)]
#if ODIN_INSPECTOR
        [ShowIf("overrideCrystalColors")]
#endif
        public Color linkVfxColor;
        [ColorUsage(true, true)]
#if ODIN_INSPECTOR
        [ShowIf("overrideCrystalColors")]
#endif
        public Color mergeVfxColor;

        [Header("Merge")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCrystalId))]
#endif
        public string higherTierCrystalId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllShardsId))]
#endif
        public string shardId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string mergeBeginEffectId = "CrystalMergeBegin";
        [NonSerialized]
        public EffectData mergeBeginEffectData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string sparkleEffectId = "CrystalSparkle";
        [NonSerialized]
        public EffectData sparkleEffectData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string hoverEffectId = "CrystalCoreHover";
        [NonSerialized]
        public EffectData hoverEffectData;
        
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string mergeEffectId = "CrystalMerge";
        [NonSerialized]
        public EffectData mergeEffectData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string mergeCrystalEffectAndroidId = "CrystalMergeGlow";
        [NonSerialized]
        public EffectData mergeCrystalEffectAndroidData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string mergeStartEffectId = "CrystalMergeStart";
        [NonSerialized]
        public EffectData mergeStartEffectData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string mergeCompleteEffectId = "CrystalMergeNewTier";
        [NonSerialized]
        public EffectData mergeCompleteEffectData;

        public float endMergeTime = 1;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectId))]
#endif
        public string activateEffectId = "CrystalActivate";
        [NonSerialized]
        public EffectData activateEffectData;

        [NonSerialized]
        public SkillTreeData skillTreeData;

        public bool allowMerge = true;





#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSkillTreeID()
        {
            return Catalog.GetDropdownAllID(Category.SkillTree);
        }
        public List<ValueDropdownItem<string>> GetAllCrystalId()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("None", ""));
            List<CatalogData> allItems = Catalog.GetDataList(Category.Item);
            for (int i = 0; i < allItems.Count; i++)
            {
                if(allItems[i] is ItemData item
                    && item.TryGetModule<ItemModuleCrystal>(out ItemModuleCrystal module)
                    && string.Equals(module.treeName, treeName))
                {
                    dropdownList.Add(new ValueDropdownItem<string>(item.id, item.id));
                }
            }

            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetAllShardsId()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("None", ""));
            foreach (string id in Catalog.GetAllID(Category.Item))
            {
                if (id.Contains("Shard"))
                {
                    dropdownList.Add(new ValueDropdownItem<string>(id, id));
                }
            }
            return dropdownList;
        }
        
        public List<ValueDropdownItem<string>> GetAllEffectId() => Catalog.GetDropdownAllID(Category.Effect);
#endif
    }
}
