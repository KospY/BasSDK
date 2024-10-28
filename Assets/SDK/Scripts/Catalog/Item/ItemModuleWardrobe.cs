using ThunderRoad.Manikin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class ItemModuleWardrobe : ItemModule
    {
	    public static ManikinEditorLocationLabels manikinChannelLayers;
	    private static bool loadingManikinChannelLayers;
	    const string manikinChanneLayersAddress = "Bas.Manikin.ChannelLayers";

        public float[] castSpeedPerHand;
	    
	    
        public enum CastShadows { None = 0, PlayerOnly = 1, PlayerAndNPC = 2 }
        public Equipment.WardRobeCategory category = Equipment.WardRobeCategory.Apparel;
        public CastShadows castShadows = CastShadows.None;
        public List<CreatureWardrobe> wardrobes = new List<CreatureWardrobe>();
        public bool isMetal;
        public int armorSoundEffectPriority;
        public string armorSoundEffectID;

        [Serializable]
        public class CreatureWardrobe
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllCreatureName))] 
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
	        if (manikinChannelLayers != null)
	        {
		        return manikinChannelLayers.GetLayer(channel, layer);
	        }
            return -1;
        }
        
        public static string GetLayerName(string channel, int layer)
		{
			if (manikinChannelLayers != null)
			{
				return manikinChannelLayers.GetLayerName(channel, layer);
			}
			return string.Empty;
		}
    }
}
