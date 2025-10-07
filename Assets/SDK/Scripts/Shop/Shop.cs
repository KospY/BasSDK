using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class Shop : ThunderBehaviour
    {
        public static Shop local { get; protected set; }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllShopID()
        {
            return Catalog.GetDropdownAllID(Category.Shop);
        }

        [ValueDropdown(nameof(GetAllShopID), AppendNextDrawer = true)]
#endif
        public string shopDataID;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public ShopData data;
        public Transform defaultShopkeeperSpawnPoint;
        public Transform tutorialShopkeeperSpawnPoint;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Creature shopkeeper;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [HideInInspector]
        public BrainModuleShopkeeper moduleShopkeeper;
        public Container shopContents;

        public GameObject priceTagPrefab;

        public string tutorialOverThereAnimationId;
        public float shopKeeperKillReloadDuration = 10;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<Item> buyingItems = new List<Item>();

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<Item> sellingItems = new List<Item>();

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public int buyingValue = 0;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public int sellingValue = 0;

        public Transform shopkeeperBuyWaypoint;
        public Transform shopkeeperSellWaypoint;
        public Transform shopkeeperStoreWaypoint;

        public WayPoint[] idleWaypoints;

        public Zone playerBuyZone;
        public Zone playerSellZone;
        public Zone playerStoreZone;

        public Zone itemBuyZone;
        public Zone itemSellZone;

        public List<ShopDisplay> displayStands { get; protected set; }

        protected Coroutine tutorialCoroutine;

        public enum Transaction
        {
            Buy,
            Sell,
        }

        public enum ShopZone
        {
            Buy,
            Sell,
            Store,
        }

        public event Action<EventTime> onShopContentChanged;
        public event Action<ShopZone, bool> onPlayerChangeZone;
        public event Action<Transaction, Item, bool> onZoneItemChanged;
        public event Action<Transaction, Item> onItemEnterZone;
        public event Action<Transaction, Item> onItemExitZone;
        public event Action<Transaction> onTransactionStart;
        public event Action<Transaction, bool> onTransactionCompleted;

 // ProjectCore
    }
}
