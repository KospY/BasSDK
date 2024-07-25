using UnityEngine;
using System;
using UnityEngine.AI;
using ThunderRoad.AI;
using System.Collections.Generic;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleExpression : BrainData.Module
    {
        [Serializable]
        public class ExpressionVariance
        {
            public FaceAnimator.Expression expression;
            public List<FaceAnimator.Expression> expressionsActive;
            public float variance = 0.05f;
            public AnimationCurve weightCurve = new AnimationCurve();
            public bool factorMood = false;
#if ODIN_INSPECTOR
            [ShowIf("factorMood")]
#endif
            public float moodMultiplier = 0.1f;
#if ODIN_INSPECTOR
            [ShowIf("factorMood")]
#endif
            public Vector2 moodFactorMinMax = new Vector2(-10f, 10f);

            [NonSerialized]
            public float lastValue;
        }
        [Header("Mood and microexpressions")]
        public float moodBase = 0f;
        public bool persistantNPCMood = false;
        public string NPCMoodSaveKey = "NPCMood";
        public float varianceUpdateCoolown = 1.5f;
        public bool setZeroVarianceOnReach = true;
        public float reachTolerance = 0.0005f;
        public List<ExpressionVariance> expressionVariance;
        [Header("Speech")]
        public int defaultSpeakFaceIndex = 0;
        public float neutralExpressionResetTime = 5f;
        [Header("Pain and death")]
        public float painExpressionDuration = 0.3f;

    }
}
