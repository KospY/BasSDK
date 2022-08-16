using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ZiplineWheel")]
    public class ZiplineWheel : MonoBehaviour
    {
        [Header("References")]
        public List<Handle> handles;
        public HingeJoint wheelHingeJoint;
        public Rigidbody wheelRigidbody;

        [Header("Motor")]
        public bool allowMotor;
        public Handle motorTriggerHandle;
        public float motorMaxVelocity = 5000;
        public FxController motorFxController;

        [Header("Friction")]
        public Collider wheelCenterCollider;
        public Collider wheelRailLeftCollider;
        public Collider wheelRailRightCollider;

        public float wheelRailOnZiplineFriction = 0f;
        public float wheelCenterOnZiplineFriction = 0.2f;
        public float wheelCenterMotorFriction = 1f;

        [Header("Rope detection")]
        public string ropeTag = "Zipline";

        [Header("On zipline")]
        public float hingeConnectedMassScaleMultiplier = 1f;
        public float wheelMassMultiplier = 1f;
        public bool forcePhysicJoint = true;
        public bool debugEnterExitZiplineSound;

    }
}
