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
        public enum PushAnimation
        {
            Default,
            Parry,
            Head,
            Torso,
            Legs,
            FallGround,
        }

        public enum DestabilizeType
        {
            Magic,
            Grab,
            Hit,
            Knockout,
            Parry,
        }

    }
}
