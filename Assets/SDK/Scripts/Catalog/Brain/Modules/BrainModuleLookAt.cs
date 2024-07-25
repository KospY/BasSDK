using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleLookAt : BrainData.Module
    {
        [Header("Blending")]
        public float weightChangeSpeed = 2f;
        public float defaultBlendTime = 0.5f;
        public AnimationCurve defaultBlendCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float snapPreventionAngle = 30f;
        public float forceUprightSpeed = 1f;

        [Header("Controls")]
        public bool customEyeTargetOnly = false;
        public float lookMaxAngle = 80f;

        [Header("IK")]
        public bool forceAimPostIK = true;
        public bool uprightWhileMoving = true;
        public IKWeight torso = new IKWeight()
        {
            currentWeight = 0f,
            clampWeight = 0.7f,
            targetWeight = 0f,
        };
        public IKWeight head = new IKWeight()
        {
            currentWeight = 0f,
            clampWeight = 0.75f,
            targetWeight = 0f,
        };
        public IKWeight eyes = new IKWeight()
        {
            currentWeight = 0f,
            clampWeight = 0.95f,
            targetWeight = 0f,
        };
        public IKWeight spine = new IKWeight()
        {
            currentWeight = 0f,
            clampWeight = 0.9f,
            targetWeight = 0f,
        };

        [System.Serializable]
        public class IKWeight
        {
#if ODIN_INSPECTOR
            [ShowInInspector] 
#endif
            [Range(0f, 1f), NonSerialized]
            public float currentWeight;
            [Range(0f, 1f)]
            public float clampWeight;
            [Range(0f, 1f)]
            public float targetWeight;
            [NonSerialized]
            public Action<float> solverWeightSetter;

            public void SetCurrentWeight(float weight)
            {
                currentWeight = weight;
                solverWeightSetter(weight);
            }
        }

        [Header("Death")]
        public Vector3 eyesDeathPosition = new Vector3(-3f, 0f, 3f);
        public float deathEyeRollDuration = 5f;

        public enum BodyBehaviour
        {
            UseLookIK,
            BodyUpright,
            None
        }

    }
}