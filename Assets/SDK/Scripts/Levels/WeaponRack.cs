using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class WeaponRack : MonoBehaviour
    {
        public string rackId = "Rack1";
        public bool playerRack;

    }
}
