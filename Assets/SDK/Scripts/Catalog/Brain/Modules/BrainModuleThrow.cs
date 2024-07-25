using UnityEngine;
using System;
using System.Collections.Generic;
using ThunderRoad.AI.Action;
using System.Linq;
using System.Collections;
using ThunderRoad.Skill.SpellPower;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleThrow : BrainModuleRanged
    {
        [Header("Rock throwing")]
        public bool rockThrowEnabled = true;
        public Vector3 rockSpawnPositioning = new Vector3(0f, 0f, 0.1f);
        public float rockGrabTime = 0.5f;
        public Vector2 rockGrabLookIKAdjustDurations = new Vector2(0.5f, 0.25f);

    }
}