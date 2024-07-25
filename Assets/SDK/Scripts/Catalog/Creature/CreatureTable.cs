using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Analytics;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class CreatureTable : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        [Multiline]
        public string description;

#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool linkedToGenderRatio;

#if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)] 
#endif
        public List<Drop> drops;


        public override CatalogData Clone()
        {
            CreatureTable clone = MemberwiseClone() as CreatureTable;
            clone.drops = drops.Select(item => (Drop)item.Clone()).ToList();
            return clone;
        }

        public List<int> GetFactionIDs()
        {
            List<int> ids = new List<int>();
            foreach (Drop drop in drops)
            {
                ids.AddRange(drop.GetFactionIDs());
            }
            return ids;
        }

        [Serializable]
        public class Drop
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Reference"), EnumToggleButtons, HideLabel] 
#endif
            public Reference reference = Reference.Creature;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllCreatureOrTableID))] 
#endif
            public string referenceID;

            // For compatibility (to remove)
            [JsonIgnore, HideInInspector]
            public string creatureID;

            // For compatibility (to remove)
            [JsonIgnore, HideInInspector]
            public string creatureTableID;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Faction")]
            [HideLabel] 
#endif
            public bool overrideFaction;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Faction")]
            [ShowIf("overrideFaction", true), ValueDropdown(nameof(GetAllFactionID)), HideLabel] 
#endif
            public int factionID = 0;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Container"), HideLabel] 
#endif
            public bool overrideContainer;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Container"), ShowIf("overrideContainer", true), ValueDropdown(nameof(GetAllContainerID)), HideLabel] 
#endif
            public string overrideContainerID;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Brain"), HideLabel] 
#endif
            public bool overrideBrain;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Brain"), ShowIf("overrideBrain", true), ValueDropdown(nameof(GetAllBrainID)), HideLabel] 
#endif
            public string overrideBrainID;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Ethnicity"), HideLabel] 
#endif
            public bool overrideEthnicity;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Ethnicity"), ShowIf("overrideEthnicity", true), ValueDropdown(nameof(GetAllEthnicityID)), HideLabel] 
#endif
            public string overrideEthnicityID;

            
#if ODIN_INSPECTOR
            [HorizontalGroup("Probability Weights"), HideLabel, ShowInInspector, OnValueChanged(nameof(EditorCalculateWeight)), PropertyOrder(0)]
#endif
            public int probabilityWeight;

#if ODIN_INSPECTOR
            [ShowInInspector, ReadOnly, ProgressBar(0, 100), PropertyOrder(1)] 
#endif
            [NonSerialized]
            public float probabilityPercent;
            [NonSerialized]
            public float probabilityRangeFrom;
            [NonSerialized]
            public float probabilityRangeTo;


#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllCreatureOrTableID()
            {
                if (reference == Reference.Creature)
                {
                    return Catalog.GetDropdownAllID(Category.Creature);
                }
                else
                {
                    return Catalog.GetDropdownAllID(Category.CreatureTable);
                }
            }

            public List<ValueDropdownItem<string>> GetAllContainerID()
            {
                return Catalog.GetDropdownAllID(Category.Container);
            }

            public List<ValueDropdownItem<string>> GetAllBrainID()
            {
                return Catalog.GetDropdownAllID(Category.Brain);
            }

            public List<ValueDropdownItem<string>> GetAllEthnicityID()
            {
                //hardcode a list of IDs for now
                List<ValueDropdownItem<string>> list = new List<ValueDropdownItem<string>>();
                list.Add(new ValueDropdownItem<string>("Eradian", "Eradian"));
                list.Add(new ValueDropdownItem<string>("Madene", "Madene"));
                list.Add(new ValueDropdownItem<string>("Sentari", "Sentari"));
                list.Add(new ValueDropdownItem<string>("Kharese", "Kharese"));
                list.Add(new ValueDropdownItem<string>("Raike", "Raike"));
                return list;
            }
            public List<ValueDropdownItem<int>> GetAllFactionID()
            {
                return Catalog.gameData.GetFactions();
            } 
#endif

            public enum Reference
            {
                Creature,
                Table,
            }

            public int[] GetFactionIDs()
            {

                if (overrideFaction)
                {
                    return new int[] { factionID };
                }
                else
                {
                    if (referenceID == "None" || referenceID == null) return new int[0];
                    if (reference == Reference.Table)
                    {
                        return Catalog.GetData<CreatureTable>(referenceID)?.GetFactionIDs()?.ToArray() ?? new int[0];
                    }
                    else
                    {
                        return new int[] { Catalog.GetData<CreatureData>(referenceID)?.factionId ?? 0 };
                    }
                }

            }

            public void OnCatalogRefresh()
            {
                if (!string.IsNullOrEmpty(referenceID)) return;
                switch (reference)
                {
                    case Reference.Creature:
                        referenceID = creatureID;
                        break;
                    case Reference.Table:
                        referenceID = creatureTableID;
                        break;
                }
            }


            public Drop Clone()
            {
                return MemberwiseClone() as Drop;
            }
        }



        public override int GetCurrentVersion()
        {
            return 1;
        }


        public void EditorCalculateWeight()
        {
            CalculateWeight();
        }

        /// <summary>
        /// Calculates the percentage and assigns the probabilities how many times
        /// the items can be picked. Function used also to validate data when tweaking numbers in editor.
        /// </summary>	
        public void CalculateWeight()
        {
        }


        protected bool TryPick(int pickCount, out CreatureData creatureData, System.Random randomGen)
        {
creatureData = null;
            return false;
        }
    }
}
