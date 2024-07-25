using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class CreatureData : EntityData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Pooling")]
#endif
        public int pooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("Pooling")]
#endif
        public int androidPooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string name;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public CreatureType type;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string prefabAddress;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string animatorBundleAddress;
        [NonSerialized]
        public IResourceLocation prefabLocation;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool removeMeshWhenPooled;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public Gender gender = Gender.None;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool allowStatus = true;

#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public short health = 50;
#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public float focus = 30f;
#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public float focusRegen = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public int baseXP = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Stats"), LabelText(@"@SkillPassiveLabel(""Max Focus"", maxFocusPerSkill)")]
#endif
        public float maxFocusPerSkill = 0.12f;
#if ODIN_INSPECTOR
        [BoxGroup("Stats"), LabelText(@"@SkillPassiveLabel(""Focus Regen"", focusRegenPerSkill)")]
#endif
        public float focusRegenPerSkill = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Stats"), LabelText("@MaxHealthLabel")]
#endif
        public float maxHealthPerSkill = 0.12f;


#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public float physicsOffDamageMult = 0.33f;

#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public float heatGainMultiplier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Stats")]
#endif
        public float heatLossMultiplier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Overlap sphere")]
#endif
        public float overlapRadius = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Overlap sphere")]
#endif
        public float overlapMinDelay = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Overlap sphere")]
#endif
        public LayerMask overlapMask;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
        [ValueDropdown(nameof(GetAllContainerID))]
#endif
        public string containerID;

#if ODIN_INSPECTOR
        [BoxGroup("General"), ValueDropdown(nameof(GetAllFactionID))]
#endif
        public int factionId;

#if ODIN_INSPECTOR
        [BoxGroup("General"), ValueDropdown(nameof(GetAllEthnicityID))]
#endif
        public string ethnicityId = "Eradian";
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool countTowardsMaxAlive = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool hasMetal = false;
#if ODIN_INSPECTOR
        [BoxGroup("AI"), ValueDropdown(nameof(GetAllBrainID))]
#endif
        public string brainId;
#if ODIN_INSPECTOR
        [BoxGroup("AI")]
#endif
        public int avoidancePriority = 10;

#if ODIN_INSPECTOR
        [TabGroup("Audio"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string focusReadyEffectId;
        [NonSerialized]
        public EffectData focusReadyEffect;
#if ODIN_INSPECTOR
        [TabGroup("Audio"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string focusFullEffectId;
        [NonSerialized]
        public EffectData focusFullEffect;
#if ODIN_INSPECTOR
        [TabGroup("Audio"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string jumpEffectId;
        [NonSerialized]
        public EffectData jumpEffectData;
#if ODIN_INSPECTOR
        [TabGroup("Audio"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string kickEffectId;
        [NonSerialized]
        public EffectData kickEffectData;
#if ODIN_INSPECTOR
        [TabGroup("Audio"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string playerFallDamageEffectId;
        [NonSerialized]
        public EffectData playerFallDamageEffectData;
#if ODIN_INSPECTOR
        [TabGroup("Audio"), ValueDropdown(nameof(GetAllVoiceID))]
#endif
        public List<string> voices;

#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public float randomMinHeight;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public float randomMaxHeight;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public bool adjustHeightToPlayer;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public float adjustHeightToPlayerDelta = 0.05f;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public PossesionScaling possesionScaling = PossesionScaling.ScaleCreature;

        public enum PossesionScaling
        {
            ScaleCreature,
            ScalePlayer,
        }

#if ODIN_INSPECTOR
        [TabGroup("Customization"), TableList]
#endif
        public List<EthnicGroup> ethnicGroups;

#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public List<Color> hairColorsPrimaryShared;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public List<Color> hairColorsSecondaryShared;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public List<Color> hairColorsSpecularShared;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public List<Color> eyesColorsIrisShared;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public List<Color> eyesColorsScleraShared;
#if ODIN_INSPECTOR
        [TabGroup("Customization")]
#endif
        public List<Color> skinColorsShared;

        [Serializable]
        public class EthnicGroup
        {
            public string id;
            public string loreTextId;

#if ODIN_INSPECTOR
            [BoxGroup("Custom Materials")]
#endif
            public string bodyMaterialAddressLod0;
#if ODIN_INSPECTOR
            [BoxGroup("Custom Materials")]
#endif
            public string bodyMaterialAddressLod1;
#if ODIN_INSPECTOR
            [BoxGroup("Custom Materials")]
#endif
            public string handsMaterialAddressLod0;
#if ODIN_INSPECTOR
            [BoxGroup("Custom Materials")]
#endif
            public string handsMaterialAddressLod1;


#if ODIN_INSPECTOR
            [BoxGroup("Custom Parts")]
            [ValueDropdown(nameof(GetAllHeadsId))]
#endif
            public List<string> allowedHeadsIDs;

#if ODIN_INSPECTOR
            [BoxGroup("Colors")]
            [BoxGroup("Colors/Hair")]
#endif
            public List<Color> hairColorsPrimary;
#if ODIN_INSPECTOR
            [BoxGroup("Colors/Hair")]
#endif
            public List<Color> hairColorsSecondary;
#if ODIN_INSPECTOR
            [BoxGroup("Colors/Hair")]
#endif
            public List<Color> hairColorsSpecular;
#if ODIN_INSPECTOR
            [BoxGroup("Colors/Eyes")]
#endif
            public List<Color> eyesColorsIris;
#if ODIN_INSPECTOR
            [BoxGroup("Colors/Eyes")]
#endif
            public List<Color> eyesColorsSclera;
#if ODIN_INSPECTOR
            [BoxGroup("Colors/Skin")]
#endif
            public List<Color> skinColors;
        }

#if ODIN_INSPECTOR
        [TabGroup("Ragdoll")]
#endif
        public RagdollData ragdollData;

        [Serializable]
        public class RagdollData
        {
            public float standingMass = 65f;
            public float handledMass = 65f;
            public float ragdolledMass = 65f;
            public bool meshRaycast = true;
            public bool meshRaycastAdjustContactPoint = false;
            public float destabilizedSpringRotationMultiplier = 0.5f;
            public float destabilizedDamperRotationMultiplier = 0.1f;
            public float destabilizedGroundSpringRotationMultiplier = 0.2f;
            public float fallAliveAnimationHeight = 0.5f;
            public float fallAliveDestabilizeHeight = 3;
            public float groundStabilizationMaxVelocity = 1;
            public float groundStabilizationMinDuration = 3;

            public float gripMaxCloseWeight = 0.9f;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllEffectID))]
#endif
            public string gripEffectId;
            [NonSerialized]
            public EffectData gripEffectData;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllItemID))]
#endif
            public string footItemId;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllItemID))]
#endif
            public string footTrackedItemId;

            public float fingerSpeed = 5;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllDamagerID))]
#endif
            public string bodyDefaultDamagerID;
            [NonSerialized]
            public DamagerData bodyDefaultDamagerData;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllEffectID))]
#endif
            public string penetrationEffectId;
            [NonSerialized]
            public EffectData penetrationEffectData;
            public float penetrationEffectRepeatDistance = 0.05f;
            [NonSerialized]
            public bool hasPenetrationEffect;

            public List<PartData> parts = new List<PartData>();

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllDamagerID()
            {
                return Catalog.GetDropdownAllID(Category.Damager);
            }

            public List<ValueDropdownItem<string>> GetAllItemID()
            {
                return Catalog.GetDropdownAllID(Category.Item);
            }

            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Category.Effect);
            }
#endif
        }

        [Serializable]
        public class PartData
        {
            public RagdollPart.Type bodyPartTypes;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllDamagerID))]
#endif
            public string bodyDamagerID;
            [NonSerialized]
            public DamagerData bodyDamagerData;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllDamagerID))]
#endif
            public string bodyAttackDamagerID;
            [NonSerialized]
            public DamagerData bodyAttackDamagerData;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllEffectID))]
#endif
            public string sliceParentEffectid;
            [NonSerialized]
            public EffectData sliceParentEffectData;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllEffectID))]
#endif
            public string sliceChildEffectId;
            [NonSerialized]
            public EffectData sliceChildEffectData;

            public float damageMultiplier = 1;

            public float penetrationPierceDeepDepth = 0.1f;
            public float penetrationPierceDeepDamageMultiplier = 1;

            public float penetrationSlashDamageMultiplier = 1;
            public float penetrationSkewerDamageMultiplier = 1;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllEffectID))]
#endif
            public string penetrationDeepEffectId;
            [NonSerialized]
            public EffectData penetrationDeepEffectData;

            public bool sliceForceKill = true;
            public float sliceSeparationForce = 3;
            public float sliceVelocityMultiplier = 0.5f;

            public float locomotionVelocityCorrection = 130;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllDamagerID()
            {
                return Catalog.GetDropdownAllID(Category.Damager);
            }

            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Category.Effect);
            }
#endif
        }


#if ODIN_INSPECTOR
        [TabGroup("Expression"), ValueDropdown(nameof(GetAllExpressionId))]
#endif
        public string expressionAttackId;
        [NonSerialized]
        public ExpressionData expressionAttackData;
#if ODIN_INSPECTOR
        [TabGroup("Expression"), ValueDropdown(nameof(GetAllExpressionId))]
#endif
        public string expressionPainId;
        [NonSerialized]
        public ExpressionData expressionPainData;
#if ODIN_INSPECTOR
        [TabGroup("Expression"), ValueDropdown(nameof(GetAllExpressionId))]
#endif
        public string expressionDeathId;
        [NonSerialized]
        public ExpressionData expressionDeathData;
#if ODIN_INSPECTOR
        [TabGroup("Expression"), ValueDropdown(nameof(GetAllExpressionId))]
#endif
        public string expressionChokeId;
        [NonSerialized]
        public ExpressionData expressionChokeData;
#if ODIN_INSPECTOR
        [TabGroup("Expression"), ValueDropdown(nameof(GetAllExpressionId))]
#endif
        public string expressionAngryId;
        [NonSerialized]
        public ExpressionData expressionAngryData;
#if ODIN_INSPECTOR
        [TabGroup("Expression")]
#endif
        public Vector2 eyeVerticalAngleClamps = new Vector2(-10f, 15f);
#if ODIN_INSPECTOR
        [TabGroup("Expression")]
#endif
        public Vector2 eyeHorizontalAngleClamps = new Vector2(-20f, 20f);
#if ODIN_INSPECTOR
        [TabGroup("Expression")]
#endif
        public List<EyeClip> eyeClips = new List<EyeClip>();

        [Serializable]
        public class EyeClip
        {
            public string clipName = "";
            public string eyeTagFilter = "";
            public float duration = 0.33f;
#if ODIN_INSPECTOR
            [MinMaxSlider(0f, 5f)]
#endif
            public Vector2 minMaxDelayPerEye = new Vector2(0f, 0f);
            public AnimationCurve openCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.5f, 0f), new Keyframe(1f, 1f));
            public bool playAutomaticallyWhileAlive = false;
#if ODIN_INSPECTOR
            [MinMaxSlider(0f, 120f)]
#endif
            public Vector2 minMaxBetweenPlays = new Vector2(2.5f, 5f);
#if ODIN_INSPECTOR
            [ReadOnly, ShowInInspector]
#endif
            [NonSerialized]
            public bool active = false;
#if ODIN_INSPECTOR
            [ReadOnly, ShowInInspector]
#endif
            [NonSerialized]
            public float lastStartTime = -1f;
#if ODIN_INSPECTOR
            [ReadOnly, ShowInInspector]
#endif
            [NonSerialized]
            public float lastEndTime = -1f;
#if ODIN_INSPECTOR
            [ReadOnly, ShowInInspector]
#endif
            [NonSerialized]
            public float maxIndividualDelay = 0f;
#if ODIN_INSPECTOR
            [ReadOnly, ShowInInspector]
#endif
            [NonSerialized]
            public float nextPlayDelay = -1f;
            [NonSerialized]
            public Dictionary<CreatureEye, float> affectedEyes = new Dictionary<CreatureEye, float>();

            public EyeClip Clone()
            {
                EyeClip clone = new EyeClip();
                clone.clipName = clipName;
                clone.eyeTagFilter = eyeTagFilter;
                clone.duration = duration;
                clone.minMaxDelayPerEye = minMaxDelayPerEye;
                clone.openCurve = openCurve;
                clone.playAutomaticallyWhileAlive = playAutomaticallyWhileAlive;
                clone.minMaxBetweenPlays = minMaxBetweenPlays;
                return clone;
            }
        }

        public enum Gender
        {
            None,
            Male,
            Female,
        }

#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public float forceMaxPosition = 3000;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public float forceMaxRotation = 250;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 forcePositionSpringDamper = new Vector2(5000, 100);
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 forceRotationSpringDamper = new Vector2(1000, 50);
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        [Header("2Handed")]
        public Vector2 forcePositionSpringDamper2HMult = Vector2.one;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 forceSpringDamper2HNoDomMult = Vector2.one;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 forceRotationSpringDamper2HMult = Vector2.one;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        [Header("Climbing")]
        public float climbingForceMaxPosition = 3000;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public float climbingForceMaxRotation = 100;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 climbingForcePositionSpringDamperMult = new Vector2(1, 3);
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        [Header("Grip")]
        public float gripForceMaxPosition = 3000;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public float gripForceMaxRotation = 100;
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 gripForcePositionSpringDamperMult = new Vector2(1, 1);
#if ODIN_INSPECTOR
        [TabGroup("Force")]
#endif
        public Vector2 gripForceRotationSpringDamperMult = new Vector2(0.5f, 1);

#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public bool overrideGroundMask = false;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public LayerMask groundMask;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public bool destabilizeOnFall = true;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionMass = 70f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionForwardSpeed = 0.2f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionBackwardSpeed = 0.2f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionStrafeSpeed = 0.2f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionRunSpeedAdd = 0.1f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionCrouchSpeed = 0.1f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionAirSpeed = 150f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionJumpForce = 0.3f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionJumpClimbVerticalMultiplier = 1f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionJumpClimbHorizontalMultiplier = 0.4f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float jumpClimbVerticalMaxVelocityRatio = 20f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionJumpMaxDuration = 0.6f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionGroundDrag = 3f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float locomotionFlyDrag = 0.3f;
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public AnimationCurve playerFallDamageCurve = AnimationCurve.Linear(12, 0, 20, 200);
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public AnimationCurve playerHortizontalFallDamageCurve = AnimationCurve.Linear(0, 0, 0, 0);
#if ODIN_INSPECTOR
        [TabGroup("Locomotion")]
#endif
        public float gripRecoverTime = 0.5f;

#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public AnimationCurve waterLocomotionDragMultiplierCurve = AnimationCurve.EaseInOut(0, 1, 1, 12);
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public AnimationCurve waterHandSpringMultiplierCurve = AnimationCurve.EaseInOut(0, 0.3f, 1, 0.1f);
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterHandLocomotionVelocityCorrectionMultiplier = 0.7f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public AnimationCurve waterJumpForceMutiplierCurve = AnimationCurve.EaseInOut(0, 1f, 1, 0.25f);
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterSwimUpForce = 10f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterHandMovementForce = 10f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterHandMovementMaxMagnitude = 10f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public AnimationCurve waterHandMovementForceCurve = AnimationCurve.EaseInOut(0, 0f, 1, 1f);
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterMaxPlayerVelocityMagnitude = 3f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterDrowningStartTime = 40f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterDrowningWarningRatio = 0.7f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterDrowningDamage = 10f;
#if ODIN_INSPECTOR
        [TabGroup("Water")]
#endif
        public float waterDrowningDamageInterval = 4f;

#if ODIN_INSPECTOR
        [TabGroup("Water"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string drownEffectId;
        [NonSerialized]
        public EffectData drownEffectData;

        protected static int index;

#if ODIN_INSPECTOR && UNITY_EDITOR
        #region ODIN

        public List<ValueDropdownItem<string>> GetAllHeadsId()
        {
            var heads = new List<ValueDropdownItem<string>>();
            foreach (var catalogData in Catalog.GetDataList(Category.Item).Where(i => i is ItemData))
            {
                var itemData = (ItemData)catalogData;
                var itemModuleWardrobe = itemData.GetModule<ItemModuleWardrobe>();

                if (itemModuleWardrobe != null && itemModuleWardrobe.category == Equipment.WardRobeCategory.Body)
                {
                    foreach (var creatureWardrobe in itemModuleWardrobe.wardrobes)
                    {
                        if (creatureWardrobe != null
                            && creatureWardrobe.manikinWardrobeData != null
                            && creatureWardrobe.manikinWardrobeData.channels.Contains("Head")
                            && creatureWardrobe.manikinWardrobeData.layers.Contains(ItemModuleWardrobe.GetLayer("Head", "Body")))
                        {
                            heads.Add(new ValueDropdownItem<string>(itemModuleWardrobe.itemData.id,
                                itemModuleWardrobe.itemData.id));
                        }
                    }
                }
            }

            return heads;
        }
        public List<ValueDropdownItem<string>> GetAllVoiceID()
        {
            return Catalog.GetDropdownAllID(Category.Voice);
        }

        public List<ValueDropdownItem<string>> GetAllDamagerID()
        {
            return Catalog.GetDropdownAllID(Category.Damager);
        }

        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
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
        public List<ValueDropdownItem<string>> GetAllSpellID()
        {
            return Catalog.GetDropdownAllID<SpellData>();
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }
        public List<ValueDropdownItem<string>> GetAllExpressionId()
        {
            return Catalog.GetDropdownAllID(Category.Expression);
        }
        public List<ValueDropdownItem<string>> GetAllHandPoseId()
        {
            return Catalog.GetDropdownAllID(Category.HandPose);
        }

        #endregion
#endif

        public override int GetCurrentVersion()
        {
            return 6;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            expressionAttackData = Catalog.GetData<ExpressionData>(expressionAttackId);
            expressionPainData = Catalog.GetData<ExpressionData>(expressionPainId);
            expressionDeathData = Catalog.GetData<ExpressionData>(expressionDeathId);
            expressionChokeData = Catalog.GetData<ExpressionData>(expressionChokeId);
            expressionAngryData = Catalog.GetData<ExpressionData>(expressionAngryId);
            playerFallDamageEffectData = Catalog.GetData<EffectData>(playerFallDamageEffectId);
            drownEffectData = Catalog.GetData<EffectData>(drownEffectId);
            jumpEffectData = Catalog.GetData<EffectData>(jumpEffectId);
            kickEffectData = Catalog.GetData<EffectData>(kickEffectId);
            focusReadyEffect = Catalog.GetData<EffectData>(focusReadyEffectId);
            focusFullEffect = Catalog.GetData<EffectData>(focusFullEffectId);

            ragdollData ??= new RagdollData();
            ragdollData.penetrationEffectData = Catalog.GetData<EffectData>(ragdollData.penetrationEffectId);
            ragdollData.hasPenetrationEffect = ragdollData.penetrationEffectData != null;
            ragdollData.bodyDefaultDamagerData = Catalog.GetData<DamagerData>(ragdollData.bodyDefaultDamagerID);
            ragdollData.gripEffectData = Catalog.GetData<EffectData>(ragdollData.gripEffectId);
            int partsCount = ragdollData.parts.Count;
            for (var i = 0; i < partsCount; i++)
            {
                PartData partData = ragdollData.parts[i];
                partData.bodyDamagerData = Catalog.GetData<DamagerData>(partData.bodyDamagerID);
                partData.bodyAttackDamagerData = Catalog.GetData<DamagerData>(partData.bodyAttackDamagerID);
                partData.sliceChildEffectData = Catalog.GetData<EffectData>(partData.sliceChildEffectId);
                partData.sliceParentEffectData = Catalog.GetData<EffectData>(partData.sliceParentEffectid);
                partData.penetrationDeepEffectData = Catalog.GetData<EffectData>(partData.penetrationDeepEffectId);
            }
        }

    }

    [Flags]
    public enum CreatureType
    {
        Human =  0b0001,
        Animal = 0b0010,
        Golem =  0b0100,
        Other =  0b1000
    }
}
