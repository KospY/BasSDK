using System;
using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleDodge : BrainData.Module
    {
        public bool enabled = true;
        public bool dodgeStabForce = true;


        public float dodgeChance = 0.1f;

        public float dodgeStabMaxAngle = 20;
        public float dodgeThreatMinSpeed = 3f;
        public float dodgeThreatDistance = 1f;
        public float dodgeForceDistance = 0.25f;
        public float dodgeMaxHeight = 0.2f;
        public float dodgeSpeed = 1f;
        public bool dodgeWhenGrabbed = false;
        public bool dodgeWhenWeaponGrabbed = false;

        public enum Dodge
        {
            Back,
            Forward,
            Right,
            Left,
            BackHigh,
            BackMed,
            BackLow,
        }

        public enum DodgeBehaviour
        {
            None,
            LowOnly,
            NotParrying,
            Always,
        }

    }
}