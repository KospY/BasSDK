using UnityEngine;
using System;
using UnityEngine.UIElements;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollHandPoser")]
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll hand poser"), RequireComponent(typeof(RagdollHand))]
    public class RagdollHandPoser : ThunderBehaviour
    {
        public RagdollHand ragdollHand;

        public string defaultHandPoseId = "DefaultOpen";

        [Range(0f, 1f)]
        public float targetWeight;

        public string targetHandPoseId = "DefaultClose";

        public bool globalRatio = true;
        [Range(0f, 1f)]
        public float thumbCloseWeight;
        [Range(0f, 1f)]
        public float indexCloseWeight;
        [Range(0f, 1f)]
        public float middleCloseWeight;
        [Range(0f, 1f)]
        public float ringCloseWeight;
        [Range(0f, 1f)]
        public float littleCloseWeight;

        public bool allowThumbTracking = true;
        public bool allowIndexTracking = true;
        public bool allowMiddleTracking = true;
        public bool allowRingTracking = true;
        public bool allowLittleTracking = true;

        [NonSerialized]
        public bool thumbPositionReached;
        [NonSerialized]
        public bool indexPositionReached;
        [NonSerialized]
        public bool middlePositionReached;
        [NonSerialized]
        public bool ringPositionReached;
        [NonSerialized]
        public bool littlePositionReached;

        [NonSerialized]
        public HandPoseData defaultHandPoseData;
        [NonSerialized]
        public HandPoseData.Pose.Fingers defaultHandPoseFingers;

        [NonSerialized]
        public HandPoseData targetHandPoseData;
        [NonSerialized]
        public HandPoseData.Pose.Fingers targetHandPoseFingers;
        [NonSerialized]
        public bool hasTargetHandPose;
        protected Coroutine moveToFingerPoseCoroutine;

#if UNITY_EDITOR
        public string saveHandPoseId = "NewHandPose";

        public void OnValidate()
        {
            if (ragdollHand == null) ragdollHand = GetComponentInParent<RagdollHand>();
            if (ragdollHand.creature)
            {

                EditorRefreshPose(ragdollHand.creature);
            }
        }

        public void EditorRefreshPose(Creature creature)
        {

            if (defaultHandPoseFingers != null && targetHandPoseFingers != null)
            {
                if (targetHandPoseId != null && targetHandPoseId != "" && targetHandPoseData != null)
                {
                    UpdatePoseThumb(globalRatio ? targetWeight : thumbCloseWeight);
                    UpdatePoseIndex(globalRatio ? targetWeight : indexCloseWeight);
                    UpdatePoseMiddle(globalRatio ? targetWeight : middleCloseWeight);
                    UpdatePoseRing(globalRatio ? targetWeight : ringCloseWeight);
                    UpdatePoseLittle(globalRatio ? targetWeight : littleCloseWeight);
                }
                else
                {
                    UpdatePoseThumb(0);
                    UpdatePoseIndex(0);
                    UpdatePoseMiddle(0);
                    UpdatePoseRing(0);
                    UpdatePoseLittle(0);
                }
            }
        }

        [Button]
        public void SaveCurrentHandPose()
        {
            SaveToHandPose(saveHandPoseId);
        }

        public void SaveToHandPose(string handPoseId)
        {
        }

        protected virtual void SaveFinger(HandPoseData.Pose.Finger poseFinger, RagdollHand.Finger finger)
        {
            poseFinger.proximal.localPosition = finger.proximal.mesh.transform.localPosition;
            poseFinger.proximal.localRotation = finger.proximal.mesh.transform.localRotation;
            poseFinger.intermediate.localPosition = finger.intermediate.mesh.transform.localPosition;
            poseFinger.intermediate.localRotation = finger.intermediate.mesh.transform.localRotation;
            poseFinger.distal.localPosition = finger.distal.mesh.transform.localPosition;
            poseFinger.distal.localRotation = finger.distal.mesh.transform.localRotation;
            poseFinger.tipLocalPosition = finger.tip.localPosition;
        }
#endif

        public void UpdatePoseThumb(float targetWeight)
        {
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerThumb, defaultHandPoseFingers.thumb, targetHandPoseFingers.thumb, targetWeight);
            else UpdateFinger(ragdollHand.fingerThumb, defaultHandPoseFingers.thumb);
        }

        public void UpdatePoseIndex(float targetWeight)
        {
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerIndex, defaultHandPoseFingers.index, targetHandPoseFingers.index, targetWeight);
            else UpdateFinger(ragdollHand.fingerIndex, defaultHandPoseFingers.index);
        }

        public void UpdatePoseMiddle(float targetWeight)
        {
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerMiddle, defaultHandPoseFingers.middle, targetHandPoseFingers.middle, targetWeight);
            else UpdateFinger(ragdollHand.fingerMiddle, defaultHandPoseFingers.middle);
        }

        public void UpdatePoseRing(float targetWeight)
        {
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerRing, defaultHandPoseFingers.ring, targetHandPoseFingers.ring, targetWeight);
            else UpdateFinger(ragdollHand.fingerRing, defaultHandPoseFingers.ring);
        }

        public void UpdatePoseLittle(float targetWeight)
        {
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerLittle, defaultHandPoseFingers.little, targetHandPoseFingers.little, targetWeight);
            else UpdateFinger(ragdollHand.fingerLittle, defaultHandPoseFingers.little);
        }

        public virtual void UpdateFinger(RagdollHand.Finger finger, HandPoseData.Pose.Finger defaultHandPoseFingers, HandPoseData.Pose.Finger targetHandPoseFingers, float targetWeight)
        {
            Vector3 proximalPos = Vector3.Lerp(defaultHandPoseFingers.proximal.localPosition, targetHandPoseFingers.proximal.localPosition, targetWeight);
            Quaternion proximalRot = Quaternion.Lerp(defaultHandPoseFingers.proximal.localRotation, targetHandPoseFingers.proximal.localRotation, targetWeight);
            Transform proximalTransform = finger.proximal.collider.transform;
            proximalTransform.localPosition = proximalPos;
            proximalTransform.localRotation = proximalRot;

            Vector3 intermediatePos = Vector3.Lerp(defaultHandPoseFingers.intermediate.localPosition, targetHandPoseFingers.intermediate.localPosition, targetWeight);
            Quaternion intermediateRot = Quaternion.Lerp(defaultHandPoseFingers.intermediate.localRotation, targetHandPoseFingers.intermediate.localRotation, targetWeight);
            Transform intermediateTransform = finger.intermediate.collider.transform;
            intermediateTransform.localPosition = intermediatePos;
            intermediateTransform.localRotation = intermediateRot;

            Vector3 distalPos = Vector3.Lerp(defaultHandPoseFingers.distal.localPosition, targetHandPoseFingers.distal.localPosition, targetWeight);
            Quaternion distalRot = Quaternion.Lerp(defaultHandPoseFingers.distal.localRotation, targetHandPoseFingers.distal.localRotation, targetWeight);
            Transform distalTransform = finger.distal.collider.transform;
            distalTransform.localPosition = distalPos;
            distalTransform.localRotation = distalRot;

            if (!Application.isPlaying)
            {
                proximalTransform = finger.proximal.mesh.transform;
                proximalTransform.localPosition = proximalPos;
                proximalTransform.localRotation = proximalRot;
                intermediateTransform = finger.intermediate.mesh.transform;
                intermediateTransform.localPosition = intermediatePos;
                intermediateTransform.localRotation = intermediateRot;
                distalTransform = finger.distal.mesh.transform;
                distalTransform.localPosition = distalPos;
                distalTransform.localRotation = distalRot;
            }
        }

        public virtual void UpdateFinger(RagdollHand.Finger finger, HandPoseData.Pose.Finger defaultHandPoseFingers)
        {
            Transform proximalTransform = finger.proximal.collider.transform;
            proximalTransform.localPosition = defaultHandPoseFingers.proximal.localPosition;
            proximalTransform.localRotation = defaultHandPoseFingers.proximal.localRotation;
            Transform intermediateTransform = finger.intermediate.collider.transform;
            intermediateTransform.localPosition = defaultHandPoseFingers.intermediate.localPosition;
            intermediateTransform.localRotation = defaultHandPoseFingers.intermediate.localRotation;
            Transform distalTransform = finger.distal.collider.transform;
            distalTransform.localPosition = defaultHandPoseFingers.distal.localPosition;
            distalTransform.localRotation = defaultHandPoseFingers.distal.localRotation;
        }

        public void SetGripFromPose(HandPoseData handPoseData)
        {
            if (handPoseData == null)
            {
                if (defaultHandPoseData == null)
                    ResetDefaultPose();

                handPoseData = defaultHandPoseData;
                if (handPoseData == null) return;
            }

            ragdollHand.grip.localPosition = handPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side).gripLocalPosition;
            ragdollHand.grip.localRotation = handPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side).gripLocalRotation;
            ragdollHand.grip.localScale = Vector3.one;
        }

        public void SetDefaultPose(HandPoseData handPoseData)
        {
            if (handPoseData == null)
            {
                ResetDefaultPose();
                return;
            }

            defaultHandPoseData = handPoseData;
            defaultHandPoseFingers = defaultHandPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
        }

        public void ResetDefaultPose()
        {
            defaultHandPoseFingers = defaultHandPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
        }

        public void SetTargetPose(HandPoseData handPoseData, bool allowThumbTracking = false, bool allowIndexTracking = false, bool allowMiddleTracking = false, bool allowRingTracking = false, bool allowLittleTracking = false)
        {
            if (handPoseData == null)
            {
                ResetTargetPose();
                return;
            }

            targetHandPoseData = handPoseData;
            if (targetHandPoseData == null)
            {
                targetHandPoseFingers = null;
                hasTargetHandPose = false;
                this.allowThumbTracking = this.allowIndexTracking = this.allowMiddleTracking = this.allowRingTracking = this.allowLittleTracking = false;
            }
            else
            {
                targetHandPoseFingers = targetHandPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
                hasTargetHandPose = true;
                this.allowThumbTracking = allowThumbTracking;
                this.allowIndexTracking = allowIndexTracking;
                this.allowMiddleTracking = allowMiddleTracking;
                this.allowRingTracking = allowRingTracking;
                this.allowLittleTracking = allowLittleTracking;
            }
        }

        public void ResetTargetPose()
        {

            if (targetHandPoseData == null)
            {
                targetHandPoseFingers = null;
                hasTargetHandPose = false;
                allowThumbTracking = allowIndexTracking = allowMiddleTracking = allowRingTracking = allowLittleTracking = false;
            }
            else
            {
                targetHandPoseFingers = targetHandPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
                hasTargetHandPose = true;
                allowThumbTracking = allowIndexTracking = allowMiddleTracking = allowRingTracking = allowLittleTracking = true;
            }
        }

        public void SetTargetWeight(float weight)
        {
            targetWeight = weight;
            UpdatePoseThumb(targetWeight);
            UpdatePoseIndex(targetWeight);
            UpdatePoseMiddle(targetWeight);
            UpdatePoseRing(targetWeight);
            UpdatePoseLittle(targetWeight);
        }


        protected void OnDrawGizmosSelected()
        {
            foreach (RagdollHand.Finger finger in ragdollHand.fingers)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(finger.distal.collider.transform.position, 0.001f);
                Gizmos.DrawWireSphere(finger.intermediate.collider.transform.position, 0.001f);
                Gizmos.DrawWireSphere(finger.proximal.collider.transform.position, 0.001f);
                Gizmos.DrawWireSphere(finger.tip.position, 0.001f);
                Gizmos.DrawLine(this.transform.position, finger.proximal.collider.transform.position);
                Gizmos.DrawLine(finger.proximal.collider.transform.position, finger.intermediate.collider.transform.position);
                Gizmos.DrawLine(finger.intermediate.collider.transform.position, finger.distal.collider.transform.position);
                Gizmos.DrawLine(finger.distal.collider.transform.position, finger.tip.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(finger.tip.position, finger.tip.forward * 0.01f);
                Gizmos.color = Color.green;
                Gizmos.DrawRay(finger.tip.position, finger.tip.up * 0.01f);
            }
            if (ragdollHand.grip)
            {
                Gizmos.matrix = ragdollHand.grip.localToWorldMatrix;
                Gizmos.color = Common.HueColourValue(HueColorName.Purple);
                Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(0.01f, 0.05f, 0.01f));
                Gizmos.DrawWireCube(new Vector3(0f, 0.03f, 0.01f), new Vector3(0.01f, 0.01f, 0.03f));
            }
        }
    }
}