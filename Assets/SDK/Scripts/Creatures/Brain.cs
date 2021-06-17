using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Brain")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Brain : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Patrol,
            Follow,
            Investigate,
            Alert,
            Combat,
        }

    }
}
