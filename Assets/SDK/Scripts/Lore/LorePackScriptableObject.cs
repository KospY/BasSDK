using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Lore/Lore Part pack")]
    public class LorePackScriptableObject : ScriptableObject
    {
        [Serializable]
        public class LoreItemData
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllTextGroupID))]
#endif //ODIN_INSPECTOR
            public string groupId;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllTextId))]
#endif //ODIN_INSPECTOR
            public string titleId;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllTextId))]
#endif //ODIN_INSPECTOR
            public string loreId;

            public LoreScriptableObject.LoreType loreType;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllTextGroupID()
            {
                return Catalog.GetTextData().GetDropdownAllTextGroups();
            }

            public List<ValueDropdownItem<string>> GetAllTextId()
            {
                return Catalog.GetTextData().GetDropdownAllTexts(groupId);
            }

           


#endif //ODIN_INSPECTOR

            public string contentType = "Bas.UI.DefaultNoteContent";

            [Tooltip("Display any sprites from the pack in the journal?")]
            public bool displayGraphicsInJournal;

#if UNITY_EDITOR && ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> ListContentTypes()
            {
                return new List<ValueDropdownItem<string>>()
        {
    new ValueDropdownItem<string>("Factionless", "Bas.UI.NoteContentFactionless"),
    new ValueDropdownItem<string>("Outlaw", "Bas.UI.NoteContentOutlaw"),
    new ValueDropdownItem<string>("Wildfolk", "Bas.UI.NoteContentWildfolk"),
    new ValueDropdownItem<string>("Eraden", "Bas.UI.NoteContentEradenCommon"),
    new ValueDropdownItem<string>("Eraden Royal", "Bas.UI.NoteContentEradenRoyal"),
    new ValueDropdownItem<string>("The Eye", "Bas.UI.NoteContentTheEyeCommon"),
    new ValueDropdownItem<string>("The Eye Propaganda", "Bas.UI.NoteContentTheEyePropaganda"),
    new ValueDropdownItem<string>("Dalgarian Preservation Society", "Bas.UI.NoteContentDalgarianPreservationSociety"),
    new ValueDropdownItem<string>("Dalgarian Tablet", "Bas.UI.NoteContentDalgarianTablet"),
  };
            }
#endif
        }

        [Header("Localization Ids")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif //ODIN_INSPECTOR
        public string groupId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif //ODIN_INSPECTOR
        public string nameId;

        [Tooltip("index use to define the position of thhe pack in the journal")]
        public int journalIndex;

        [Header("Requirement")]
        [Tooltip("lore that need to be read before this one")]
        public List<LorePackScriptableObject> loreRequirements;

        [Header("Conditions")]
        public LorePackCondition condition;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllItemId))]
#endif //ODIN_INSPECTOR
        public string itemId = "Note";
        public bool spawnPackAsSingleItem;

        [Header("Lore Item Data")]
        public List<LoreItemData> loreData;




#if ODIN_INSPECTOR

        public List<ValueDropdownItem<string>> GetAllItemId()
        {
            return new List<ValueDropdownItem<string>>()
                {
                    new ValueDropdownItem<string>("Note", "Note"),
                    new ValueDropdownItem<string>("DalgarianTablet", "DalgTablet"),
                    new ValueDropdownItem<string>("Notepad", "Notepad"),
                    new ValueDropdownItem<string>("NotepadOutlaw", "NotepadOutlaw"),
                    new ValueDropdownItem<string>("NotepadWildfolk", "NotepadWildfolk"),
                    new ValueDropdownItem<string>("NotepadEraden", "NotepadEradenCommon"),
                    new ValueDropdownItem<string>("NotepadEradenRoyal", "NotepadEradenRoyal"),
                    new ValueDropdownItem<string>("NotepadTheEye", "NotepadTheEyeCommon"),
                    new ValueDropdownItem<string>("NotepadTheEyePropaganda", "NotepadTheEyePropaganda"),
                    new ValueDropdownItem<string>("NotepadDalgarianSociety", "NotepadDalgarianSociety")
                };
        }
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(groupId);
        }


#endif //ODIN_INSPECTOR

        #region Tool
#if UNITY_EDITOR
        int noteIndex = 0;
        bool currentlySpawning = false;
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllLanguages()
        {
            return Catalog.GetDropdownAllID<TextData>();
        }
        [ValueDropdown(nameof(GetAllLanguages))]
#endif //ODIN_INSPECTOR
        [Header("Editor Tool")]
        public string language = "English";
        [Button]
        public void SpawnLore()
        {
        }


        private void OnItemSpawn(Item obj)
        {
            ILoreDisplay loreDisplay = obj.GetComponentInChildren<ILoreDisplay>();
            if (loreDisplay == null)
            {
                Debug.LogError("Can not find ILoreDisplay in the item : " + obj.itemId);
                return;
            }

            loreDisplay.EditorSetLore(loreData[noteIndex]);
            noteIndex++;
        }

        private void OnItemSpawnAllData(Item obj)
        {
            ILoreDisplay loreDisplay = obj.GetComponentInChildren<ILoreDisplay>();
            if (loreDisplay == null)
            {
                Debug.LogError("Can not find ILoreDisplay in the item : " + obj.itemId);
                return;
            }

            loreDisplay.EditorSetMultipleLore(loreData);
        }
#endif //UNITY_EDITOR
        #endregion Tool
    }
}
