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
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Ragdoll")]
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll")]
    public class Ragdoll : ThunderBehaviour
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

        [Header("Player arm")]
        public float playerArmPositionSpring = 5000f;
        public float playerArmPositionDamper = 40f;
        public float playerArmRotationSpring = 1000f;
        public float playerArmRotationDamper = 40f;
        public float playerArmMaxPositionForce = 3000f;
        public float playerArmMaxRotationForce = 250f;

        [Header("Collision")]
        public float collisionEffectMinDelay = 0.2f;
        public float collisionMinVelocity = 2.0f;
        [NonSerialized]
        public float lastCollisionEffectTime;

        [Header("Misc")]
        public bool allowSelfDamage;
        public bool grippable = true;

        [NonSerialized]
        public Creature creature;

    }
}
