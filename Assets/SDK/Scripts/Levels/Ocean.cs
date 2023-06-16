using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Ocean")]
    [AddComponentMenu("ThunderRoad/Levels/Ocean")]
    public class Ocean : MonoBehaviour
    {
        public string prefabAddress = "Bas.Ocean.Greenland.LightHouse";
        public bool alwaysUseHighQuality = false;
        public string lowQualityPrefabAddress = "Bas.Ocean.LowQuality";
        public GameObject lowQuality;
        public Transform heightReference;
        public bool showWhenInRoomOnly = true;

        public enum Quality
        {
            Plane,
            Waves,
        }

    }
}