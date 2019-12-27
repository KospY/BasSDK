using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    public class HandPoseDefinition : MonoBehaviour
    {
        public Vector3 mirrorAxis = new Vector3(1, -1, 1);

        public Side side = Side.Right;
        public Vector3 gripLocalPosition = new Vector3(-0.08f, -0.025f, 0.01f);
        public Vector3 gripLocalRotation = new Vector3(0, 120, 85);
        public HandleDefinition handleReference;

        [HideInInspector]
        public Transform thumbDistal;
        [HideInInspector]
        public Transform thumbIntermediate;
        [HideInInspector]
        public Transform thumbProximal;
        [HideInInspector]
        public Transform indexDistal;
        [HideInInspector]
        public Transform indexIntermediate;
        [HideInInspector]
        public Transform indexProximal;
        [HideInInspector]
        public Transform middleDistal;
        [HideInInspector]
        public Transform middleIntermediate;
        [HideInInspector]
        public Transform middleProximal;
        [HideInInspector]
        public Transform ringDistal;
        [HideInInspector]
        public Transform ringIntermediate;
        [HideInInspector]
        public Transform ringProximal;
        [HideInInspector]
        public Transform littleDistal;
        [HideInInspector]
        public Transform littleIntermediate;
        [HideInInspector]
        public Transform littleProximal;

        protected virtual void OnValidate()
        {

            if (!thumbProximal) thumbProximal = this.transform.Find("thumbProximal"); if (!thumbProximal) { thumbProximal = new GameObject("thumbProximal").transform; thumbProximal.SetParent(this.transform, false); }
            if (!thumbIntermediate) thumbIntermediate = thumbProximal.transform.Find("thumbIntermediate"); if (!thumbIntermediate) { thumbIntermediate = new GameObject("thumbIntermediate").transform; thumbIntermediate.SetParent(thumbProximal, false); }
            if (!thumbDistal) thumbDistal = thumbIntermediate.transform.Find("thumbDistal"); if (!thumbDistal) { thumbDistal = new GameObject("thumbDistal").transform; thumbDistal.SetParent(thumbIntermediate, false); }

            if (!indexProximal) indexProximal = this.transform.Find("indexProximal"); if (!indexProximal) { indexProximal = new GameObject("indexProximal").transform; indexProximal.SetParent(this.transform, false); }
            if (!indexIntermediate) indexIntermediate = indexProximal.transform.Find("indexIntermediate"); if (!indexIntermediate) { indexIntermediate = new GameObject("indexIntermediate").transform; indexIntermediate.SetParent(indexProximal, false); }
            if (!indexDistal) indexDistal = indexIntermediate.transform.Find("indexDistal"); if (!indexDistal) { indexDistal = new GameObject("indexDistal").transform; indexDistal.SetParent(indexIntermediate, false); }

            if (!middleProximal) middleProximal = this.transform.Find("middleProximal"); if (!middleProximal) { middleProximal = new GameObject("middleProximal").transform; middleProximal.SetParent(this.transform, false); }
            if (!middleIntermediate) middleIntermediate = middleProximal.transform.Find("middleIntermediate"); if (!middleIntermediate) { middleIntermediate = new GameObject("middleIntermediate").transform; middleIntermediate.SetParent(middleProximal, false); }
            if (!middleDistal) middleDistal = middleIntermediate.transform.Find("middleDistal"); if (!middleDistal) { middleDistal = new GameObject("middleDistal").transform; middleDistal.SetParent(middleIntermediate, false); }

            if (!ringProximal) ringProximal = this.transform.Find("ringProximal"); if (!ringProximal) { ringProximal = new GameObject("ringProximal").transform; ringProximal.SetParent(this.transform, false); }
            if (!ringIntermediate) ringIntermediate = ringProximal.transform.Find("ringIntermediate"); if (!ringIntermediate) { ringIntermediate = new GameObject("ringIntermediate").transform; ringIntermediate.SetParent(ringProximal, false); }
            if (!ringDistal) ringDistal = ringIntermediate.transform.Find("ringDistal"); if (!ringDistal) { ringDistal = new GameObject("ringDistal").transform; ringDistal.SetParent(ringIntermediate, false); }

            if (!littleProximal) littleProximal = this.transform.Find("littleProximal"); if (!littleProximal) { littleProximal = new GameObject("littleProximal").transform; littleProximal.SetParent(this.transform, false); }
            if (!littleIntermediate) littleIntermediate = littleProximal.transform.Find("littleIntermediate"); if (!littleIntermediate) { littleIntermediate = new GameObject("littleIntermediate").transform; littleIntermediate.SetParent(littleProximal, false); }
            if (!littleDistal) littleDistal = littleIntermediate.transform.Find("littleDistal"); if (!littleDistal) { littleDistal = new GameObject("littleDistal").transform; littleDistal.SetParent(littleIntermediate, false); }
        }

        [Button]
        public void AlignObject()
        {
            ItemDefinition objectDefinition = handleReference.GetComponentInParent<ItemDefinition>();

            Vector3 orgHandleLocalPostion = handleReference.transform.localPosition;
            Quaternion orgHandleLocalRotation = handleReference.transform.localRotation;

            handleReference.transform.position = handleReference.GetDefaultAxisPosition(side);
            handleReference.transform.position = handleReference.transform.TransformPoint(handleReference.GetDefaultOrientation(side).positionOffset);
            handleReference.transform.rotation = handleReference.transform.rotation * Quaternion.Euler(handleReference.GetDefaultOrientation(side).rotation);
            handleReference.transform.position = handleReference.transform.TransformPoint(new Vector3(side == Side.Right ? handleReference.ikAnchorOffset.x : -handleReference.ikAnchorOffset.x, handleReference.ikAnchorOffset.y, handleReference.ikAnchorOffset.z));

            if (objectDefinition != null) objectDefinition.transform.MoveAlign(handleReference.transform, this.transform.TransformPoint(gripLocalPosition), this.transform.rotation * Quaternion.Euler(gripLocalRotation));
            else handleReference.transform.root.MoveAlign(handleReference.transform, this.transform.TransformPoint(gripLocalPosition), this.transform.rotation * Quaternion.Euler(gripLocalRotation));

            handleReference.transform.localPosition = orgHandleLocalPostion;
            handleReference.transform.localRotation = orgHandleLocalRotation;
        }

        [Button]
        protected virtual void CopyFromAnimator()
        {
            Animator animator = this.GetComponentInParent<Animator>();
            thumbProximal.position = animator.GetBoneTransform(HumanBodyBones.RightThumbProximal).position;
            thumbProximal.rotation = animator.GetBoneTransform(HumanBodyBones.RightThumbProximal).rotation;
            thumbIntermediate.position = animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).position;
            thumbIntermediate.rotation = animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).rotation;
            thumbDistal.position = animator.GetBoneTransform(HumanBodyBones.RightThumbDistal).position;
            thumbDistal.rotation = animator.GetBoneTransform(HumanBodyBones.RightThumbDistal).rotation;

            indexProximal.position = animator.GetBoneTransform(HumanBodyBones.RightIndexProximal).position;
            indexProximal.rotation = animator.GetBoneTransform(HumanBodyBones.RightIndexProximal).rotation;
            indexIntermediate.position = animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).position;
            indexIntermediate.rotation = animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).rotation;
            indexDistal.position = animator.GetBoneTransform(HumanBodyBones.RightIndexDistal).position;
            indexDistal.rotation = animator.GetBoneTransform(HumanBodyBones.RightIndexDistal).rotation;

            middleProximal.position = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal).position;
            middleProximal.rotation = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal).rotation;
            middleIntermediate.position = animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).position;
            middleIntermediate.rotation = animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).rotation;
            middleDistal.position = animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal).position;
            middleDistal.rotation = animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal).rotation;

            ringProximal.position = animator.GetBoneTransform(HumanBodyBones.RightRingProximal).position;
            ringProximal.rotation = animator.GetBoneTransform(HumanBodyBones.RightRingProximal).rotation;
            ringIntermediate.position = animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate).position;
            ringIntermediate.rotation = animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate).rotation;
            ringDistal.position = animator.GetBoneTransform(HumanBodyBones.RightRingDistal).position;
            ringDistal.rotation = animator.GetBoneTransform(HumanBodyBones.RightRingDistal).rotation;

            littleProximal.position = animator.GetBoneTransform(HumanBodyBones.RightLittleProximal).position;
            littleProximal.rotation = animator.GetBoneTransform(HumanBodyBones.RightLittleProximal).rotation;
            littleIntermediate.position = animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).position;
            littleIntermediate.rotation = animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).rotation;
            littleDistal.position = animator.GetBoneTransform(HumanBodyBones.RightLittleDistal).position;
            littleDistal.rotation = animator.GetBoneTransform(HumanBodyBones.RightLittleDistal).rotation;
        }

        [Button]
        protected virtual void CopyToAnimator()
        {
            Animator animator = this.GetComponentInParent<Animator>();
            animator.GetBoneTransform(HumanBodyBones.RightThumbProximal).position = thumbProximal.position;
            animator.GetBoneTransform(HumanBodyBones.RightThumbProximal).rotation = thumbProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).position = thumbIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).rotation = thumbIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightThumbDistal).position = thumbDistal.position;
            animator.GetBoneTransform(HumanBodyBones.RightThumbDistal).rotation = thumbDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.RightIndexProximal).position = indexProximal.position;
            animator.GetBoneTransform(HumanBodyBones.RightIndexProximal).rotation = indexProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).position = indexIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).rotation = indexIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightIndexDistal).position = indexDistal.position;
            animator.GetBoneTransform(HumanBodyBones.RightIndexDistal).rotation = indexDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal).position = middleProximal.position;
            animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal).rotation = middleProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).position = middleIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).rotation = middleIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal).position = middleDistal.position;
            animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal).rotation = middleDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.RightRingProximal).position = ringProximal.position;
            animator.GetBoneTransform(HumanBodyBones.RightRingProximal).rotation = ringProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate).position = ringIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate).rotation = ringIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightRingDistal).position = ringDistal.position;
            animator.GetBoneTransform(HumanBodyBones.RightRingDistal).rotation = ringDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.RightLittleProximal).position = littleProximal.position;
            animator.GetBoneTransform(HumanBodyBones.RightLittleProximal).rotation = littleProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).position = littleIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).rotation = littleIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.RightLittleDistal).position = littleDistal.position;
            animator.GetBoneTransform(HumanBodyBones.RightLittleDistal).rotation = littleDistal.rotation;
        }

        [Button]
        public void Mirror()
        {
            this.transform.Mirror(mirrorAxis);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(thumbDistal.position, 0.001f);
            Gizmos.DrawWireSphere(thumbIntermediate.position, 0.001f);
            Gizmos.DrawWireSphere(thumbProximal.position, 0.001f);
            Gizmos.DrawLine(thumbDistal.position, thumbIntermediate.position);
            Gizmos.DrawLine(thumbIntermediate.position, thumbProximal.position);
            Gizmos.DrawLine(thumbProximal.position, this.transform.position);

            Gizmos.DrawWireSphere(indexDistal.position, 0.001f);
            Gizmos.DrawWireSphere(indexIntermediate.position, 0.001f);
            Gizmos.DrawWireSphere(indexProximal.position, 0.001f);
            Gizmos.DrawLine(indexDistal.position, indexIntermediate.position);
            Gizmos.DrawLine(indexIntermediate.position, indexProximal.position);
            Gizmos.DrawLine(indexProximal.position, this.transform.position);

            Gizmos.DrawWireSphere(middleDistal.position, 0.001f);
            Gizmos.DrawWireSphere(middleIntermediate.position, 0.001f);
            Gizmos.DrawWireSphere(middleProximal.position, 0.001f);
            Gizmos.DrawLine(middleDistal.position, middleIntermediate.position);
            Gizmos.DrawLine(middleIntermediate.position, middleProximal.position);
            Gizmos.DrawLine(middleProximal.position, this.transform.position);

            Gizmos.DrawWireSphere(ringDistal.position, 0.001f);
            Gizmos.DrawWireSphere(ringIntermediate.position, 0.001f);
            Gizmos.DrawWireSphere(ringProximal.position, 0.001f);
            Gizmos.DrawLine(ringDistal.position, ringIntermediate.position);
            Gizmos.DrawLine(ringIntermediate.position, ringProximal.position);
            Gizmos.DrawLine(ringProximal.position, this.transform.position);

            Gizmos.DrawWireSphere(littleDistal.position, 0.001f);
            Gizmos.DrawWireSphere(littleIntermediate.position, 0.001f);
            Gizmos.DrawWireSphere(littleProximal.position, 0.001f);
            Gizmos.DrawLine(littleDistal.position, littleIntermediate.position);
            Gizmos.DrawLine(littleIntermediate.position, littleProximal.position);
            Gizmos.DrawLine(littleProximal.position, this.transform.position);
        }
    }
}
