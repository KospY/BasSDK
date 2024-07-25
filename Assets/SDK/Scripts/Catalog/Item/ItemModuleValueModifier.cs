using ThunderRoad.Modules;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class ItemModuleValueModifier : ItemModule
    {
	    [Tooltip("If true, the item will use the main currency of the current gamemode instead of the one specified on the item")]
        public bool useModeMainCurrency = false;
        
        public bool spawnPriceTag = false;
        public string priceTagAddress = "Bas.Misc.ShopPriceTag";

    }
}
