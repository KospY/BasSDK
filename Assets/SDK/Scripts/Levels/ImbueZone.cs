using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ImbueZone")]
    [RequireComponent(typeof(Collider))]
    public class ImbueZone : MonoBehaviour
    {
        public float transferRate = 0.1f;
        public float transferMaxPercent = 50f;

        public string imbueSpellId;
    }
}