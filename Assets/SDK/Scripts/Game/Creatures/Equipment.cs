using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Chabuk.ManikinMono;

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
        public bool equipWeaponsOnLoad = true;
        public bool equipWardrobesOnLoad = true;
        public bool refreshStanceForPlayer;

    }
}
