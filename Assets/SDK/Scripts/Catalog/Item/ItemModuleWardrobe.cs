using ThunderRoad.Manikin;
using System;
using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class ItemModuleWardrobe : ItemModule
    {
        public enum CastShadows { None = 0, PlayerOnly = 1, PlayerAndNPC = 2 }

        public Equipment.WardRobeCategory category = Equipment.WardRobeCategory.Apparel;
        public CastShadows castShadows = CastShadows.None;
        public List<CreatureWardrobe> wardrobes = new List<CreatureWardrobe>();
        public bool isMetal;

        public static Dictionary<string, string[]> locations = new Dictionary<string, string[]>()
        {
            { "Head", new string[] { "Helmet", "Headband", "Nose", "EarringRight", "EarringLeft", "Beard", "Hair", "Tatoo","EyesBrows", "EyesLashes","Eyes","Mouth","Body" } },
            { "Torso", new string[] { "ShoulderRight", "ShoulderLeft", "Cloak", "Jacket", "Underwear", "Tatoo", "Body" } },
            { "HandLeft", new string[] { "Glove", "Wrist", "Ring", "Tatoo", "Body" } },
            { "HandRight", new string[] { "Glove", "Wrist", "Ring", "Tatoo", "Body" } },
            { "Legs", new string[] { "Skirt", "Armor", "Pants", "Tatoo", "Body" } },
            { "Feet", new string[] { "Boot", "Socket", "Tatoo", "Body" } },
            { "Global", new string[] { "Vfx" } }
        };

        [Serializable]
        public class CreatureWardrobe
        {
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllCreatureName")] 
#endif
            [JsonMergeKey]
            public string creatureName;
            public string wardrobeDataAddress;
#if ODIN_INSPECTOR
            [ShowInInspector, ReadOnly] 
#endif
            [NonSerialized]
            public ManikinWardrobeData manikinWardrobeData;

            public CreatureWardrobe Clone()
            {
                return MemberwiseClone() as CreatureWardrobe;
            }

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllCreatureName()
            {
                List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
                foreach (CreatureData creatureData in Catalog.GetDataList(Category.Creature))
                {
                    if (dropdownList.Exists(d => d.Value == creatureData.name))
                    {
                        continue;
                    }
                    dropdownList.Add(new ValueDropdownItem<string>(creatureData.name, creatureData.name));
                }
                return dropdownList;
            } 
#endif
        }

        public static int GetLayer(string channel, string layer)
        {
            if (locations.TryGetValue(channel, out string[] layers))
            {
                for (int i = 0; i < layers.Length; i++)
                {
                    if (layers[i] == layer)
                    {
                        return i;
                    }
                }
            }

            return 1;
        }
    }
}
