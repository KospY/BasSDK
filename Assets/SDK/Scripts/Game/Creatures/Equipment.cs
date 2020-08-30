using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Equipment")]
    public class Equipment : MonoBehaviour
    {
        public bool equipApparelsOnLoad = true;
        public bool equipWeaponsOnLoad = true;
        public bool refreshStanceForPlayer;

    }
}
