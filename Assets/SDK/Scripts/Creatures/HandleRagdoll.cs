using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/HandleRagdoll")]
    [AddComponentMenu("ThunderRoad/Creatures/Handle ragdoll")]
    public class HandleRagdoll : Handle
    {
        public bool canBeEscaped = false;
        public int grappleEscapeParameterValue = 0;
        public float escapeDelay = 0f;
    }
}
