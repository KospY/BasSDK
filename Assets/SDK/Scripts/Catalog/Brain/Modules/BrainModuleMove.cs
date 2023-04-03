using UnityEngine;
using System;
using UnityEngine.AI;
using ThunderRoad.AI;
using System.Collections.Generic;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class BrainModuleMove : BrainData.Module
    {
        [Header("General")]
        public bool allowMove = true;
        public float runDistance = 3;
        public bool useAcceleration = true;
        public float acceleration = 0.3f;
        public float navReachDistance = 0.05f;
        public float strafeMinDelay = 0;
        public float strafeMaxDelay = 5;
        public int samplePositionCircleSteps = 6;
        public NavmeshArea areaMask = ~NavmeshArea.NotWalkable;

        [Header("Close or stuck detection")]
        public float stuckRadius = 0.01f;
        public float stuckMaxDuration = 0.5f;
        public bool preventTooClose = true;
        public float tooCloseDistance = 0.7f;
        public float edgeAvoidanceDistance = 0.1f;

        [Header("Locomotion override")]
        public bool locomotionOverride;
        public float locomotionForwardSpeedMultiplier = 1f;
        public float locomotionBackwardSpeedMultiplier = 1f;
        public float locomotionStrafeSpeedMultiplier = 1f;
        public float locomotionRunSpeedMultiplier = 1f;

        [Header("Doors")]
        public float reachTime = 0.5f;
        public AnimationCurve reachCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public string doorReachClip = "Bas.Animation.Interaction.ReachDoorHandleLeft";

    }
}