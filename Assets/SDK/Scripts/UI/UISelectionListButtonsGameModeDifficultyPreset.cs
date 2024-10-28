using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR

namespace ThunderRoad
{
    public class UISelectionListButtonsGameModeDifficultyPreset : UISelectionListButtons
    {
        public GameObject difficultyParent;
        public TextMeshProUGUI presetDescriptionText;
        public UISelectionListButtonsLevelModeOption LevelOptionPrefab = null;
        public Transform optionsPanel = null;
        public UIText optionDescriptionText;

        [Header("Custom text")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif //ODIN_INSPECTOR
        public string presetCustomLocalizedGroupId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif //ODIN_INSPECTOR
        public string presetCustomLocalizedName;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif //ODIN_INSPECTOR
        public string presetCustomLocalizedDescription;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(presetCustomLocalizedGroupId);
        }
#endif //ODIN_INSPECTOR

    }
}