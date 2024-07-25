using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/Water.html")]
    [AddComponentMenu("ThunderRoad/Levels/Water")]
    public class Water : MonoBehaviour
    {
        
        public GameObject underWaterEffect;

        [Tooltip("When enabled, the Water will only be enabled when you enter the room (Dungeons Only)")]
        public bool showWhenInRoomOnly = true;

        [NonSerialized]
        public bool waterHeightCanChangeovertime = false;

        public enum Quality
        {
            Low,
            High,
        }

    }
}