using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using ThunderRoad.Modules;
using UnityEngine.ResourceManagement.ResourceLocations;
using Newtonsoft.Json;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class GameModeData : CatalogData
    {
        public string name = "Unknown";
        public string nameLocalizationId = "";
        [Multiline]
        public string description = "";
        public string descriptionLocalizationId = "";
        public string warning = "";
        public string warningId = "";
        public string iconAddress;
        public int order = int.MaxValue;
        [NonSerialized]
        public IResourceLocation iconLocation;

        public State state = State.Show;

        [NonSerialized]
        public Texture2D iconLoaded;

        public enum State
        {
            Show,
            Disabled,
            Hidden,
        }


        public override IEnumerator OnCatalogRefreshCoroutine()
        {
            yield return Catalog.LoadLocationCoroutine<Texture2D>(iconAddress, location => iconLocation = location, id);
        }

        public bool refreshMapOnlyWhenPlayerNear = false;
        
        public bool saveGameOnSkillUnlock = false;
        public bool allowLevelSelection = true;
        public bool allowRefundCoreSkills = false;
        
        public bool defaultPlayerInvincibility = false;
        public bool defaultPlayerFallDamage = false;
        public bool defaultPlayerInfiniteFocus = false;
        public bool defaultPlayerFastCast = false;
        public bool defaultInfiniteImbue = false; 
        public bool defaultInfiniteSupply = false;
        public bool defaultClimbFree = false;
        public bool defaultEasyDismemberment = false;
        public bool defaultInfiniteArrows = false;
        public bool defaultArmorDetection = true;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllLevelID))]
#endif
        public string levelHome;
        public string levelHomeModeName;
        public string levelHomeTravelSpawnerId;

        public bool hasTutorial;
        public string tutorialPlayerSpawnerId;

        /// <summary>
        /// At character creation the player can select the start inventory from this list
        /// If no choice thhen just make a list of 1 element withh the default values
        /// This can not be null or empty
        /// </summary>
        public List<InventoryStart> playerInventoryStart;

#if ODIN_INSPECTOR
        [ShowInInspector]
        [ValueDropdown(nameof(GetAllValueTypes))]
#endif
        public string mainCurrency = "Gold";
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public List<string> otherCurrencies = new List<string>();

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public List<GameModeModule> modules = new List<GameModeModule>();

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public List<string> characterStatPrefabAddresses = new List<string>();
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        /// <summary>
        /// Difficulty options that are available for this gamemode to customise the difficulty (they have value from preset)
        /// </summary>
        public List<OptionBase> difficultyOptions = new List<OptionBase>();

#if ODIN_INSPECTOR
        [ShowInInspector]
        [ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true, ShowPaging = false), LabelText("Difficulty Presets")]
#endif
        /// <summary>
        /// Presets of difficulty options for this gamemode to customise the difficulty
        /// </summary>
        public List<DifficultyPreset> difficultyPresets = new List<DifficultyPreset>();
        public int defaultDifficultyPreset = 0;

        [Header("WarningMessage")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string warningMessageGroupId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string warningMessageTextId;


#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCheats))]
#endif
        [Header("MenuOption")]
        public List<string> cheatsAllowed;

#if ODIN_INSPECTOR
#if UNITY_EDITOR
        [Button]
        public void AddDifficultyPreset()
        {
            DifficultyPreset difficultyPreset = new DifficultyPreset() { gameModeData = this };
            difficultyPreset.Refresh();
            difficultyPresets.Add(difficultyPreset);
        }
#endif

        public List<ValueDropdownItem<string>> GetAllValueTypes()
        {
            return Catalog.gameData.GetAllValueTypes();
        }

        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Category.Container);
        }

        public List<ValueDropdownItem<string>> GetAllLevelID()
        {
            return Catalog.GetDropdownAllID(Category.Level);
        }
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(warningMessageGroupId);
        }

        public List<ValueDropdownItem<string>> GetAllCheats()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("Cheats", "Cheats"));
            dropdownList.Add(new ValueDropdownItem<string>("Skills", "Skills"));
            dropdownList.Add(new ValueDropdownItem<string>("Spawn", "Spawn"));
            dropdownList.Add(new ValueDropdownItem<string>("Progression", "Progression"));

            return dropdownList;
        }
#endif

        /// <summary>
        /// This will return all difficulty options set in the data class, and also allow modders to override the options in code
        /// </summary>
        /// <returns></returns>
        public virtual List<OptionBase> GetDifficultyOptions()
        {
            return difficultyOptions;
        }

        public virtual bool TryGetSavedOption<T>(out T option) where T : OptionBase
        {
            option = null;
            List<OptionBase> options = GetDifficultyOptions();
            int count = options.Count;
            for (int i = 0; i < count; i++)
            {
                if(options[i] is T temp)
                {
                    option = temp;
                    return true;
                }
            }

            return false;
        }

        [JsonIgnore]
        public virtual bool TriggerWarningMessage => false;

        public virtual void OnWarningMessagegDoNotShowAgain() { }

    }
}