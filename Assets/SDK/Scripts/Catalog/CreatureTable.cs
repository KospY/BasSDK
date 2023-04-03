using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Analytics;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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
        [BoxGroup("Difficulty")]
        [HorizontalGroup("Difficulty/Line"), MinMaxSlider(0, 4), LabelWidth(120), InlineButton("CopyFromLeft")] 
#endif
        public Vector2Int minMaxDifficulty = new Vector2Int(0, 0);

#if ODIN_INSPECTOR
        [HorizontalGroup("Difficulty/Line"), ShowInInspector, EnumToggleButtons, OnValueChanged("EditorCalculateWeight"), LabelWidth(150), InlineButton("CopyFromRight"), HideLabel] 
#endif
        [NonSerialized]
        public static Difficulty showWeightDifficulty;

        public enum Difficulty
        {
            VeryEasy,
            Easy,
            Medium,
            Hard,
            VeryHard,
        }

#if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)] 
#endif
        public List<Drop> drops;
        private float probabilityTotalWeight;
        private static int maxPickCount = 20;

        public override CatalogData Clone()
        {
            CreatureTable clone = MemberwiseClone() as CreatureTable;
            clone.drops = drops.Select(item => (Drop)item.Clone()).ToList();
            return clone;
        }

        public void CopyFromLeft()
        {
            if ((int)showWeightDifficulty == 0) return;
            foreach (Drop drop in drops)
            {
                drop.probabilityWeights[(int)showWeightDifficulty] = drop.probabilityWeights[(int)showWeightDifficulty - 1];
            }
        }

        public void CopyFromRight()
        {
            if ((int)showWeightDifficulty == 4) return;
            foreach (Drop drop in drops)
            {
                drop.probabilityWeights[(int)showWeightDifficulty] = drop.probabilityWeights[(int)showWeightDifficulty + 1];
            }
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
            [ValueDropdown("GetAllCreatureOrTableID")] 
#endif
            public string referenceID;

            // For compatibility (to remove)
            [JsonIgnore, HideInInspector]
            public string creatureID;
            [JsonProperty("creatureID")]
            private string creatureIDAlternateSetter
            {
                set { creatureID = value; }
            }

            // For compatibility (to remove)
            [JsonIgnore, HideInInspector]
            public string creatureTableID;
            [JsonProperty("creatureTableID")]
            private string creatureTableIDAlternateSetter
            {
                set { creatureTableID = value; }
            }

            // For compatibility (to remove) Added in U10
            [JsonIgnore, HideInInspector]
            public float probabilityWeight;
            [JsonProperty("probabilityWeight")]
            private float probabilityWeightAlternateSetter
            {
                set { probabilityWeights[0] = (int)value; }
            }

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Faction")]
            [HideLabel] 
#endif
            public bool overrideFaction;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Faction")]
            [ShowIf("overrideFaction", true), ValueDropdown("GetAllFactionID"), HideLabel] 
#endif
            public int factionID = 0;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Container"), HideLabel] 
#endif
            public bool overrideContainer;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Container"), ShowIf("overrideContainer", true), ValueDropdown("GetAllContainerID"), HideLabel] 
#endif
            public string overrideContainerID;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Brain"), HideLabel] 
#endif
            public bool overrideBrain;

#if ODIN_INSPECTOR
            [HorizontalGroup("Override Brain"), ShowIf("overrideBrain", true), ValueDropdown("GetAllBrainID"), HideLabel] 
#endif
            public string overrideBrainID;

            [HideInInspector]
            public int[] probabilityWeights = new int[5];

#if ODIN_INSPECTOR
            [HorizontalGroup("Probability Weights"), HideLabel, ShowInInspector, OnValueChanged("EditorCalculateWeight"), PropertyOrder(0)] 
#endif
            protected int weight
            {
                get { return probabilityWeights[(int)showWeightDifficulty]; }
                set { probabilityWeights[(int)showWeightDifficulty] = Mathf.Abs(value); }
            }

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

            public void EditorCalculateWeight()
            {
                foreach (CreatureTable creatureTable in Catalog.GetDataList(Category.CreatureTable))
                {
                    creatureTable.EditorCalculateWeight();
                }
            }

            public Drop Clone()
            {
                return MemberwiseClone() as Drop;
            }
        }


#if ODIN_INSPECTOR

        [BoxGroup("Generation test"), NonSerialized, ShowInInspector]
        public string generationCreature;
        [BoxGroup("Generation test"), NonSerialized, ShowInInspector]
        public string generationFaction;
        [BoxGroup("Generation test"), NonSerialized, ShowInInspector]
        public string generationContainer;
        [BoxGroup("Generation test"), NonSerialized, ShowInInspector]
        public string generationBrain;

        [Button]
        public void TestGeneration()
        {
            if (TryPick((int)showWeightDifficulty, out CreatureData creatureData))
            {
                generationCreature = creatureData.id;
                generationFaction = Catalog.gameData.GetFaction(creatureData.factionId).name;
                generationContainer = creatureData.containerID;
                generationBrain = creatureData.brainId;
            }
            else
            {
                generationCreature = "Nothing";
                generationFaction = null;
                generationContainer = null;
                generationBrain = null;
            }
        }
#endif

        public override int GetCurrentVersion()
        {
            return 1;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            
            drops ??= new List<Drop>();

            CalculateWeight((int)showWeightDifficulty);
        }

        public void EditorCalculateWeight()
        {
            CalculateWeight((int)showWeightDifficulty);
        }

        /// <summary>
        /// Calculates the percentage and assigns the probabilities how many times
        /// the items can be picked. Function used also to validate data when tweaking numbers in editor.
        /// </summary>	
        public void CalculateWeight(int difficulty)
        {
            if (drops.IsNullOrEmpty()) return;

            float currentProbabilityWeightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            int dropsCount = drops.Count;
            for (var i = 0; i < dropsCount; i++)
            {
                Drop lootDropItem = drops[i];
                lootDropItem.OnCatalogRefresh();
                if (lootDropItem.probabilityWeights[difficulty] < 0)
                {
                    // Prevent usage of negative weight.
                    lootDropItem.probabilityWeights[difficulty] = 0;
                    continue;
                }
                lootDropItem.probabilityRangeFrom = currentProbabilityWeightMaximum;
                currentProbabilityWeightMaximum += lootDropItem.probabilityWeights[difficulty];
                lootDropItem.probabilityRangeTo = currentProbabilityWeightMaximum;
                
            }
            probabilityTotalWeight = currentProbabilityWeightMaximum;
            // Calculate percentage of item drop select rate.
            for (var i = 0; i < dropsCount; i++)
            {
                Drop lootDropItem = drops[i];
                lootDropItem.probabilityPercent = lootDropItem.probabilityWeights[difficulty] / probabilityTotalWeight * 100;
            }
        }

        public bool TryPick(out CreatureData creatureData, System.Random randomGen = null)
        {
            return TryPick(0, 0, out creatureData, randomGen);
        }

        public bool TryPick(int difficulty, out CreatureData creatureData, System.Random randomGen = null)
        {
            return TryPick(difficulty, 0, out creatureData, randomGen);
        }

        protected bool TryPick(int difficulty, int pickCount, out CreatureData creatureData, System.Random randomGen)
        {
            difficulty = Mathf.Clamp(difficulty, minMaxDifficulty.x, minMaxDifficulty.y);

            CalculateWeight(difficulty);

            creatureData = null;

            if (probabilityTotalWeight == 0)
            {
                return false;
            }

            pickCount++;
            if (pickCount > maxPickCount)
            {
                Debug.LogError(id + " | Max pick count reached! ( " + maxPickCount + ") Please check if there is any loop in the drop table...");
                return false;
            }
            float pickedNumber = randomGen != null ? (float)randomGen.NextDouble() * probabilityTotalWeight: UnityEngine.Random.Range(0, probabilityTotalWeight);

            // Find an item whose range contains pickedNumber
            foreach (Drop drop in drops)
            {
                // If the picked number matches the item's range, return item
                if (pickedNumber > drop.probabilityRangeFrom && pickedNumber < drop.probabilityRangeTo)
                {
                    if (drop.referenceID == null || drop.referenceID == "") return false;

                    if (drop.reference == Drop.Reference.Creature)
                    {
                        if (drop.overrideContainer || drop.overrideBrain || drop.overrideFaction)
                        {
                            creatureData = Catalog.GetData<CreatureData>(drop.referenceID)?.Clone() as CreatureData;
                            if (creatureData != null)
                            {
                                if (drop.overrideBrain) creatureData.brainId = drop.overrideBrainID;
                                if (drop.overrideContainer) creatureData.containerID = drop.overrideContainerID;
                                if (drop.overrideFaction) creatureData.factionId = drop.factionID;
                                return true;
                            }
                            else
                            {
                                Debug.LogError("Can't find creatureID " + drop.referenceID);
                                return false;
                            }
                        }
                        else
                        {
                            creatureData = Catalog.GetData<CreatureData>(drop.referenceID);
                            if (creatureData != null)
                            {
                                return true;
                            }
                            else
                            {
                                Debug.LogError("Can't find creatureID " + drop.referenceID);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        CreatureTable creatureTable = Catalog.GetData<CreatureTable>(drop.referenceID);
                        if (creatureTable != null)
                        {
                            if (drop.overrideContainer || drop.overrideBrain || drop.overrideFaction)
                            {
                                if (creatureTable.TryPick(difficulty, pickCount, out CreatureData tmpCreatureData, randomGen))
                                {
                                    creatureData = tmpCreatureData.Clone() as CreatureData;
                                    if (drop.overrideBrain) creatureData.brainId = drop.overrideBrainID;
                                    if (drop.overrideContainer) creatureData.containerID = drop.overrideContainerID;
                                    if (drop.overrideFaction) creatureData.factionId = drop.factionID;
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (creatureTable.TryPick(difficulty, pickCount, out creatureData, randomGen))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("Can't find creatureTable " + drop.referenceID);
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
