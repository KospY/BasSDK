using UnityEngine;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[AddComponentMenu("ThunderRoad/Creatures/Wrist relaxer")]
    public class WristRelaxer : MonoBehaviour
    {
        public Transform armTwistBone;
        public Transform upperArmBone;
        public Transform lowerArmBone;
        public Transform handBone;

        [Tooltip("The weight of relaxing the twist")]
        [Range(0f, 1f)]
        public float weight = 1f;

        [Tooltip("The weight of relaxing the arm of this Transform (UMA)")]
        [Range(0f, 1f)]
        public float weightArm = 0.5f;

        [Tooltip("If 0.5, will be twisted half way from parent to child. If 1, the twist angle will be locked to the child and will rotate with along with it.")]
        [Range(0f, 1f)]
        public float parentChildCrossfade = 0.5f;

        [Tooltip("Rotation offset around the twist axis.")]
        [Range(-180f, 180f)]
        public float twistAngleOffset;

        private void OnValidate()
        {
            if (!upperArmBone || !lowerArmBone || !handBone || !armTwistBone)
            {
                RagdollHand ragdollHand = this.GetComponentInParent<RagdollHand>();
                Creature creature = this.GetComponentInParent<Creature>();
                if (creature && creature.animator && ragdollHand)
                {
                    if (!upperArmBone) upperArmBone = creature.animator.GetBoneTransform(ragdollHand.side == Side.Right ? HumanBodyBones.RightUpperArm : HumanBodyBones.LeftUpperArm);
                    if (!lowerArmBone) lowerArmBone = creature.animator.GetBoneTransform(ragdollHand.side == Side.Right ? HumanBodyBones.RightLowerArm : HumanBodyBones.LeftLowerArm);
                    if (!armTwistBone) armTwistBone = creature.animator.GetBoneTransform(ragdollHand.side == Side.Right ? HumanBodyBones.RightLowerArm : HumanBodyBones.LeftLowerArm).GetChild(0);
                    if (!handBone) handBone = creature.animator.GetBoneTransform(ragdollHand.side == Side.Right ? HumanBodyBones.RightHand : HumanBodyBones.LeftHand);
                }
            }
        }

    }
}