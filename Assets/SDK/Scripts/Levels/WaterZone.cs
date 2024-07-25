using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/WaterZone")]
	[RequireComponent(typeof(Collider))]
    public class WaterZone : MonoBehaviour
    {
        public float depletionRate = 1f;
        public string depletionSpellId = "Fire";
        public string steamEffectId;
        public Transform effectSpawnPoint;

    }
}