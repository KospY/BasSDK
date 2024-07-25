using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class HolderData : InteractableData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Holder"), ValueDropdown(nameof(GetAllItemID))]
#endif
        public string spawnItemID;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public int spawnQuantity = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool allowInfiniteSupplyCheat = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public DespawnProtection itemDespawnCondition;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public int maxQuantity = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public string targetAnchor;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool forceAllowTouchOnPlayer;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool highlighterPlayerView = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public FilterLogic tagFilter = FilterLogic.AnyExcept;
#if ODIN_INSPECTOR
        [BoxGroup("Holder"), ValueDropdown(nameof(GetAllHolderSlots))]
#endif
        public List<string> slots;

#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public FilterLogic typeFilter = FilterLogic.AnyExcept;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public List<ItemData.Type> itemTypes;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public FilterLogic itemFilter = FilterLogic.AnyExcept;
#if ODIN_INSPECTOR
	    [BoxGroup("Holder"), ValueDropdown(nameof(GetAllItemID))]
#endif
        public List<string> itemIds;
        
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool locked = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool warnIfNotAllowed = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool warnIfInUse = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool warnIfLocked = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool grabFromHandle = true;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool playerGrabFromHandle = true;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool grabWithTrigger = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool disableWhenHolstered = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool allowTeleGrab = true;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool hideFromFpv;

#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public GrabTeleport grabTeleport = GrabTeleport.Disabled;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool disableObjectProximityHighlighter = false;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public bool delegateToParentHolder;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public float cleanItemsDuration = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public float cleanItemsStep = 10f;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }

        public List<ValueDropdownItem<string>> GetAllHolderSlots()
        {
            return Catalog.GetDropdownHolderSlots();
        }
#endif

        public enum DespawnProtection
        {
            Normal,
            ParentItem,
            ParentCreature,
            Never,
        }

        public enum GrabTeleport
        {
            Disabled,
            Enabled,
            IfParentHolder
        }

#if ODIN_INSPECTOR
        [BoxGroup("Holder")]
#endif
        public string audioContainerAddress;
        [NonSerialized]
        public IResourceLocation audioContainerLocation;

    }
}
