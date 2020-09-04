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
        public float runDistance = 3f;
        public float navRagdollMult = 2f;
        public bool useAcceleration;
        public float acceleration = 0.3f;
        public float actionCycleSpeed = 1;

    }
}
