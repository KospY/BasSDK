using System;
using UnityEngine;

namespace ThunderRoad
{
    [Serializable]
    public class ExpressionData : CatalogData
    {
        public AnimationCurve curve;
        [Range(-1f, 1f)]
        public float neckUp_Down = 0f;
        [Range(-1f, 1f)]
        public float neckLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float neckTiltLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float headUp_Down = 0f;
        [Range(-1f, 1f)]
        public float headLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float headTiltLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float jawOpen_Close = 0f;
        [Range(-1f, 1f)]
        public float jawForward_Back = 0f;
        [Range(-1f, 1f)]
        public float jawLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float mouthLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float mouthUp_Down = 0f;
        [Range(-1f, 1f)]
        public float mouthNarrow_Pucker = 0f;
        [Range(-1f, 1f)]
        public float tongueOut = 0f;
        [Range(0f, 1f)]
        public float tongueCurl = 0f;
        [Range(-1f, 1f)]
        public float tongueUp_Down = 0f;
        [Range(-1f, 1f)]
        public float tongueLeft_Right = 0f;
        [Range(-1f, 1f)]
        public float tongueWide_Narrow = 0f;
        [Range(-1f, 1f)]
        public float leftMouthSmile_Frown = 0f;
        [Range(-1f, 1f)]
        public float rightMouthSmile_Frown = 0f;
        [Range(-1f, 1f)]
        public float leftLowerLipUp_Down = 0f;
        [Range(-1f, 1f)]
        public float rightLowerLipUp_Down = 0f;
        [Range(-1f, 1f)]
        public float leftUpperLipUp_Down = 0f;
        [Range(-1f, 1f)]
        public float rightUpperLipUp_Down = 0f;
        [Range(-1f, 1f)]
        public float leftCheekPuff_Squint = 0f;
        [Range(-1f, 1f)]
        public float rightCheekPuff_Squint = 0f;
        [Range(0f, 1f)]
        public float noseSneer = 0f;
        [Range(-1f, 1f)]
        public float leftEyeOpen_Close = 0f;
        [Range(-1f, 1f)]
        public float rightEyeOpen_Close = 0f;
        [Range(-1f, 1f)]
        public float leftEyeUp_Down = 0f;
        [Range(-1f, 1f)]
        public float rightEyeUp_Down = 0f;
        [Range(-1f, 1f)]
        public float leftEyeIn_Out = 0f;
        [Range(-1f, 1f)]
        public float rightEyeIn_Out = 0f;
        [Range(0f, 1f)]
        public float browsIn = 0f;
        [Range(-1f, 1f)]
        public float leftBrowUp_Down = 0f;
        [Range(-1f, 1f)]
        public float rightBrowUp_Down = 0f;
        [Range(-1f, 1f)]
        public float midBrowUp_Down = 0f;
    }
}
