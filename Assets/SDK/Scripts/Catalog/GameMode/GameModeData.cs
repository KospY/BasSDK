using UnityEngine;
using System.Collections.Generic;
using System;
using ThunderRoad.Modules;
using UnityEngine.ResourceManagement.ResourceLocations;
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

        public bool resetSpells = true;
        public bool resetApparels = true;

        public bool allowMenuOptions = true;
        public bool allowMenuSandbox = false;
        public bool allowLevelSelection = true;

        public bool defaultPlayerInvincibility = false;
        public bool defaultPlayerFallDamage = false;
        public bool defaultPlayerInfiniteMana = false;
        public bool defaultPlayerInfiniteFocus = false;
        public bool defaultInfiniteImbue = false;
        public bool defaultInfiniteArrows = false;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllLevelID")]
#endif
        public string levelHome;
        public string levelHomeModeName;

        public bool tutorialEnabled = false;
        public string tutorialPlayerSpawnerId = "tutorial";
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllContainerID")]
#endif
        public string tutorialContainerId = "PlayerTutorial";

        public string defaultPlayerHomeContainerID = "PlayerHomeDefault";

#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        public List<GameModeModule> modules = new List<GameModeModule>();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Category.Container);
        }

        public List<ValueDropdownItem<string>> GetAllLevelID()
        {
            return Catalog.GetDropdownAllID(Category.Level);
        }
#endif

    }
}