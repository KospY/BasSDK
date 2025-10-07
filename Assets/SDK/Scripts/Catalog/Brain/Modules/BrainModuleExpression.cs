using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


using TriInspector;
using Newtonsoft.Json;

namespace ThunderRoad
{
    [System.Serializable]
    public class BrainModuleExpression : BrainData.Module
    {
        [Serializable]
        public class ExpressionVariance
        {
            public FaceAnimator.Expression expression;
            public List<FaceAnimator.Expression> expressionsActive;
            public float variance = 0.05f;
            public AnimationCurve weightCurve = new();
            public bool factorMood = false;
            [ShowIf("factorMood")]
            public float moodMultiplier = 0.1f;
            [ShowIf("factorMood")]
            public Vector2 moodFactorMinMax = new(-10f, 10f);

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
        public float delayHideSubtitleDuration = 0.6f;
        [Header("Pain and death")]
        public float painExpressionDuration = 0.3f;

    }
}
