using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class LevelModuleShop : LevelModule
    {
#if ODIN_INSPECTOR
        List<ValueDropdownItem<string>> AllShopIDs() => Catalog.GetDropdownAllID<ShopData>();

        [ValueDropdown(nameof(AllShopIDs))]
#endif
        public string shopID;

        public override IEnumerator OnLoadCoroutine()
        {
            yield return null;
        }
    }
}
