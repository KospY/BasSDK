using UnityEngine;
using System.Collections;
using ThunderRoad.AI.Action;
using System;

namespace ThunderRoad
{
    public class BrainModuleBow : BrainModuleRanged
    {
        public float maxShootAngle = 1;
        public float arrowDrawDelay = 0.5f;
        public float arrowNockDelay = 1f;
        public float bowDrawDelay = 1f;
        public float aimHitDropDelay = 0.1f;
        public Vector3 elbowIKGoal = new Vector3(2f, 0.05f, -0.2f);

    }
}