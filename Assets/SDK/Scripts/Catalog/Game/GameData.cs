using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
        public bool dungeonStaticBatching = true;

        [NonSerialized]
        public string sourceFolders;

        public string GetLatestOverrideFolder()
        {
            return sourceFolders.Split('+').Last();
        }


        public string defaultPlayerMaleCreatureID = "PlayerDefaultMale";
        public string defaultPlayerFemaleCreatureID = "PlayerDefaultFemale";


#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string spellWheelEffectId;
        [NonSerialized]

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string SpellWheelOpenHandPoseId;
        [NonSerialized]
        public HandPoseData SpellWheelOpenHandPoseData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string SpellWheelCloseHandPoseId;
        [NonSerialized]
        public HandPoseData SpellWheelCloseHandPoseData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string uiPointingCloseHandPoseId;
        [NonSerialized]
        public HandPoseData uiPointingCloseHandPoseData;


#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string skillOrbInHandEffectId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string defaultWheelOrbEffectId;
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

        [ColorUsage(true, true)]
        public Color[] tierColors;

        public string heartBeatLoopAddress;
        public string deathEffectId = "Death";
        public float deathSlowMoRatio = 0.05f;
        public float deathSlowMoDuration = 0.25f;
        public AnimationCurve deathSlowMoEnterCurve;
        public AnimationCurve deathSlowMoExitCurve;

        public AnimationCurve mergeDistanceCurve = AnimationCurve.EaseInOut(0f, 0.2f, 1f, 0.8f);
        public int mergeLeftoverShardMultiplier = 4;

        public float playerRespawnDelay = 10f;
#if ODIN_INSPECTOR
        [FoldoutGroup("Platform Parameters", false)]
#endif
        public PlatformParameters pc = new PlatformParameters();
#if ODIN_INSPECTOR
        [FoldoutGroup("Platform Parameters", false)]
#endif
        public PlatformParameters android = new PlatformParameters();
        [NonSerialized]
        public PlatformParameters platformParameters;

        [Serializable]
        public class PlatformParameters
        {
            public int maxHomeItem = 30;
            public int maxWaveAlive = 20;
            public int maxRoomNpc = 10;
            public int itemSpawnerMaxItems = 100;
            public int itemSpawnerDelay = 0;
            public float mirrorQuality = 1f;

            public float cleanDeadLastInteractionDelay = 7;
            public float cleanObjectsLastInteractionDelay = 7;

            public float aiCycleMinSpeed = 0.1f;
            public float aiCycleMaxSpeed = 2;

            public int poolingAudioCount = 20;
            public int poolingVfxCount = 10;
            public int poolingShaderCount = 100;
            public int poolingRevealCount = 100;
            public int poolingMeshCount = 100;
            public int poolingLightCount = 100;

            public bool enableEffectAudio = true;
            public bool enableEffectVfx = true;
            public bool enableEffectParticle = true;
            public bool enableEffectDecal = true;
            public bool enableEffectMesh = true;
            public bool enableEffectReveal = true;
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

#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<Category> categories;
#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<string> holderSlots;
        public int baseSkillCost = 4;
        public ScalingMode skillCostScalingMode = ScalingMode.Linear;
#if ODIN_INSPECTOR
        [ShowIf(nameof(skillCostScalingMode), ScalingMode.Exponential)]
#endif
        public float skillCostMultiplierPerTier = 2;

        public ScreenshotFormat screenshotFormat = ScreenshotFormat.JPG;
        public int screenshotJpegQuality = 75;

        public enum ScalingMode
        {
            Linear,
            Exponential
        }

        public enum ScreenshotFormat
        {
            JPG,
            PNG,
            All,
        }

        public List<LayerName> locomotionPhysicsLayers = new List<LayerName>();
        public string endGameMenuPrefabAddress;
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
        public int cleanFarObjectsSqrDistance = 62500; //250m squared
        public float motionBlurIntensity = 0.35f;

        public float droppedItemImbueDrainNpc = 10;
        public float droppedItemImbueDrainPlayer = 5;

        public bool useDynamicMusic = false;

        public string errorAudioGroupAddress;
        [NonSerialized]
        public AudioContainer errorAudioGroup;

#if ODIN_INSPECTOR
        [FoldoutGroup("Collision Detection", false)]
#endif
        public CollisionDetection collisionDetection;

        [Serializable]
        public class IDMapColors
        {
            public int id;
            public Color color;
            public string label;
        }

        public Color GetIDMapColor(int id)
        {
            return Color.black;
        }

        public int GetIDMapId(Color color)
        {
return 0;
        }


#if ODIN_INSPECTOR
        [FoldoutGroup("Physic Material ID Map colours", false)]
#endif
        [Tooltip("The colours used to represent the different physic material ids in the ID map texture, 16 ids are supported")]
        public List<IDMapColors> physicMaterialIDMapColours = new List<IDMapColors>();

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
#if ODIN_INSPECTOR
        [FoldoutGroup("Haptics", false)]
#endif
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
            public AnimationCurve telekinesisMassIntensity = AnimationCurve.Linear(0.5f, 0.6f, 10, 3f);
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
                this.curveIntensity = curveIntensity ?? AnimationCurve.Linear(0, 0.05f, 1, 0.2f);
                this.curvePeriod = AnimationCurve.Linear(0, 0.05f, 1, 0.05f);
                this.duration = 1f;
            }

            public HapticClip(AnimationCurve curveIntensity, float amplitude, float duration)
            {
                this.curveIntensity = curveIntensity ?? AnimationCurve.Linear(0, 0.05f, 1, 0.2f);
                this.curvePeriod = AnimationCurve.Linear(0, 0.05f, 1, 0.05f);
                this.duration = duration;
            }

            public AnimationCurve curveIntensity;
            public AnimationCurve curvePeriod;
            public float duration;
        }


#if ODIN_INSPECTOR

        public List<ValueDropdownItem<string>> GetAllValueTypes()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("Gold", "Gold"));
            dropdownList.Add(new ValueDropdownItem<string>("CrystalShard", "CrystalShard"));
            return dropdownList;
        }

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
            return Catalog.GetDropdownAllID(ThunderRoad.Category.Level);
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(ThunderRoad.Category.Effect);
        }

        public List<ValueDropdownItem<string>> GetAllHandPoseID()
        {
            return Catalog.GetDropdownAllID(ThunderRoad.Category.HandPose);
        }
#endif



        public void OnCatalogRefresh()
        {
        }


        public IEnumerator OnCatalogRefreshCoroutine()
        {
            yield return null;
        }
    }
}
