using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class SkillTree : MonoBehaviour
    {
        [ColorUsage(true, true)]
        public Color ringEmissionColor = Color.white;
        [ColorUsage(true, true)]
        public Color monolithEmissionColor = Color.white;

        public bool crystalChangeRingColor = true;
        public float colorChangeSpeed = 1;

        public float ringRotationForce = 3f;
        public float ringRotationTargetVelocity = 20f;
        public float ringAutoRotationPlayerDistance = 2f;
        public float ringAutoRotationTargetVelocity = 3f;
        protected JointMotor ringJointMotor;

        public float showTreeDelay = 1;

        public float showTreeDuration = 2;
        public float hideTreeDuration = 0.5f;

        public float floorHeight = 0.2f;
        public float circleRadius = 0.1f;
        public float crossOuterDistance = 0.5f;

        public float positionSpring = 30;
        public float positionDamper = 20f;
        public float positionMaxForce = 100f;

        public float rotationSpring = 1f;
        public float rotationDamper = 0.1f;
        public float rotationMaxForce = 100f;

        public float ringSleepThreshold = 0.01f;

        public Renderer ringSmall;
        public Renderer ringMedium;
        public Renderer ringReceptacles;
        public Renderer monolith;

    }
}