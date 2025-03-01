using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class TextData : CatalogData
    {
        public const string DEFAULT_TEXT_GROUP = "Default";

        // Used for mod.io (we could get the language code dynamically using a call to the CultureInfo with the ISO 639 but the chinese languages code is different on the mod.io API,
        // so we set the code here, instead of adding specific code to check which language is and manually change the code for some of them).
        public string languageCode;

#if ODIN_INSPECTOR
        [TabGroup("Group", "Texts")] 
#endif
        [JsonMergeKey]
        public List<TextGroup> textGroups;
#if ODIN_INSPECTOR
        [TabGroup("Group", "Items")] 
#endif
        public List<Item> items;
#if ODIN_INSPECTOR
        [TabGroup("Group", "Waves")] 
#endif
        public List<Wave> waves;

        [Serializable]
        public class TextGroup
        {
            [JsonMergeKey]
            public string id;
            public List<TextID> texts;

        }

        [Serializable]
        public class TextID
        {
#if ODIN_INSPECTOR
            [HorizontalGroup(LabelWidth = 20, Width = 200)] 
#endif
            [JsonMergeKey]
            public string id;
#if ODIN_INSPECTOR
            [HorizontalGroup(), HideLabel] 
#endif
            [TextArea(0, 20)]
            public string text;
            public string spriteAddress;
            public string videoAddress;
            [NonSerialized]
            public IResourceLocation spriteLocation;
            [NonSerialized]
            public IResourceLocation videoLocation;
        }

        [Serializable]
        public class Item
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Split", LabelWidth = 50, Width = 200), VerticalGroup("Split/vert")] 
#endif
            [JsonMergeKey]
            public string id;
#if ODIN_INSPECTOR
            [VerticalGroup("Split/vert")] 
#endif
            public string name;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split"), HideLabel] 
#endif
            [TextArea(0, 20)]
            public string description;
        }

        [Serializable]
        public class Wave
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Split", LabelWidth = 50, Width = 200), VerticalGroup("Split/vert")] 
#endif
            [JsonMergeKey]
            public string id;
#if ODIN_INSPECTOR
            [VerticalGroup("Split/vert")] 
#endif
            public string title;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split"), HideLabel] 
#endif
            [TextArea(0, 20)]
            public string description;
        }

        public override int GetCurrentVersion()
        {
            return 1;
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetDropdownAllTextGroups()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            foreach (TextGroup textGroup in textGroups)
            {
                dropdownList.Add(new ValueDropdownItem<string>(textGroup.id, textGroup.id));
            }
            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetDropdownAllTexts(string groupId)
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            foreach (TextGroup textGroup in textGroups)
            {
                if (string.Equals(textGroup.id, groupId, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (TextID textId in textGroup.texts)
                    {
                        dropdownList.Add(new ValueDropdownItem<string>(textId.id, textId.id));
                    }
                    break;
                }
            }
            return dropdownList;
        } 
#endif
    }
}
