using UnityEngine;

namespace BS
{
    public class HandPoseDefinition : MonoBehaviour
    {
        public Vector3 mirrorAxis = new Vector3(1, -1, 1);

        [Header("Transforms")]
        public Transform thumbDistal;
        public Transform thumbIntermediate;
        public Transform thumbProximal;

        public Transform indexDistal;
        public Transform indexIntermediate;
        public Transform indexProximal;

        public Transform middleDistal;
        public Transform middleIntermediate;
        public Transform middleProximal;

        public Transform ringDistal;
        public Transform ringIntermediate;
        public Transform ringProximal;

        public Transform littleDistal;
        public Transform littleIntermediate;
        public Transform littleProximal;

        [Header("Helpers")]
        public Animator animatorReference;
        public HandleDefinition handleReference;

        protected virtual void OnValidate()
        {
            if (!thumbProximal) { thumbProximal = new GameObject("thumbProximal").transform; thumbProximal.SetParent(this.transform, false); }
            if (!thumbIntermediate) { thumbIntermediate = new GameObject("thumbIntermediate").transform; thumbIntermediate.SetParent(thumbProximal, false); }
            if (!thumbDistal) { thumbDistal = new GameObject("thumbDistal").transform; thumbDistal.SetParent(thumbIntermediate, false); }

            if (!indexProximal) { indexProximal = new GameObject("indexProximal").transform; indexProximal.SetParent(this.transform, false); }
            if (!indexIntermediate) { indexIntermediate = new GameObject("indexIntermediate").transform; indexIntermediate.SetParent(indexProximal, false); }
            if (!indexDistal) { indexDistal = new GameObject("indexDistal").transform; indexDistal.SetParent(indexIntermediate, false); }

            if (!middleProximal) { middleProximal = new GameObject("middleProximal").transform; middleProximal.SetParent(this.transform, false); }
            if (!middleIntermediate) { middleIntermediate = new GameObject("middleIntermediate").transform; middleIntermediate.SetParent(middleProximal, false); }
            if (!middleDistal) { middleDistal = new GameObject("middleDistal").transform; middleDistal.SetParent(middleIntermediate, false); }

            if (!ringProximal) { ringProximal = new GameObject("ringProximal").transform; ringProximal.SetParent(this.transform, false); }
            if (!ringIntermediate) { ringIntermediate = new GameObject("ringIntermediate").transform; ringIntermediate.SetParent(ringProximal, false); }
            if (!ringDistal) { ringDistal = new GameObject("ringDistal").transform; ringDistal.SetParent(ringIntermediate, false); }

            if (!littleProximal) { littleProximal = new GameObject("littleProximal").transform; littleProximal.SetParent(this.transform, false); }
            if (!littleIntermediate) { littleIntermediate = new GameObject("littleIntermediate").transform; littleIntermediate.SetParent(littleProximal, false); }
            if (!littleDistal) { littleDistal = new GameObject("littleDistal").transform; littleDistal.SetParent(littleIntermediate, false); }
        }

        [ContextMenu("Align object")]
        public void AlignObject()
        {
            Vector3 gripLocalPosition = new Vector3(-0.08f, 0.015f, -0.03f);
            Vector3 gripLocalRotation = new Vector3(150, 90, 180);
            ItemDefinition objectDefinition = handleReference.GetComponentInParent<ItemDefinition>();
            objectDefinition.transform.MoveAlign(handleReference.transform, this.transform.TransformPoint(gripLocalPosition), this.transform.rotation * Quaternion.Euler(gripLocalRotation));
        }

        [ContextMenu("Copy from animator")]
        protected virtual void CopyFromAnimator()
        {
            thumbDistal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightThumbDistal).localPosition;
            thumbDistal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightThumbDistal).localRotation;
            thumbIntermediate.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).localPosition;
            thumbIntermediate.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).localRotation;
            thumbProximal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightThumbProximal).localPosition;
            thumbProximal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightThumbProximal).localRotation;

            indexDistal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightIndexDistal).localPosition;
            indexDistal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightIndexDistal).localRotation;
            indexIntermediate.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).localPosition;
            indexIntermediate.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).localRotation;
            indexProximal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightIndexProximal).localPosition;
            indexProximal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightIndexProximal).localRotation;

            middleDistal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleDistal).localPosition;
            middleDistal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleDistal).localRotation;
            middleIntermediate.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).localPosition;
            middleIntermediate.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).localRotation;
            middleProximal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleProximal).localPosition;
            middleProximal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleProximal).localRotation;

            ringDistal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightRingDistal).localPosition;
            ringDistal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightRingDistal).localRotation;
            ringIntermediate.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightRingIntermediate).localPosition;
            ringIntermediate.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightRingIntermediate).localRotation;
            ringProximal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightRingProximal).localPosition;
            ringProximal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightRingProximal).localRotation;

            littleDistal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightLittleDistal).localPosition;
            littleDistal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightLittleDistal).localRotation;
            littleIntermediate.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).localPosition;
            littleIntermediate.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).localRotation;
            littleProximal.localPosition = animatorReference.GetBoneTransform(HumanBodyBones.RightLittleProximal).localPosition;
            littleProximal.localRotation = animatorReference.GetBoneTransform(HumanBodyBones.RightLittleProximal).localRotation;
        }

        [ContextMenu("Copy to animator")]
        protected virtual void CopyToAnimator()
        {
            animatorReference.GetBoneTransform(HumanBodyBones.RightThumbDistal).localPosition = thumbDistal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightThumbDistal).localRotation = thumbDistal.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).localPosition = thumbIntermediate.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).localRotation = thumbIntermediate.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightThumbProximal).localPosition = thumbProximal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightThumbProximal).localRotation = thumbProximal.localRotation;

            animatorReference.GetBoneTransform(HumanBodyBones.RightIndexDistal).localPosition = indexDistal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightIndexDistal).localRotation = indexDistal.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).localPosition = indexIntermediate.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).localRotation = indexIntermediate.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightIndexProximal).localPosition = indexProximal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightIndexProximal).localRotation = indexProximal.localRotation;

            animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleDistal).localPosition = middleDistal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleDistal).localRotation = middleDistal.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).localPosition = middleIntermediate.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).localRotation = middleIntermediate.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleProximal).localPosition = middleProximal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightMiddleProximal).localRotation = middleProximal.localRotation;

            animatorReference.GetBoneTransform(HumanBodyBones.RightRingDistal).localPosition = ringDistal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightRingDistal).localRotation = ringDistal.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightRingIntermediate).localPosition = ringIntermediate.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightRingIntermediate).localRotation = ringIntermediate.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightRingProximal).localPosition = ringProximal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightRingProximal).localRotation = ringProximal.localRotation;

            animatorReference.GetBoneTransform(HumanBodyBones.RightLittleDistal).localPosition = littleDistal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightLittleDistal).localRotation = littleDistal.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).localPosition = littleIntermediate.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).localRotation = littleIntermediate.localRotation;
            animatorReference.GetBoneTransform(HumanBodyBones.RightLittleProximal).localPosition = littleProximal.localPosition;
            animatorReference.GetBoneTransform(HumanBodyBones.RightLittleProximal).localRotation = littleProximal.localRotation;
        }

        [ContextMenu("Mirror")]
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
