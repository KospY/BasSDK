using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class UIText : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string textGroupId;

        [Tooltip("Enable this flag to have dynamic localized texts set from script. *The group id must be set on the inspector.")]
        public bool setFromScript;

        [Multiline]
        public string text;
        public bool forceUpperCase;

        /// <summary>
        /// Invoked when this changes the target text component.
        /// </summary>
        public event System.Action TextChanged;
        public bool ForceEnglishForNonLatin = false; //should this UI text force english for non-latin languages?
        public bool ForceLatinFontForNonLatin = false;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }
#endif        
    }
}
