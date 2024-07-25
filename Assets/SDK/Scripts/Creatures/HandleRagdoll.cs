using System;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/HandleRagdoll")]
    [AddComponentMenu("ThunderRoad/Creatures/Handle ragdoll")]
    public class HandleRagdoll : Handle
    {
        public static float combatScaleChangeTime = 0.75f;

        public bool canBeEscaped = false;
        public bool wasTkGrabbed = false;
        public int grappleEscapeParameterValue = 0;
        public float escapeDelay = 0f;
    }
}
