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
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll part")]
    [RequireComponent(typeof(CollisionHandler))]
    public class RagdollPart : MonoBehaviour
    {
        [Header("Part")]
        public Transform meshBone;
        public Type type;
        public bool sliceAllowed;
        public float sliceWidth = 0.04f;
        public float sliceJointBreakForce = 5000;
        public Transform sliceTransform;
        public Material sliceFillMaterial;

        public float springPositionMultiplier = 1;
        public float damperPositionMultiplier = 1;
        public float springRotationMultiplier = 1;
        public float damperRotationMultiplier = 1;

        public List<RagdollPart> ignoredParts;

        public enum Type
        {
            None,
            Head,
            Neck,
            Torso,
            LeftArm,
            RightArm,
            LeftHand,
            RightHand,
            LeftLeg,
            RightLeg,
            LeftFoot,
            RightFoot,
        }

    }
}