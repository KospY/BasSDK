using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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
        [TableList(AlwaysExpanded = true)] 
#endif
        public List<Content> contents = new List<Content>();

#if ODIN_INSPECTOR

        [NonSerialized, ShowInInspector, InlineButton("TestGeneration")]
        public List<string> generationOutput;

        public void TestGeneration()
        {
            generationOutput = new List<string>();
            foreach (Content ct in contents)
            {
                if (ct.reference == Content.Reference.Item)
                {
                    generationOutput.Add(ct.referenceID);
                }
                else
                {
                    if (ct.lootTable != null)
                    {
                        ItemData pickedItem = ct.lootTable.Pick();
                        if (pickedItem != null)
                        {
                            generationOutput.Add(pickedItem.id);
                        }
                    }
                }
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
            foreach (Content ct in contents)
            {
                ct.OnCatalogRefresh();
            }
        }

        public override CatalogData Clone()
        {
            ContainerData clone = MemberwiseClone() as ContainerData;
            clone.contents = clone.contents.Select(item => (Content)item.Clone()).ToList();
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
            [ValueDropdown("GetAllItemOrLootTableID")] 
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
            public LootTable lootTable;

            public delegate void ContentSpawned(Item item);
            public event ContentSpawned OnContentSpawn;

            public Content Clone()
            {
                return new Content(itemData, state.CloneJson(), customDataList is null ? null : customDataList.CloneJson(), quantity);
            }

            /// <summary>
            /// Returns true if successfully refreshed
            /// As mods which were inside a container which are now uninstalled could now be invalid
            /// </summary>
            /// <returns></returns>
            public bool OnCatalogRefresh()
            {
                if (string.IsNullOrEmpty(referenceID)) return true;
                if (reference == Reference.Item)
                {
                    return Catalog.TryGetData<ItemData>(referenceID, out itemData);
                } else if (reference == Reference.Table)
                {
                    return Catalog.TryGetData<LootTable>(referenceID, out lootTable);
                }
                return true;
            }

            public Content()
            {
            }

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
                // Prevents negative quantities
                quantity = Mathf.Max(0, newQuantity);

                // Prevents overflowing item quantities
                if (itemData != null)
                    quantity = Mathf.Min(quantity, itemData.isStackable ? itemData.maxStackQuantity : 1);
            }

        }
    }

    public abstract class ContentCustomData
    {

    }

    public class ContentCustomDataSpell : ContentCustomData
    {
        public int LevelCastThrow;
        public int LevelCrystalShockwave;
    }
    
    /// <summary>
    /// Used to store liquid contents and maximum level
    /// </summary>
    public class ContentCustomDataLiquidContainer : ContentCustomData
    {
        public List<LiquidData.Content> LiquidContents;

        public ContentCustomDataLiquidContainer(List<LiquidData.Content> contents)
        {
            LiquidContents = contents;
        }

        public ContentCustomDataLiquidContainer()
        {
            LiquidContents = new List<LiquidData.Content>();
        }
    }

    /// <summary>
    /// Used to save animator parameter state
    /// </summary>
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

            public AnimatorParam() { }
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

        public ContentCustomDataAnimatorParams(Animator animator)
        {
            if (savedParams == null) savedParams = new List<AnimatorParam>();
            savedParams.Clear();
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                savedParams.Add(new AnimatorParam(animator, param));
            }
        }
    }

    public class ContentCustomDataContainedValue : ContentCustomData
    {
        public float value;

        public ContentCustomDataContainedValue() { }

        public ContentCustomDataContainedValue(float value)
        {
            this.value = value;
        }
    }

    public abstract class ContentState
    {

    }

    public class ContentStateWorn : ContentState
    {

    }

    public class ContentStateHolder : ContentState
    {
        public string holderName;
        public ContentStateHolder() { }
        public ContentStateHolder(string holderName)
        {
            this.holderName = holderName;
        }
    }

    public class ContentStatePlaced : ContentState
    {
        public Vector3 position;
        public Quaternion rotation;
        public ContentStatePlaced() { }
        public ContentStatePlaced(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
