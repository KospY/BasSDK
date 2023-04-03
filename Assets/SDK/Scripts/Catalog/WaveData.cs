using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class WaveData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string category = "Misc";

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string localizationId;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string title = "Unknown";

#if ODIN_INSPECTOR
        [BoxGroup("General"), Multiline]
#endif
        public string description;

        public enum LoopBehavior
        {
            NoLoop,
            LoopSeamless,
            Loop,
        }

        [JsonProperty("loop")]
        private bool loopBehaviorSetter
        {
            set
            {
                if (value)
                {
                    loopBehavior = LoopBehavior.LoopSeamless;
                }
                else
                {
                    loopBehavior = LoopBehavior.NoLoop;
                }
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public LoopBehavior loopBehavior;

        // For compatibility (to remove)
        [JsonProperty("maxAlive")]
        private int maxAliveAlternateSetter
        {
            set { totalMaxAlive = value; }
        }
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public int totalMaxAlive = 5;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool alwaysAvailable;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public List<string> waveSelectors;

#if ODIN_INSPECTOR
        [BoxGroup("Modifiers")]
#endif
        public float playerHealthMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Modifiers")]
#endif
        public float enemiesHealthMultiplier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Info"), NonSerialized, ShowInInspector, ReadOnly]
#endif
        public int minTotalCount;
#if ODIN_INSPECTOR
        [BoxGroup("Info"), NonSerialized, ShowInInspector, ReadOnly]
#endif
        public float maxTotalCount;
#if ODIN_INSPECTOR
        [ListDrawerSettings(IsReadOnly = true)]
#endif
        public List<WaveFaction> factions = new List<WaveFaction>();

#if ODIN_INSPECTOR
        [ListDrawerSettings(OnBeginListElementGUI = "OnBeginCheckGroup", OnEndListElementGUI = "OnEndCheckGroup")]
#endif
        public List<Group> groups;

        void OnBeginCheckGroup(int index)
        {
            groups[index].index = index;

            if (string.IsNullOrEmpty(groups[index].referenceID))
            {
                GUI.color = Color.red;
            }
            else
            {
                switch (groups[index].reference)
                {
                    case Group.Reference.Creature when Catalog.GetData<CreatureData>(groups[index].referenceID, false) == null:
                    case Group.Reference.Table when Catalog.GetData<CreatureTable>(groups[index].referenceID, false) == null:
                        GUI.color = Color.red;
                        break;
                }
            }
        }

        void OnEndCheckGroup(int index)
        {
            GUI.color = Color.white;
        }

        public override int GetCurrentVersion()
        {
            return 2;
        }

        public override CatalogData Clone()
        {
            WaveData clone = MemberwiseClone() as WaveData;
            clone.groups = clone.groups.Select(item => (Group)item.Clone()).ToList();
            return clone;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            minTotalCount = 0;
            maxTotalCount = 0;
            Dictionary<int, WaveFaction> foundFactions = new Dictionary<int, WaveFaction>();
            foreach (WaveFaction faction in factions)
            {
                faction.factionName = Catalog.gameData.GetFaction(faction.factionID).name;
                foundFactions.Add(faction.factionID, faction);
            }
            List<int> factionsInWave = new List<int>();
            List<WaveFaction> newFactions = new List<WaveFaction>();
            if (groups != null)
            {
                for (var i = 0; i < groups.Count; i++)
                {
                    groups[i].waveData = this;
                    minTotalCount += groups[i].minMaxCount.x;
                    maxTotalCount += groups[i].minMaxCount.y;
                    groups[i].OnCatalogRefresh();
                    groups[i].GetFactions(factionsInWave);
                }
            }
            factionsInWave.Sort();
            if (factionsInWave.Count > 0)
            {
                foreach (int factionID in factionsInWave)
                {
                    if (foundFactions.TryGetValue(factionID, out WaveFaction existingFaction))
                    {
                        newFactions.Add(existingFaction);
                        foundFactions.Remove(factionID);
                    }
                    else
                    {
                        newFactions.Add(new WaveFaction(Catalog.gameData.GetFaction(factionID), Mathf.RoundToInt(totalMaxAlive / factionsInWave.Count)));
                    }
                }
            }
            factions = newFactions;
        }


        [Serializable]
        public class WaveFaction
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Split")]
            [BoxGroup("Split/Faction")]
            [HorizontalGroup("Split/Faction/Group"), ReadOnly, HideLabel]
#endif
            public int factionID;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Faction/Group"), NonSerialized, ShowInInspector, ReadOnly, HideLabel]
#endif
            public string factionName;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Faction health multiplier"), HideLabel]
#endif
            public float factionHealthMultiplier = 1f;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Faction max alive"), HideLabel]
#endif
            public int factionMaxAlive = 5;

            public WaveFaction() { }

            public WaveFaction(GameData.Faction faction, int factionDefaultMax)
            {
                factionID = faction.id;
                factionName = faction.name;
                factionMaxAlive = factionDefaultMax;
            }
        }

        public class SpawnData
        {
            public CreatureData data;
            public Group spawnGroup;

            public SpawnData(CreatureData d, Group g)
            {
                data = d;
                spawnGroup = g;
            }
        }

        [Serializable]
        public class Group
        {
            [NonSerialized]
            public WaveData waveData;

#if ODIN_INSPECTOR
            [HorizontalGroup("Split", MinWidth = 40)]
            [BoxGroup("Split/Index"), ShowInInspector, ReadOnly, HideLabel]
#endif
            [NonSerialized]
            public int index;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Reference")]
            [HorizontalGroup("Split/Reference/Group", MinWidth = 150), HideLabel, EnumToggleButtons]
#endif
            public Reference reference = Reference.Creature;

#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Reference/Group", MinWidth = 150), ValueDropdown("GetAllCreatureOrTableID"), HideLabel, OnValueChanged("RefreshInfo")]
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

#if ODIN_INSPECTOR
            [BoxGroup("Split/Override Faction")]
            [HorizontalGroup("Split/Override Faction/Group"), HideLabel, OnValueChanged("RefreshInfo")]
#endif
            public bool overrideFaction;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Override Faction/Group")]
            [EnableIf("overrideFaction", true), ValueDropdown("GetAllFactionID"), HideLabel, OnValueChanged("RefreshInfo")]
#endif
            public int factionID = 0;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Override Container")]
            [HorizontalGroup("Split/Override Container/Group"), HideLabel]
#endif
            public bool overrideContainer;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Override Container/Group"), EnableIf("overrideContainer", true), ValueDropdown("GetAllContainerID"), HideLabel]
#endif
            public string overrideContainerID;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Override Brain")]
            [HorizontalGroup("Split/Override Brain/Group"), HideLabel]
#endif
            public bool overrideBrain;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Override Brain/Group"), EnableIf("overrideBrain", true), ValueDropdown("GetAllBrainID"), HideLabel]
#endif
            public string overrideBrainID;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Override max melee")]
            [HorizontalGroup("Split/Override max melee/Group"), HideLabel]
#endif
            public bool overrideMaxMelee;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Override max melee/Group"), HideLabel, EnableIf("overrideMaxMelee", true)]
#endif
            public int overrideMaxMeleeCount;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Group health mult"), HideLabel]
#endif
            public float groupHealthMultiplier = 1f;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Spawning"), HideLabel]
            [MinMaxSlider(1, 5, showFields: true), OnValueChanged("RefreshInfo")]
#endif
            public Vector2Int minMaxCount = new Vector2Int(1, 1);

#if ODIN_INSPECTOR
            [BoxGroup("Split/Set spawn point")]
            [HorizontalGroup("Split/Set spawn point/Group", MaxWidth = 10), HideLabel, ShowInInspector, NonSerialized, OnValueChanged("SetRandSpawn")]
#endif
            public bool set = false;
#if ODIN_INSPECTOR
            [HorizontalGroup("Split/Set spawn point/Group", MaxWidth = 30), HideLabel, EnableIf("set")]
#endif
            public int spawnPointIndex = -1;

            // For compatibility (to remove)
            [JsonProperty("conditionStepIndex")]
            private int conditionAlternateSetter
            {
                set { prereqGroupIndex = value; }
            }
#if ODIN_INSPECTOR
            [BoxGroup("Split/Prereq group index"), HideLabel]
#endif
            public int prereqGroupIndex = -1;
            [NonSerialized]
            public Group prereqGroup;

            // For compatibility (to remove)
            [JsonProperty("conditionThreshold")]
            private int conditionThreshAlternateSetter
            {
                set { prereqMaxRemainingAlive = value; }
            }
#if ODIN_INSPECTOR
            [BoxGroup("Split/Prereq group max alive"), PropertyRange(0, "GetMaxThreshold"), HideLabel]
#endif
            public int prereqMaxRemainingAlive = 0;

            public enum Reference
            {
                Creature,
                Table,
            }
            public Group Clone()
            {
                return MemberwiseClone() as Group;
            }

            public int GetMaxThreshold()
            {
                if (waveData != null)
                {
                    Group conditionStep = waveData.groups.ElementAtOrDefault(prereqGroupIndex);
                    if (conditionStep != null)
                    {
                        return conditionStep.minMaxCount.x - 1;
                    }
                }
                return 4;
            }

            public void SetRandSpawn()
            {
                if (!set) spawnPointIndex = -1;
                else
                {
                    if (spawnPointIndex == -1) spawnPointIndex = 0;
                }
            }

            private void RefreshInfo()
            {
                waveData?.RefreshInfo();
            }

            public void GetFactions(List<int> existing)
            {
                if (overrideFaction)
                {
                    if (!existing.Contains(factionID)) existing.Add(factionID);
                }
                else
                {
                    if (referenceID == "None") return;
                    if (reference == Reference.Creature)
                    {
                        CreatureData creatureData = Catalog.GetData<CreatureData>(referenceID);
                        if (!existing.Contains(creatureData.factionId)) existing.Add(creatureData.factionId);
                    }
                    else
                    {
                        CreatureTable creatureTable = Catalog.GetData<CreatureTable>(referenceID);
                        foreach (int factionID in creatureTable.GetFactionIDs())
                        {
                            if (!existing.Contains(factionID)) existing.Add(factionID);
                        }
                    }
                }
            }

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllCreatureOrTableID()
            {
                return Catalog.GetDropdownAllID(reference == Reference.Creature ? Category.Creature : Category.CreatureTable);
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

            public void OnCatalogRefresh()
            {
                if (string.IsNullOrEmpty(referenceID))
                {
                    if (reference == Reference.Creature) referenceID = creatureID;
                    if (reference == Reference.Table) referenceID = creatureTableID;
                }
            }

        }
    }
}