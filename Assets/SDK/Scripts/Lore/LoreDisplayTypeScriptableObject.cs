using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Lore/LoreDisplayType")]
    public class LoreDisplayTypeScriptableObject : ScriptableObject
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string groupID;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string displayName;
        public Color32 colour;
        [Tooltip("Display this pack as dalgarian in the journal?")] public bool displayInJournalAsDalgarian;
        public int factionOrderIndex;

        public string GetNameKey()
        {
            return groupID + "/" + displayName;
        }

        /// <summary>
        /// Can be deleted after we have some specified, just used to generate dummy data.
        /// See: LoreModule @ 233 (GetAllDisplayType)
        /// If it's not using dummy data this can be deleted.
        /// </summary>
        public static LoreDisplayTypeScriptableObject Create(string displayName, Color32 colour)
        {
            LoreDisplayTypeScriptableObject obj = CreateInstance<LoreDisplayTypeScriptableObject>();
            obj.groupID = "Default";
            obj.displayName = displayName;
            obj.colour = colour;
            return obj;
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(groupID);
        }
#endif // ODIN_INSPECTOR
    }
}
