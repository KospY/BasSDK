using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class ShopDisplay : ThunderBehaviour
    {
        public enum DisplayType
        {
            Holders,
            Shelf,
            Basket,
        }

        public string displayCategory;
        [NonSerialized]
        public ShopData.ShopStockCategory connectedCategory;

        public DisplayType displayType;

        public List<Transform> includeTransforms = new List<Transform>();


        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, ShowIf("displayType", DisplayType.Holders)]
#endif
        public List<Holder> holders;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, ShowIf("displayType", DisplayType.Shelf)]
#endif
        public List<ItemShelf> shelves;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, ShowIf("displayType", DisplayType.Basket)]
#endif
        public List<ItemBasket> baskets;

        public Shop shop;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<Item> displayItems;

    }
}
