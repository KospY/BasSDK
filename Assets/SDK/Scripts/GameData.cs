using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;
using System.Linq;
using UnityEngine.Rendering;

namespace ThunderRoad
{
    [Serializable]
    public class GameData
    {
        public static string savesFolder = "Default";

        public bool overrideSaveCharacters = false;
        public bool overrideSaveOptions = false;
        public bool disableCharacterCustomization = false;
        public bool disableCharacterDeletion = false;

        public bool showControlPanelOnStart = false;
        public bool arcadeControlPanel;

        public bool fullscreen = false;
        public int screenWidth = 1920;
        public int screenHeight = 1080;
        public bool debugDungeonCaptureMemoryUsage = false;
        public bool debugDungeonWaitPress = false;
        public bool dungeonStaticBatching = true;

        public bool ShouldSerializearcadeControlPanel()
        {
            return arcadeControlPanel;
        }

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllLevelID")]
#endif
        public string levelStart;
        public string levelStartModeName;
        public Dictionary<string, string> levelStartOptions;

        public string defaultPlayerMaleCreatureID = "PlayerDefaultMale";
        public string defaultPlayerFemaleCreatureID = "PlayerDefaultFemale";


#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string spellWheelEffectId;
        [NonSerialized]

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllHandPoseID")]
#endif
        public string SpellWheelOpenHandPoseId;
        [NonSerialized]
        public HandPoseData SpellWheelOpenHandPoseData;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllHandPoseID")]
#endif
        public string SpellWheelCloseHandPoseId;
        [NonSerialized]
        public HandPoseData SpellWheelCloseHandPoseData;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllHandPoseID")]
#endif
        public string uiPointingCloseHandPoseId;
        [NonSerialized]
        public HandPoseData uiPointingCloseHandPoseData;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string itemWheelEffectId;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string defaultWheelOrbEffectId;
        public float fingerSpeed = 6f;

        public string defaultPostProcessAddress;
#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<PostProcessProfile> postProcessProfiles = new List<PostProcessProfile>();

        [Serializable]
        public class PostProcessProfile
        {
            public string id;
            public string address;
            [NonSerialized]
            public VolumeProfile volumeProfile;
        }

        public string heartBeatLoopAddress;

        public float deathSlowMoRatio = 0.05f;
        public float deathSlowMoDuration = 0.25f;
        public AnimationCurve deathSlowMoEnterCurve;
        public AnimationCurve deathSlowMoExitCurve;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string deathEffectId;
        public PlatformParameters pc = new PlatformParameters();
        public PlatformParameters android = new PlatformParameters();
        [NonSerialized]
        public PlatformParameters platformParameters;

        [Serializable]
        public class PlatformParameters
        {
            public int maxWaveAlive = 20;
            public int maxRoomNpc = 10;
            public int itemSpawnerMaxItems = 100;
            public int itemSpawnerDelay = 0;
            public float mirrorQuality = 1f;

            public float aiCycleMinSpeed = 0.1f;
            public float aiCycleMaxSpeed = 2;

            public int poolingAudioCount = 20;
            public int poolingVfxCount = 10;
            public int poolingShaderCount = 100;
            public int poolingRevealCount = 100;
            public int poolingPaintCount = 100;
            public int poolingMeshCount = 100;
            public int poolingLightCount = 100;
            public int poolingEffectInstanceCount = 600;

            public bool enableEffectAudio = true;
            public bool enableEffectVfx = true;
            public bool enableEffectParticle = true;
            public bool enableEffectDecal = true;
            public bool enableEffectMesh = true;
            public bool enableEffectReveal = true;
            public bool enableEffectPaint = true;
            public bool enableEffectLight = true;
        }

        [Serializable]
        public class Category
        {
            public string name = "Unknown";
            public string group;
            public string iconAddress;
            [NonSerialized]
            public IResourceLocation iconLocation;
            [NonSerialized]
            public int itemsCount;
        }

        /// <summary>
        /// Used to define category groups
        /// They wrap item categories and provides essential information for the inventory, like slots or sounds.
        /// </summary>
        [Serializable]
        public class CategoryGroup
        {
            // Localized display name
            private string displayName;
            [NonSerialized] public Category[] LinkedCategories;

            /// <summary>
            /// Localized name of the current category group
            /// </summary>
            /// <returns>The localized name if it exists and if the Localization manager is initialized.
            /// Returns its id otherwise</returns>
            public string DisplayName()
            {
                if (string.IsNullOrEmpty(displayName))
                    InitCachedCategoryGroupLanguageValue(null);

                return displayName;
            }

            /// <summary>
            /// Is the description invalid ?
            /// It checks against the static invalid category group ID
            /// </summary>
            /// <returns>True if the id match or is empty, false otherwise</returns>
            public bool IsInvalid() => InvalidCategoryGroup.id == id ||
                                     string.IsNullOrEmpty(id);

            /// <summary>
            /// Checks if the category with the given name/path belong to this linked categories set
            /// </summary>
            /// <param name="categoryPath">The name of the category to check for</param>
            /// <returns>True if the given name belong to a linked category, false otherwise</returns>
            public bool IsCategoryPathMatching(string categoryPath)
            {
                if (LinkedCategories == null) return false;

                for (int i = 0; i < LinkedCategories.Length; i++)
                {
                    if (LinkedCategories[i].name == categoryPath)
                        return true;
                }
                return false;
            }

            // Identifier used for comparison, hashcode and localization display
            public string id;

            // address of the icon
            public string iconAddress;

            // Is the group used as an inventory category ?
            public bool useInInventory;

            // If set to true, this category will always be displayed, even if empty
            public bool displayWhenEmpty;

            // Order to sort by in the inventory (only used if useInInventory = true)
            public int inventorySortingOrder;

            // Max number of slots (only used if useInInventory == true)
            public int inventoryMaxSlotAmount;

            // Should we use an item quantity limit (not only stack slots)
            public bool limitInventoryMaxItemAmount;

            // Max number of items (only used if useInInventory == true and limitInventoryMaxItemAmount == true)
            public int inventoryMaxItemAmount;

            public AudioContainerAddressAndVolume hoverSounds;
            public AudioContainerAddressAndVolume selectSounds;
            public AudioContainerAddressAndVolume itemHoverSounds;
            public AudioContainerAddressAndVolume itemSelectSounds;
            public AudioContainerAddressAndVolume itemStoreSounds;

            public CategoryGroup(string id,
                bool useInInventory,
                int inventorySortingOrder,
                int inventoryMaxSlotAmount,
                string iconAddress,
                AudioContainerAddressAndVolume hoverSounds,
                AudioContainerAddressAndVolume selectSounds,
                AudioContainerAddressAndVolume itemStoreSounds,
                AudioContainerAddressAndVolume itemHoverSounds,
                AudioContainerAddressAndVolume itemSelectSounds)
            {
                this.id = id;
                this.useInInventory = useInInventory;
                this.inventorySortingOrder = inventorySortingOrder;
                this.inventoryMaxSlotAmount = inventoryMaxSlotAmount;
                this.iconAddress = iconAddress;

                // The next parameters will get cached at initialization
                displayName = null;
                LinkedCategories = null;
                this.hoverSounds = hoverSounds;
                this.selectSounds = selectSounds;
                this.itemStoreSounds = itemStoreSounds;
                this.itemHoverSounds = itemHoverSounds;
                this.itemSelectSounds = itemSelectSounds;
            }

            /// <summary>
            /// Inits the category's display name to use localization
            /// </summary>
            /// <param name="_">Unused language id</param>
            public void InitCachedCategoryGroupLanguageValue(string _)
            {
            }

        }

        /// <summary>
        /// Class to match an audio container to a volume in DB, to be easily tweak-able in catalog
        /// </summary>
        [Serializable]
        public class AudioContainerAddressAndVolume
        {
            /// <summary>
            /// Address of the container to load
            /// </summary>
            public string audioContainerAddress;

            /// <summary>
            /// Volume, in DB, of the sounds that will play
            /// </summary>
            public float volumeDb;

            /// <summary>
            /// Cached container, loaded asynchronously from the catalog  
            /// </summary>
            [NonSerialized] public AudioContainer Container;

        }

        /// <summary>
        /// Used to hold default category groups sounds, used as a Fallback when they are not defined in the groups
        /// </summary>
        [Serializable]
        public class CategoryGroupAudioContainersAddresses
        {
            public AudioContainerAddressAndVolume defaultHoverSounds;
            public AudioContainerAddressAndVolume defaultSelectSounds;
            public AudioContainerAddressAndVolume defaultItemStoreSounds;
            public AudioContainerAddressAndVolume defaultItemHoverSounds;
            public AudioContainerAddressAndVolume defaultItemSelectSounds;
            public AudioContainerAddressAndVolume itemStoredErrorSounds;

        }

        /// <summary>
        /// Static invalid category description, used as a value to check for not allowed items
        /// </summary>
        [NonSerialized]
        public static CategoryGroup InvalidCategoryGroup =
            new CategoryGroup("INVALID_CATEGORY_GROUP",
                false,
                -999,
                -1,
                null,
                new AudioContainerAddressAndVolume(),
                new AudioContainerAddressAndVolume(),
                new AudioContainerAddressAndVolume(),
                new AudioContainerAddressAndVolume(),
                new AudioContainerAddressAndVolume());

        public Experience experience;
        public Honor honor;
        public Score score;

        [Serializable]
        public class Score
        {
            public int hitReceived;
            public int waveReached;
        }

        [Serializable]
        public class Experience
        {
            public Actions actions = new Actions();
            public Modifiers modifiers = new Modifiers();

            public int aiDifficulty;
            public int apparelTier;
            public int weaponTier;
            public int shieldTier;

            public int historyMaxSameAction;
            public int historyMaxSameActionAndModifier;
            public int historyMultiKill;
            public float historyMinTime = 2;
            public float historyMaxTime = 30;
            public int historyMaxCount = 20;
        }

        [Serializable]
        public class Honor
        {
            public int max = 1000;
            public Actions actions = new Actions();
            public Modifiers modifiers = new Modifiers();
        }

        [Serializable]
        public class Actions
        {
            public int decapitation;
            public int dismemberment;
            public int magicDismemberment;
            public int cutThroat;
            public int slashHeadOrNeck;
            public int pierceHeadOrNeck;
            public int pierceTorso;
            public int bluntHead;
            public int punchOrKick;
            public int ragdollHit;
            public int staticBlunt;
            public int staticSlash;
            public int staticPierce;

            public int electrocuted;
            public int burned;
            public int frozen;
            public int drained;
            public int poisoned;

            public int throwDecapitation;
            public int throwDismemberment;
            public int throwSlashHeadOrNeck;
            public int throwPierceHeadOrNeck;
            public int throwPierce;
            public int throwBlunt;
        }

        [Serializable]
        public class Modifiers
        {
            public int flying;
            public int grabbed;
            public int electrocuted;
            public int onGround;
            public int unarmed;
            public int dead;
        }

        public List<Faction> factions;

        [Serializable]
        public class Faction
        {
            public int id;
            public string name;
            public AttackBehaviour attackBehaviour = AttackBehaviour.Default;
            public enum AttackBehaviour
            {
                Default,
                Ignored,
                Passive,
                Agressive,
            }
        }

        [Serializable]
        public class SkillCluster
        {
#if ODIN_INSPECTOR
            [FoldoutGroup("Cluster", false)]
#endif
            public int id;
#if ODIN_INSPECTOR
            [FoldoutGroup("Cluster", false)]
#endif
            public int depth;
#if ODIN_INSPECTOR
            [FoldoutGroup("Cluster", false)]
#endif
            public List<Skill> skills;
#if ODIN_INSPECTOR
            [FoldoutGroup("Cluster", false)]
#endif
            public int[] dependancies;
#if ODIN_INSPECTOR
            [FoldoutGroup("Cluster", false)]
            [ValueDropdown("GetAllEffectID")]
#endif
            public string effectId;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Catalog.Category.Effect);
            }
#endif
        }

        [Serializable]
        public class Skill
        {
            public int id;
            public string name;
            public string description;
            public bool unlocked = false;
            public int[] dependancies;
            public bool multiLevel = false;
#if ODIN_INSPECTOR
            [ShowIf("multiLevel")]
#endif
            public int levels = 3;
#if ODIN_INSPECTOR
            [ShowIf("multiLevel"), ValueDropdown("GetAllEffectID")]
#endif
            public List<string> levelEffects;
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllEffectID")]
#endif
            public string effectId;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Catalog.Category.Effect);
            }
#endif
        }

#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<Category> categories;
#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<CategoryGroup> categoryGroups;
        public List<string> holderSlots;
        public List<SkillCluster> skillTree;
        public CategoryGroupAudioContainersAddresses categoryGroupDefaultSounds;


        public ScreenshotFormat screenshotFormat = ScreenshotFormat.JPG;
        public int screenshotJpegQuality = 75;

        public enum ScreenshotFormat
        {
            JPG,
            PNG,
            All,
        }

        public Locomotion.GroundDetection locomotionGroundDetection = Locomotion.GroundDetection.Collision;
        public string menuPrefabAddress;
        public string playerHighlighterAddress;
        public int itemIconResolution = 256;
        public float throwVelocity = 3;
        public float throwMultiplier = 2;
#if ODIN_INSPECTOR
        [MinMaxSlider(0f, 100f, ShowFields = true)]
#endif
        public Vector2 collisionEnterVelocityRange = new Vector2(1f, 7f);
#if ODIN_INSPECTOR
        [MinMaxSlider(0f, 20f, ShowFields = true)]
#endif
        public Vector2 collisionStayVelocityRange = new Vector2(0.5f, 3f);
        public Vector2 penetrationVelocityEffectRange = new Vector2(0f, 1f);
        public int maxObjectCollision = 10;
        public int maxFramesToKeepCollision = 5;
        public int maxCollisionStayCheckPerFrame = 100;
        public int cleanDeadLastInteractionDelay = 7;
        public int cleanObjectsLastInteractionDelay = 7;
        public int cleanFarObjectsSqrDistance = 62500; //250m squared
        public float motionBlurIntensity = 0.35f;

        public string errorAudioGroupAddress;
        [NonSerialized]
        public AudioContainer errorAudioGroup;

        public CollisionDetection collisionDetection;
        [Serializable]
        public class CollisionDetection
        {
            public CollisionDetectionMode grabbed = CollisionDetectionMode.ContinuousSpeculative;
            public CollisionDetectionMode dropped = CollisionDetectionMode.Discrete;
            public CollisionDetectionMode throwed = CollisionDetectionMode.ContinuousDynamic;
            public CollisionDetectionMode telekinesis = CollisionDetectionMode.ContinuousSpeculative;
            public CollisionDetectionMode npcAttack = CollisionDetectionMode.ContinuousSpeculative;
            public CollisionDetectionMode kick = CollisionDetectionMode.ContinuousSpeculative;
        }
        public Haptics haptics;
        [Serializable]
        public class Haptics
        {
            [Header("Clips")]
            public HapticClip hit;
            public HapticClip bowShoot;
            public HapticClip telekinesisThrow;
            public HapticClip spellSelected;
            [Header("Telekinesis")]
            public Vector2 telekinesisIntensity = new Vector2(0.05f, 0.2f);
            public float telekinesisViveCosmosMultiplier = 0.1f;
            public Vector2 telekinesisPeriod = new Vector2(0.06f, 0.03f);
            public float telekinesisFocus = 0.1f;
            [Header("Bow draw")]
            public float bowDrawPeriod = 0.02f;
            public float bowDrawIntensity = 0.8f;
            [Header("Rubbing")]
            public float rubbingPeriod = 0.02f;
            public float rubbingIntensity = 0.4f;
            [Header("Penetration")]
            public float penetrationPeriod = 0.01f;
            public float penetrationIntensity = 0.6f;
        }

        [Serializable]
        public class HapticClip
        {
            public HapticClip()
            {
                this.curveIntensity = AnimationCurve.Linear(0, 0.05f, 1, 0.2f);
                this.curvePeriod = AnimationCurve.Linear(0, 0.05f, 1, 0.05f);
                this.duration = 1f;
            }

            public HapticClip(AnimationCurve curveIntensity)
            {
                this.curveIntensity = curveIntensity;
                this.curvePeriod = AnimationCurve.Linear(0, 0.05f, 1, 0.05f);
                this.duration = 1f;
            }

            public HapticClip(AnimationCurve curveIntensity, float amplitude, float duration)
            {
                this.curveIntensity = curveIntensity;
                this.curvePeriod = AnimationCurve.Linear(0, 0.05f, 1, 0.05f);
                this.duration = duration;
            }

            public AnimationCurve curveIntensity;
            public AnimationCurve curvePeriod;
            public float duration;
        }


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetCategories()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            foreach (Category category in categories)
            {
                dropdownList.Add(new ValueDropdownItem<string>(category.name, category.name));
            }
            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetCategoryGroups()
        {
            List<ValueDropdownItem<string>> categoryGoups = new List<ValueDropdownItem<string>>();
            foreach (Category category in categories)
            {
                if (categoryGoups.Contains(new ValueDropdownItem<string>(category.group, category.group))) continue;

                categoryGoups.Add(new ValueDropdownItem<string>(category.group, category.group));
            }
            return categoryGoups;
        }
#endif

        public Faction GetFaction(int factionId)
        {
            foreach (Faction faction in factions)
            {
                if (faction.id == factionId)
                {
                    return faction;
                }
            }
            return null;
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<int>> GetFactions()
        {
            List<ValueDropdownItem<int>> dropdownList = new List<ValueDropdownItem<int>>();
            foreach (Faction faction in factions)
            {
                dropdownList.Add(new ValueDropdownItem<int>(faction.name, faction.id));
            }
            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetAllLevelID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Level);
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Effect);
        }

        public List<ValueDropdownItem<string>> GetAllHandPoseID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.HandPose);
        }
#endif

        public Category GetCategoryUnknown()
        {
            return categories.Find(sc => sc.name == "Unknown");
        }

        public Category GetCategory(string name)
        {
            Category categoryReturn = null;
            foreach (Category category in categories)
            {
                if (category.name == name)
                {
                    categoryReturn = category;
                    break;
                }
            }
            return categoryReturn != null ? categoryReturn : GetCategoryUnknown();
        }

        /// <summary>
        /// Returns the corresponding category group
        /// </summary>
        /// <param name="id">Id of the searched category group</param>
        /// <returns>the category with the same id if found, otherwise static invalid</returns>
        public CategoryGroup GetCategoryGroup(string id)
        {
            for (var i = 0; i < categoryGroups.Count; i++)
            {
                var categoryGroup = categoryGroups[i];
                if (categoryGroup.id == id)
                    return categoryGroup;
            }

            return InvalidCategoryGroup;
        }

        /// <summary>
        /// Caches and returns all category groups allowed in inventory
        /// </summary>
        /// <returns>All the category groups with "useInInventory" set to true</returns>
        public CategoryGroup[] GetInventoryCategoryGroups()
        {
            return System.Array.Empty<CategoryGroup>();
        }

        public void OnCatalogRefresh()
        {
        }


        /// <summary>
        /// Method to update data values when the language changes
        /// </summary>
        /// <param name="language">Language key</param>
        public void OnLanguageChanged(string language)
        {
            UpdateCategoryGroupsDisplayNames(language);
        }

        /// <summary>
        /// Update display names of category groups with localization
        /// </summary>
        /// <param name="language">Current language</param>
        public void UpdateCategoryGroupsDisplayNames(string language)
        {
            for (int i = 0; i < categoryGroups.Count; i++)
                categoryGroups[i].InitCachedCategoryGroupLanguageValue(language);
        }

    }
}