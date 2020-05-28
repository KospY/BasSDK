using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class HandPoseDefinition : MonoBehaviour
    {
        public Vector3 gripLocalPosition = new Vector3(-0.08f, -0.025f, 0.01f);
        public Vector3 gripLocalRotation = new Vector3(0, 120, 85);

        public Side side = Side.Right;
        public HandleDefinition handleReference;
        public int handleOrientationIndex = 0;

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
            Animator animator = this.GetComponentInParent<Animator>();
            Transform rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
            Transform leftHandBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);

            if (this.transform.parent.parent != rightHandBone)
            {
                Debug.LogError("HandPoseDefinition is not correctly placed, should be HandRightBone/BodyHand/HandPoseDefinition");
                return;
            }

            handleReference.CheckOrientations();
            HandleOrientation handleOrientation = handleReference.orientationDefaultRight;
            if (side == Side.Left) handleOrientation = handleReference.orientationDefaultLeft;

            if (handleOrientationIndex > 0)
            {
                int i = 0;
                foreach (HandleOrientation ho in handleReference.orientations)
                {
                    if (handleReference.orientationDefaultLeft == ho) continue;
                    if (handleReference.orientationDefaultRight == ho) continue;
                    if (ho.side == side)
                    {
                        if ((handleOrientationIndex - 1) == i) handleOrientation = ho;
                        i++;
                    }
                }
            }

            if (handleOrientation)
            {
                ItemDefinition objectDefinition = handleReference.GetComponentInParent<ItemDefinition>();

                Transform objectGrip = new GameObject("ObjectGrip").transform;
                objectGrip.SetParent(objectDefinition ? objectDefinition.transform : handleReference.transform.root);
                objectGrip.position = handleOrientation.transform.position + (handleOrientation.handleDefinition.transform.up * handleOrientation.handleDefinition.GetDefaultAxisLocalPosition());
                objectGrip.rotation = handleOrientation.transform.rotation;

                Transform alignObject = handleReference.transform.root;
                if (objectDefinition != null) alignObject = objectDefinition.transform;

                Transform handGripRight = this.transform.parent.Find("HandGrip");
                if (!handGripRight)
                {
                    handGripRight = new GameObject("HandGrip").transform;
                    handGripRight.SetParent(this.transform.parent);
                }
                handGripRight.localPosition = gripLocalPosition;
                handGripRight.localRotation = Quaternion.Euler(gripLocalRotation);

                if (side == Side.Right)
                {
                    alignObject.MoveAlign(objectGrip, handGripRight.position, handGripRight.rotation);
                }
                else
                {
                    Transform bodyHandLeft = leftHandBone.Find("BodyHand");
                    if (!bodyHandLeft)
                    {
                        bodyHandLeft = new GameObject("BodyHand").transform;
                        bodyHandLeft.SetParent(leftHandBone);
                        bodyHandLeft.localPosition = Vector3.zero;
                        bodyHandLeft.localRotation = Quaternion.Euler(90, 0, 0);
                    }
                    Transform handGripLeft = bodyHandLeft.Find("HandGrip");
                    if (!handGripLeft)
                    {
                        handGripLeft = new GameObject("HandGrip").transform;
                        handGripLeft.SetParent(bodyHandLeft);
                    }
                    handGripLeft.localPosition = gripLocalPosition;
                    handGripLeft.localRotation = Quaternion.Euler(gripLocalRotation);
                    handGripLeft.MirrorRelativeToParent(new Vector3(-1, 1, 1));
                    handGripLeft.localScale = Vector3.one;
                    alignObject.MoveAlign(objectGrip, handGripLeft.position, handGripLeft.rotation);
                }

                DestroyImmediate(objectGrip.gameObject);
            }
            else
            {
                Debug.LogError("No orientation set on the handle!");
            }
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
            // Right
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

            // Left
            HandPoseDefinition leftPose = Instantiate(this.gameObject, animator.GetBoneTransform(HumanBodyBones.LeftHand)).GetComponent<HandPoseDefinition>();
            leftPose.transform.localPosition = Vector3.zero;
            leftPose.transform.localRotation = Quaternion.Euler(90, 0, 180);
            leftPose.Mirror();
            animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal).position = leftPose.thumbProximal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal).rotation = leftPose.thumbProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate).position = leftPose.thumbIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate).rotation = leftPose.thumbIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal).position = leftPose.thumbDistal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal).rotation = leftPose.thumbDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal).position = leftPose.indexProximal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal).rotation = leftPose.indexProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate).position = leftPose.indexIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate).rotation = leftPose.indexIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal).position = leftPose.indexDistal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal).rotation = leftPose.indexDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal).position = leftPose.middleProximal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal).rotation = leftPose.middleProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate).position = leftPose.middleIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate).rotation = leftPose.middleIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal).position = leftPose.middleDistal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal).rotation = leftPose.middleDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.LeftRingProximal).position = leftPose.ringProximal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftRingProximal).rotation = leftPose.ringProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate).position = leftPose.ringIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate).rotation = leftPose.ringIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftRingDistal).position = leftPose.ringDistal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftRingDistal).rotation = leftPose.ringDistal.rotation;

            animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal).position = leftPose.littleProximal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal).rotation = leftPose.littleProximal.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate).position = leftPose.littleIntermediate.position;
            animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate).rotation = leftPose.littleIntermediate.rotation;
            animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal).position = leftPose.littleDistal.position;
            animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal).rotation = leftPose.littleDistal.rotation;
            DestroyImmediate(leftPose.gameObject);
        }

        [Button]
        public void Mirror()
        {
            this.transform.MirrorChilds(new Vector3(1, -1, 1));
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
