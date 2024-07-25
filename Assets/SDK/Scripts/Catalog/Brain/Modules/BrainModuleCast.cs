using UnityEngine;
using ThunderRoad.AI;
using ThunderRoad.AI.Action;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleCast : BrainData.Module
    {
#if ODIN_INSPECTOR
        [MinMaxSlider(0, 60, showFields: true)] 
#endif
        public Vector2 castMinMaxDelay = new Vector2(8, 16);

        public float aimArmLengthMultiplier = 0.99f;

        public float minMana = 5;
        public float maxAimAngle = 50;
        public float chargeDurationMultiplier = 1f;
        public bool usePositionIK = true;
        public bool useRotationIK = false;
        public AnimationCurve IKAdjustCurve;
        public float IKMaxWeight = 0.7f;
        public float throwEarlyRelease = 0.1f;
        public Vector2 spreadCone = new Vector2(0.6f, 0.6f);
        public float maxFireAngle = 60;

#if ODIN_INSPECTOR
        [BoxGroup("Probability")] 
#endif
        public Vector2 shortRangeMinMaxDist = new Vector2(0, 5);
#if ODIN_INSPECTOR
        [BoxGroup("Probability")] 
#endif
        public Vector2 midRangeMinMaxDist = new Vector2(5, 8);
#if ODIN_INSPECTOR
        [BoxGroup("Probability")] 
#endif
        public Vector2 longRangeMinMaxDist = new Vector2(8, 20);

    }
}