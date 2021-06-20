using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Chabuk.ManikinMono;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Equipment")]
    public class Equipment : MonoBehaviour
    {
        public bool equipWeaponsOnLoad = true;
        public bool equipWardrobesOnLoad = true;

        public enum WardRobeCategory
        {
            Apparel,
            Body
        }

    }
}
