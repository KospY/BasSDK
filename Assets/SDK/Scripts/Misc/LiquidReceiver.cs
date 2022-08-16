using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/LiquidReceiver")]
    [AddComponentMenu("ThunderRoad/Liquid receiver")]
    public class LiquidReceiver : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string drinkEffectId;

        public float maxAngle = 30;
        public float stopDelay = 0.1f;
        public float effectRate = 1f;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Effect);
        }
#endif

    }
}