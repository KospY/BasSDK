using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public abstract class SkillData : CatalogData, IContainerLoadable<SkillData>
    {
        // Show disabled skills at the bottom of the Odin tree. It's reversed, I don't know why.
        public override string SortKey() => (!showInTree && !isDefaultSkill ? "AAA" : "")
                                            + (8 - (isDefaultSkill ? -1 : tier))
                                            + (isTierBlocker ? 0 : 8 - (isDefaultSkill ? -1 : tier))
                                            + id;
#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display"), ValueDropdown(nameof(GetAllShardsId))]
#endif
        public string shardId = "Crystal_Small_01_Shard";

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public string prefabAddress = "Bas.Item.Misc.SkillOrb";

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public string meshAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public float meshSize = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string orbLinkEffectId = "SkillTreeOrbLink";

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display"), InfoBox("0-indexed tier. Use 0 for Tier 1, 1 for Tier 2, and 2 for Tier 3.")]
#endif
        public int tier;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public bool allowSkill = true;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public bool forceAllowRefund = false;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public bool showInTree = true;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public bool hideInSkillMenu = false;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public bool IsCombinedSkill => !string.IsNullOrEmpty(primarySkillTreeId) && !string.IsNullOrEmpty(secondarySkillTreeId) && !primarySkillTreeId.Equals(secondarySkillTreeId);

#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string skillTreeDisplayName;

        public string GetName() 
        {
            return "Skills";
        }

        public string EditorPrefix()
        {
            if (string.IsNullOrEmpty(primarySkillTreeId)) return "";
            string prefix = allowSkill && showInTree ? "" : "[Disabled] ";
            return prefix
                   + $"{(IsCombinedSkill ? $"X-{string.Concat((primarySkillTreeId[0].ToString() + secondarySkillTreeId[0]).OrderBy(s => s))}" : primarySkillTreeId[0].ToString())}{tier + 1} ";
        }

#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string description;


#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string imageAddress = "";

#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string videoAddress = "";
        
#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string buttonSpriteSheetAddress = "Bas.Ui.SkillTree.Icons";
#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string buttonEnabledIconAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        public string buttonDisabledIconAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Orb")]
#endif
        public string orbIconAddress = "";

#if UNITY_EDITOR
        [NonSerialized]
        public Sprite icon;
#endif

#if ODIN_INSPECTOR
        [BoxGroup("Skill Popup")]
#endif
        private int videoCount = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tutorial")]
#endif
        public string tutorial;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tutorial")]
#endif
        public string tutorialLocalizationId;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tutorial")]
#endif
        public string tutorialGoal;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tutorial")]
#endif
        public string tutorialGoalLocalizationId;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Cost")]
#endif
        public int costOverride = -1;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Cost")]
        [ShowInInspector]
#endif
        [JsonIgnore]
        public int Cost
        {
            get
            {
                if (isTierBlocker) return 0;
                if (costOverride >= 0) return costOverride;
                float cost = Catalog.gameData?.baseSkillCost ?? 0;
                if (!IsCombinedSkill
                    && primarySkillTreeId != null
                    && Catalog.TryGetData(primarySkillTreeId, out SkillTreeData skillTreeData))
                {
                    cost *= skillTreeData.costMultiplier;
                }

                float tierCost = Catalog.gameData?.skillCostScalingMode switch
                {
                    GameData.ScalingMode.Exponential
                        => cost * Mathf.Pow(Catalog.gameData?.skillCostMultiplierPerTier ?? 2, tier),
                    _ => cost * (tier + 1)
                };
                return Mathf.FloorToInt(tierCost);
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("Skill Cost"), FormerlySerializedAs("defaultSkill")]
#endif
        public bool isDefaultSkill = false;
        
        [NonSerialized]
        public EffectData orbLinkEffectData;
        [NonSerialized]
        public Texture image = null;
        [NonSerialized]
        public VideoClip video = null;

        public delegate void SkillLoadedEvent(SkillData skill, Creature creature);

        public static event SkillLoadedEvent OnSkillLoadedEvent;
        public static event SkillLoadedEvent OnLateSkillsLoadedEvent;
        public static event SkillLoadedEvent OnSkillUnloadedEvent;

#if ODIN_INSPECTOR

        [BoxGroup("Skill Tree Display"), ValueDropdown(nameof(GetAllSkillTreeID))]
#endif
        public string primarySkillTreeId;

        [NonSerialized]
        public SkillTreeData primarySkillTree;

#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display"), ValueDropdown(nameof(GetAllSkillTreeID))]
#endif
        public string secondarySkillTreeId;

        [NonSerialized]
        public SkillTreeData secondarySkillTree;
        
        // Only one should be blocker by Tier
#if ODIN_INSPECTOR
        [BoxGroup("Skill Tree Display")]
#endif
        public bool isTierBlocker = false;

        private static bool forceInvalidateIconNames = false;
        private static bool forceInvalidateButtonNames = false;
        private static bool forceInvalidateTierBoundMesh = false;
        private static bool forceOverrideToLocalisedText = false;
        private static bool forceOverrideVideoAddress = false;

        public string RemoveTreeAffix(string treeName)
        {
            if (treeName == null) return null;
            if (treeName.EndsWith("Tree") && treeName.Length > 4)
            {
                return treeName[..^4];
            }

            return treeName;
        }

        public string GetCatalogEditorTreeName()
        {
            if (string.Compare(primarySkillTreeId, secondarySkillTreeId, StringComparison.Ordinal) < 0)
            {
                return RemoveTreeAffix(primarySkillTreeId) + secondarySkillTreeId;
            }
            return RemoveTreeAffix(secondarySkillTreeId) + primarySkillTreeId;
        }

        public bool IsOnTree(string treeId)
        {
            return primarySkillTreeId == treeId || secondarySkillTreeId == treeId;
        }
        public string GetSkillTreeKey()
        {
            return GetSkillTreeKey(primarySkillTreeId, secondarySkillTreeId);
        }

        public static string GetSkillTreeKey(string primaryTreeName, string secondaryTreeName)
        {
            //combine the two skill tree ids in alphabetical order
            if (string.Compare(primaryTreeName, secondaryTreeName, StringComparison.Ordinal) < 0)
            {
                return primaryTreeName + secondaryTreeName;
            }
            return secondaryTreeName + primaryTreeName;
        }

        public void GetVideo(Action<VideoClip> onVideoLoaded)
        {
            if(video != null)
            {
                videoCount++;
                onVideoLoaded.Invoke(video);
            }

            Catalog.LoadAssetAsync(videoAddress,
                (VideoClip clip) =>
                {
                    if (clip == null) return;
                    videoCount++;
                    video = clip;
                    onVideoLoaded.Invoke(video);
                },
                "SkillData: " + id + " loading video");
        }

        public void ReleaseVideo()
        {
            videoCount--;
            if(videoCount <= 0)
            {
                VideoClip clip = video;
                video = null;
                Catalog.ReleaseAsset(clip);
            }
        }
#if UNITY_EDITOR
        public override void CatalogEditorRefresh()
        {
            icon = Catalog.EditorLoad<Sprite>(orbIconAddress);
        }
#endif
        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();

            if (forceInvalidateIconNames || string.IsNullOrEmpty(orbIconAddress))
            {
                orbIconAddress = $"Bas.Ui.SkillTree.Icons[{id}]";
            }
            
            primarySkillTree = Catalog.GetData<SkillTreeData>(primarySkillTreeId);
            secondarySkillTree = Catalog.GetData<SkillTreeData>(secondarySkillTreeId);

            if (forceOverrideToLocalisedText)
            {
                skillTreeDisplayName = $"{{{id}Name}}";
                description = $"{{{id}Description}}";
            }

            if (forceInvalidateTierBoundMesh && isTierBlocker)
            {
                prefabAddress = "Bas.Item.Misc.SkillOrb";
                meshAddress = $"Bas.Mesh.SkillTree.TierCrystal.{primarySkillTreeId}.T{tier + 1}";
                meshSize = 0.5f;
            }

            if (forceOverrideVideoAddress)
            {
                videoAddress = $"Bas.Video.Skill.{id}";
            }

            if (IsCombinedSkill)
            {
                (string treeA, string treeB) = string.CompareOrdinal(primarySkillTreeId, secondarySkillTreeId) > 0
                    ? (secondarySkillTreeId, primarySkillTreeId)
                    : (primarySkillTreeId, secondarySkillTreeId);

                if (forceInvalidateButtonNames || string.IsNullOrEmpty(buttonEnabledIconAddress))
                    buttonEnabledIconAddress = $"{buttonSpriteSheetAddress}[{treeA}_{treeB}_ButtonColor]";

                if (forceInvalidateButtonNames || string.IsNullOrEmpty(buttonDisabledIconAddress))
                    buttonDisabledIconAddress = $"{buttonSpriteSheetAddress}[{treeA}_{treeB}_Button]";
            } else if (primarySkillTree != null)
            {
                if (forceInvalidateButtonNames || string.IsNullOrEmpty(buttonEnabledIconAddress)
                    && string.IsNullOrEmpty(primarySkillTree.iconEnabledAddress))
                    buttonEnabledIconAddress = $"{buttonSpriteSheetAddress}[{primarySkillTreeId}_ButtonColor]";

                if (forceInvalidateButtonNames || string.IsNullOrEmpty(buttonDisabledIconAddress)
                    && string.IsNullOrEmpty(primarySkillTree.iconDisabledAddress))
                    buttonDisabledIconAddress = $"{buttonSpriteSheetAddress}[{primarySkillTreeId}_Button]";
            }
        }

        public void GetOrbIcon(Action<Sprite> callback)
        {
            Catalog.LoadAssetAsync(orbIconAddress, callback, id);
        }

        public void GetButtonIcon(bool enabled, Action<Sprite> callback)
        {
            string address = enabled ? buttonEnabledIconAddress : buttonDisabledIconAddress;
            if (string.IsNullOrEmpty(address) && !IsCombinedSkill)
                address = enabled ? primarySkillTree.iconEnabledAddress : primarySkillTree.iconDisabledAddress;
            Catalog.LoadAssetAsync(address, callback, id);
        }

        /// <summary>
        /// Called when the skill is loaded on a creature, the creature is the one who has the skill
        /// </summary>
        /// <param name="skillData"></param>
        /// <param name="creature"></param>
        public virtual void OnSkillLoaded(SkillData skillData, Creature creature)
        {
            OnSkillLoadedEvent?.Invoke(skillData, creature);
        }

        /// <summary>
        /// Called when the skill is unloaded from a creature, the creature is the one who has the skill
        /// </summary>
        /// <param name="skillData"></param>
        /// <param name="creature"></param>
        public virtual void OnSkillUnloaded(SkillData skillData, Creature creature)
        {
            OnSkillUnloadedEvent?.Invoke(skillData, creature);
        }

        public virtual void OnLateSkillsLoaded(SkillData skillData, Creature creature)
        {
            OnLateSkillsLoadedEvent?.Invoke(skillData, creature);
        }

        public void OnLoadedFromContainer(Container container)
        {
            throw new NotImplementedException();
        }
        
        public virtual ContainerContent InstanceContent()
        {
	        return new SkillContent(this);
        }

#if ODIN_INSPECTOR
        
        public List<ValueDropdownItem<string>> GetAllHandPoseID()
        {
            return Catalog.GetDropdownAllID(Category.HandPose);
        }
        
        public List<ValueDropdownItem<string>> GetAllStatusEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Status);
        }
        
        public List<ValueDropdownItem<string>> GetAllSpellID()
        {
            return Catalog.GetDropdownAllID<SpellData>();
        }

        public List<ValueDropdownItem<string>> GetAllShardsId()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }

        public List<ValueDropdownItem<string>> GetAllSkillID()
        {
            return Catalog.GetDropdownAllID(Category.Skill);
        }

        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }
        
        public List<ValueDropdownItem<string>> GetAllSkillTreeID()
        {
            return Catalog.GetDropdownAllID(Category.SkillTree);
        }
        
        public List<ValueDropdownItem<string>> GetAllDamagerID()
        {
	        return Catalog.GetDropdownAllID(Category.Damager);
        }
        
        public List<ValueDropdownItem<string>> GetAllMaterialID()
        {
	        return Catalog.GetDropdownAllID(Category.Material);
        }
#else
        public void GetAllHandPoseID()
        {
        }
        
        public void GetAllStatusEffectID()
        {
        }
        
        public void GetAllSpellID()
        {
        }

        public void GetAllShardsId()
        {
        }

        public void GetAllEffectID()
        {
        }

        public void GetAllSkillID()
        {
        }

        public void GetAllItemID()
        {
        }
        
        public void GetAllSkillTreeID()
        {
        }
        
        public void GetAllDamagerID()
        {
        }
        
        public void GetAllMaterialID()
        {
        }        
#endif
    }
}
