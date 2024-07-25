
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif //ODIN_INSPECTOR
using System.Collections;
using UnityEngine;

namespace ThunderRoad
{
    public class ItemModuleNote : ItemModule
    {
        public bool isCustomNote = false;
#if ODIN_INSPECTOR
        [ShowIf("isCustomNote")]
#endif
        public string contentPrefabAddress = "";
#if ODIN_INSPECTOR
        [ShowIf("isCustomNote")]
#endif
        public string groupTextId;
#if ODIN_INSPECTOR
        [ShowIf("isCustomNote")]
#endif
        public string textId;
    }
}