﻿using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/LiquidReceiver.html")]
    [AddComponentMenu("ThunderRoad/Liquid receiver")]
    public class LiquidReceiver : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID), AppendNextDrawer = true)]
#endif
        public string drinkEffectId;

        public float maxAngle = 30;
        public float stopDelay = 0.1f;
        public float effectRate = 1f;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }
#endif

    }
}