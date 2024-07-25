using System;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad
{
	[Serializable]
    public class EnemyConfig : CatalogData
    {
        [System.Serializable]
        public class DescriptionByLevelId 
        {
            public string levelId;
            public string description;
        }

        public const string GroupId = "EnemyConfig";

        [Header("UI")]
        public string iconAddress;
        public bool customIconColor = false;
        
        #if ODIN_INSPECTOR
        [ShowIf("customIconColor")]
        #endif
        [Tooltip("Icon color")]
        public Color iconColor = Color.white;
        [Tooltip("Banner color")]
        public Color color;
        public string name;
        public string nameLocalizationId;
        
        //public Dictionary<string, string> descriptionByLevelId;
        public List<DescriptionByLevelId> descriptionsByLevelId;

        [Header("CREATURE TABLES")]
        [Header("Patrol Enemies")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string patrolMixId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string patrolMeleeId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string patrolRangedId;

        [Header("Alert Enemies")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string alertMixId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string alertMeleeId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string alertRangedId;

        [Header("Rare Enemies")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string rareMixId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string rareMeleeId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
#endif
        public string rareRangedId;

        [Header("WAVES")]
        [Header("Standard Waves")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string meleeOnlyStd;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string meleeFocusedStd;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string mixedStd;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string rangedFocusedStd;

        [Header("Arena Waves")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string meleeOnlyArena;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string meleeFocusedArena;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string mixedArena;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string rangedFocusedArena;

        [Header("Ending Waves")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string meleeOnlyEnd;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string meleeFocusedEnd;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string mixedEnd;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllWavesID))]
#endif
        public string rangedFocusedEnd;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCreatureTableID()
        {
            var dropdownList = new List<ValueDropdownItem<string>> { new("None", "") };
            var creatureTableIds = Catalog.GetDataList(Category.CreatureTable).OfType<CreatureTable>().Select(x => x.id).ToList();
            foreach (var id in creatureTableIds)
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetAllWavesID()
        {
            var dropdownList = new List<ValueDropdownItem<string>> { new("None", "") };
            var waveIds = Catalog.GetDataList(Category.Wave).OfType<WaveData>().Select(x => x.id).ToList();
            foreach (var id in waveIds)
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        }
#endif
    }
}