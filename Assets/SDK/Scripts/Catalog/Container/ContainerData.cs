using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
#else
using TriInspector;
#endif

namespace ThunderRoad
{

    [Serializable]
    public class ContainerData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Container")] 
#endif
        public string displayName = "Unknown";
#if ODIN_INSPECTOR
        [BoxGroup("Container")] 
#endif
        [Multiline]
        public string description;

#if ODIN_INSPECTOR
        [HideInInspector]
#endif
        public List<Content> contents = new List<Content>();

#if ODIN_INSPECTOR
        [ShowInInspector, HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
#endif
        public List<ContainerContent> containerContents = new List<ContainerContent>();



        public override int GetCurrentVersion()
        {
            return 1;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();

        }

        public override CatalogData Clone()
        {
            ContainerData clone = MemberwiseClone() as ContainerData;
            clone.containerContents = clone.containerContents.Select(item => item.Clone()).ToList();
            return clone;
        }


        [Serializable]
        public class Content
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Reference"), EnumToggleButtons, HideLabel, TableColumnWidth(100, false)] 
#endif
            public Reference reference = Reference.Item;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllItemOrLootTableID))] 
#endif
            [JsonMergeKey]
            public string referenceID;

#if ODIN_INSPECTOR
            [TableColumnWidth(55, false)] 
#endif
            public int quantity = 1;

#if ODIN_INSPECTOR
            [ShowInInspector] 
#endif
            public ContentState state;

#if ODIN_INSPECTOR
            [ShowInInspector] 
#endif
            public List<ContentCustomData> customDataList;

            public enum Reference
            {
                Item,
                Table
            }

            [NonSerialized]
            public ItemData itemData;
            [NonSerialized]
            public LootTableBase lootTable;

            public delegate void ContentSpawned(Item item);
            public event ContentSpawned OnContentSpawn;


            public Content(ItemData itemData, ContentState state = null, List<ContentCustomData> customDataList = null, int quantity = 1)
            {
                this.reference = Reference.Item;
                this.referenceID = itemData.id;
                this.itemData = itemData;
                this.state = state;
                this.customDataList = customDataList;
                SetQuantity(quantity);
            }

            public void SetQuantity(int newQuantity)
            {
            }

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllItemOrLootTableID()
            {
                if (reference == Reference.Item)
                {
                    return Catalog.GetDropdownAllID(Category.Item);
                }
                else
                {
                    return Catalog.GetDropdownAllID(Category.LootTable);
                }
            }
#endif
        }
    }

    [System.Serializable]
    public abstract class ContentCustomData
    {
    }

    /// <summary>
    /// Used to store liquid contents and maximum level
    /// </summary>
    [System.Serializable]
    public class ContentCustomDataLiquidContainer : ContentCustomData
    {
        public List<LiquidData.Content> LiquidContents;

    }

    /// <summary>
    /// Used to save animator parameter state
    /// </summary>
    [System.Serializable]
    public class ContentCustomDataAnimatorParams : ContentCustomData
    {
        [System.Serializable]
        public class AnimatorParam
        {
            public string name;
            public AnimatorControllerParameterType type = AnimatorControllerParameterType.Bool;
            public bool boolVal;
            public int intVal;
            public float floatVal;


            public AnimatorParam(Animator animator, AnimatorControllerParameter param)
            {
                name = param.name;
                type = param.type;
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Bool:
                    boolVal = animator.GetBool(param.nameHash);
                    break;
                    case AnimatorControllerParameterType.Int:
                    intVal = animator.GetInteger(param.nameHash);
                    break;
                    case AnimatorControllerParameterType.Float:
                    floatVal = animator.GetFloat(param.nameHash);
                    break;
                }
            }
        }

        public List<AnimatorParam> savedParams = new List<AnimatorParam>();

    }

    [System.Serializable]
    public class ContentCustomDataValueOverride : ContentCustomData
    {
        public string valueType = null;
        public float value = -1f;

    }

    /// <summary>
    /// Allows you to store an order value for the content, useful for sorting
    /// </summary>
    [System.Serializable]
    public class ContentCustomOrder : ContentCustomData
    {
        public int order = 0;

    }

    [System.Serializable]
    public abstract class ContentState
    {
        /// <summary>
        /// Returns a cloned instance of the ContentState.
        /// It is recommended to implment this method and avoid CloneJson and instead do a deep copy of the object yourself where possible.
        /// </summary>
        /// <returns></returns>
        public virtual ContentState Clone()
        {
            return this.CloneJson();
        }
    }

    [System.Serializable]
    public class ContentStateWorn : ContentState
    {
        public override ContentState Clone()
        {
            return new ContentStateWorn();
        }
    }

    [System.Serializable]
    public class ContentStateHolder : ContentState
    {
        public string holderName;

        public ContentStateHolder() { }

        public ContentStateHolder(string holderName)
        {
            this.holderName = holderName;
        }
        public override ContentState Clone()
        {
            return new ContentStateHolder(holderName);
        }
    }

    [System.Serializable]
    public class ContentStatePlaced : ContentState
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool kinematic;
        public string levelId;
        public float lastSpawnTime;

        public ContentStatePlaced() { }


        public override ContentState Clone()
        {
            return new ContentStatePlaced()
            {
                position = position,
                rotation = rotation,
                kinematic = kinematic,
                levelId = levelId,
                lastSpawnTime = lastSpawnTime
            };
        }
    }

    /// <summary>
    /// A class to set the state on an item so it wont appear in the inventory UI
    /// </summary>
    [System.Serializable]
    public class ContentStateIgnoredByInventory : ContentState
    {
        public override ContentState Clone()
        {
            return new ContentStateIgnoredByInventory();
        }
    }

}
