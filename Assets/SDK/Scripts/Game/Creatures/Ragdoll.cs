using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll")]
    public class Ragdoll : MonoBehaviour
    {
        public Transform meshRig;
        public Transform meshRootBone;

        [Header("Parts")]
        public RagdollPart headPart;
        public RagdollPart leftUpperArmPart;
        public RagdollPart rightUpperArmPart;
        public RagdollPart targetPart;
        public RagdollPart rootPart;

        [Header("Default forces")]
        public float springPositionForce = 1000;
        public float damperPositionForce = 50;
        public float maxPositionForce = 1000;

        public float springRotationForce = 800;
        public float damperRotationForce = 50;
        public float maxRotationForce = 100;

        [Header("Destabilized")]
        public float destabilizedSpringRotationMultiplier = 0.5f;
        public float destabilizedDamperRotationMultiplier = 0.1f;
        public float destabilizedGroundSpringRotationMultiplier = 0.2f;

        [Header("HipsAttached")]
        public float hipsAttachedSpringPositionMultiplier = 1f;
        public float hipsAttachedDamperPositionMultiplier = 0f;
        public float hipsAttachedSpringRotationMultiplier = 1f;
        public float hipsAttachedDamperRotationMultiplier = 0f;

        [Header("StandUp")]
        public AnimationCurve standUpCurve;
        public float standUpFromGrabDuration = 1.0f;
        public float preStandUpDuration = 3f;
        public float preStandUpRatio = 0.7f;

        [Header("Melee")]
        public float meleeArmSpringMultiplier = 2.0f;
        public float meleeArmMaxForceMultiplier = 10.0f;
        public float parryArmSpringMultiplier = 1.0f;
        public float parryArmMaxForceMultiplier = 10.0f;
        public float aimArmSpringMultiplier = 0.5f;

        [Header("Player arm")]
        public float playerArmPositionSpring = 5000f;
        public float playerArmPositionDamper = 40f;
        public float playerArmRotationSpring = 1000f;
        public float playerArmRotationDamper = 40f;
        public float playerArmMaxPositionForce = 3000f;
        public float playerArmMaxRotationForce = 250f;

        [Header("Collision")]
        public float collisionMinDelay = 0.2f;
        public float collisionMinVelocity = 2.0f;

        [Header("Grab")]
        public float grabThrowMinVelocity = 2.0f;
        public float grabSpeedMultiplier = 0.5f;
        public float grabFallHeightMultiplier = 0.3f;

        [Header("Misc")]
        public bool allowSelfDamage;
        public bool grippable = true;

    }
}