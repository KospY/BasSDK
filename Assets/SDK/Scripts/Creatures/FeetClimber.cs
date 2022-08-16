using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FeetClimber")]
    [AddComponentMenu("ThunderRoad/Creatures/Feet climber")]
    public class FeetClimber : ThunderBehaviour
    {
        public float footSpeed = 4;
        public float sweepAngle = -70;
        public float sweepMinDelay = 0.5f;
        public float sweepMaxVerticalAngle = 30;
        public float sweepMaxHorizontalAngle = 30;

        public float sphereCastRadius = 0.05f;
        public float moveOutWeight = 0.2f;

        public float legLenghtMultiplier = 1.3f;
        public float minFootSpacing = 0.2f;

        public float legToHeadMaxAngle = 30;
        public float footMaxAngle = 45;
        public bool showDebug;

    }
}
