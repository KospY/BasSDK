using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UIText : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllTextGroupID")]
#endif
        public string textGroupId;

        [Tooltip("Enable this flag to have dynamic localized texts set from script. *The group id must be set on the inspector.")]
        public bool setFromScript;

        [Multiline]
        public string text;
        public bool forceUpperCase;

        // If true, the localization text is not set with GetString() from the TextGroups (Ex: texts from ItemData or WaveData)
        // This can be used for labels that, depending on the context, can have localized default strings from text groups and
        // also localized text strings from other CatalogData subclasses
        private bool hasCustomLocalization;

        private bool wasInitializedFromScript;

        // If this is true the text string will be used as the Item localization ID and we will try to get the Items localization on refresh
        private bool textIsItem;

    }
}
