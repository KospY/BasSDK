using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using ThunderRoad.Manikin;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/Equipment.html")]
    [AddComponentMenu("ThunderRoad/Creatures/Equipment")]
    public class Equipment : MonoBehaviour
    {
        public delegate void OnArmourEquipped(Wearable slot, Item item);
        public delegate void OnArmourUnEquipped(Wearable slot, Item item);

        public class WeaponDrawInfo
        {
            public ItemModuleAI.WeaponClass weaponClass = ItemModuleAI.WeaponClass.None;
            public ItemModuleAI.WeaponHandling weaponHandling = ItemModuleAI.WeaponHandling.None;
            public bool checkAmmo = false;
            public bool checkInHolder = false;
        }

        public bool canSwapExistingArmour = true;
        public bool equipWardrobesOnLoad = true;
        public bool armourEditModeEnabled = false;

        public enum WardRobeCategory
        {
            Apparel,
            Body
        }

    }
}
