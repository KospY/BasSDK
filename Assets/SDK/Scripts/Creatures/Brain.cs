using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Brain")]
    [AddComponentMenu("ThunderRoad/Creatures/Brain")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Brain : ThunderBehaviour
    {
        public enum State
        {
            Idle,
            Follow,
            Patrol,
            Investigate,
            Alert,
            Combat,
            Grappled,
            Custom,
        }

    }
}
