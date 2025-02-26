using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollHandPoser")]
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll hand poser"), RequireComponent(typeof(RagdollHand))]
    public class RagdollHandPoser : ThunderBehaviour
    {
        public RagdollHand ragdollHand;

        [CatalogPicker(new[] { Category.HandPose })]
        public string defaultHandPoseId = "DefaultOpen";

        [Range(0f, 1f)]
        public float targetWeight;

        [CatalogPicker(new[] { Category.HandPose })]
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

        public HandPoseData.Pose.MirrorParams mirrorParams;
        
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

#if UNITY_EDITOR
        [CatalogPicker(new[] { Category.HandPose })]
        public string saveHandPoseId = "NewHandPose";
        protected string lastSaveHandPoseId;

        public void OnValidate()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif
            if (!gameObject.activeInHierarchy) return;
            if (Application.isBatchMode) return;
            if (ragdollHand == null) ragdollHand = GetComponentInParent<RagdollHand>();
            if (ragdollHand.creature)
            {
                Catalog.EditorLoadAllJson();

                if (ragdollHand.creature.data == null)
                {
                    ragdollHand.creature.data = Catalog.GetData<CreatureData>(ragdollHand.creature.creatureId);
                }

                if (defaultHandPoseData == null || defaultHandPoseData.id != defaultHandPoseId)
                {
                    defaultHandPoseData = Catalog.GetData<HandPoseData>(defaultHandPoseId);
                }

                if (targetHandPoseData == null || targetHandPoseData.id != targetHandPoseId)
                {
                    targetHandPoseData = Catalog.GetData<HandPoseData>(targetHandPoseId);
                }
                if (lastSaveHandPoseId == saveHandPoseId)
                {
                    // Avoid refresh when selecting a save pose ID
                    EditorRefreshPose(ragdollHand.creature);
                }
                lastSaveHandPoseId = saveHandPoseId;
            }
        }

        public void EditorRefreshPose(Creature creature)
        {
            if (ragdollHand.creature.data == null)
            {
                ragdollHand.creature.data = Catalog.GetData<CreatureData>(ragdollHand.creature.creatureId);
            }

            if (defaultHandPoseData != null)
            {
                HandPoseData.Pose pose = defaultHandPoseData.GetCreaturePose(ragdollHand.creature);
                if (pose != null)
                {
                    defaultHandPoseFingers = pose.GetFingers(ragdollHand.side);
                }
                else
                {
                    Debug.LogError($"Could not find creature pose {defaultHandPoseData.id} for {ragdollHand.creature.data.name}");
                }
            }
            if (targetHandPoseData != null)
            {
                HandPoseData.Pose pose = targetHandPoseData.GetCreaturePose(ragdollHand.creature);
                if (pose != null)
                {
                    targetHandPoseFingers = pose.GetFingers(ragdollHand.side);
                    hasTargetHandPose = true;
                }
                else
                {
                    Debug.LogError($"Could not find creature pose {targetHandPoseData.id} for {ragdollHand.creature.data.name}");
                    hasTargetHandPose = false;
                }
            }
            else
            {
                hasTargetHandPose = false;
            }

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
            if (handPoseId != null && handPoseId != "")
            {
                if (ragdollHand.creature.data == null)
                {
                    ragdollHand.creature.data = Catalog.GetData<CreatureData>(ragdollHand.creature.creatureId);
                }
                HandPoseData handPoseData = Catalog.GetData<HandPoseData>(handPoseId, false);
                if (handPoseData == null)
                {
                    Debug.Log("Handpose " + handPoseId + " doesn't exist, creating a new one...");
                    handPoseData = new HandPoseData();
                    handPoseData.id = handPoseId;
                    handPoseData.Init();
                    handPoseData.OnCatalogRefresh();
                    Catalog.GetDataList(Category.HandPose).Add(handPoseData);
                    handPoseData.AddCreaturePose(ragdollHand.creature);
                }

                HandPoseData.Pose pose = handPoseData.GetCreaturePose(ragdollHand.creature);
                if (pose == null)
                {
                    handPoseData.AddCreaturePose(ragdollHand.creature.data.name);
                }

                HandPoseData.Pose.Fingers handCreaturePoseFingers = handPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
                SaveFinger(handCreaturePoseFingers.thumb, ragdollHand.fingerThumb);
                SaveFinger(handCreaturePoseFingers.index, ragdollHand.fingerIndex);
                SaveFinger(handCreaturePoseFingers.middle, ragdollHand.fingerMiddle);
                SaveFinger(handCreaturePoseFingers.ring, ragdollHand.fingerRing);
                SaveFinger(handCreaturePoseFingers.little, ragdollHand.fingerLittle);
                handCreaturePoseFingers.gripLocalPosition = ragdollHand.grip.localPosition;
                handCreaturePoseFingers.gripLocalRotation = ragdollHand.grip.localRotation;
                handCreaturePoseFingers.rootLocalPosition = ragdollHand.grip.InverseTransformPoint(ragdollHand.transform.position);
                handPoseData.GetCreaturePose(ragdollHand.creature).Save(ragdollHand.side, mirrorParams);
                Catalog.SaveToJson(handPoseData);
                Debug.Log("Handpose " + handPoseId + " Saved!");
            }
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
            thumbCloseWeight = targetWeight;
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerThumb, defaultHandPoseFingers.thumb, targetHandPoseFingers.thumb, targetWeight);
            else UpdateFinger(ragdollHand.fingerThumb, defaultHandPoseFingers.thumb);
        }

        public void UpdatePoseIndex(float targetWeight)
        {
            indexCloseWeight = targetWeight;
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerIndex, defaultHandPoseFingers.index, targetHandPoseFingers.index, targetWeight);
            else UpdateFinger(ragdollHand.fingerIndex, defaultHandPoseFingers.index);
        }

        public void UpdatePoseMiddle(float targetWeight)
        {
            middleCloseWeight = targetWeight;
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerMiddle, defaultHandPoseFingers.middle, targetHandPoseFingers.middle, targetWeight);
            else UpdateFinger(ragdollHand.fingerMiddle, defaultHandPoseFingers.middle);
        }

        public void UpdatePoseRing(float targetWeight)
        {
            ringCloseWeight = targetWeight;
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerRing, defaultHandPoseFingers.ring, targetHandPoseFingers.ring, targetWeight);
            else UpdateFinger(ragdollHand.fingerRing, defaultHandPoseFingers.ring);
        }

        public void UpdatePoseLittle(float targetWeight)
        {
            littleCloseWeight = targetWeight;
            if (hasTargetHandPose) UpdateFinger(ragdollHand.fingerLittle, defaultHandPoseFingers.little, targetHandPoseFingers.little, targetWeight);
            else UpdateFinger(ragdollHand.fingerLittle, defaultHandPoseFingers.little);
        }

        public void UpdatePose(HandPoseData.FingerType finger, float weight)
        {
            UpdateFinger(ragdollHand.GetFinger(finger), defaultHandPoseFingers.GetFinger(finger));
        }
        public virtual void UpdateFinger(RagdollHand.Finger finger, HandPoseData.Pose.Finger defaultHandPoseFingers, HandPoseData.Pose.Finger targetHandPoseFingers, float targetWeight)
        {

            HandPoseData.Pose.Finger.Bone proximal = defaultHandPoseFingers.proximal;
            Vector3 proximalPos = Vector3.Lerp(proximal.localPosition, targetHandPoseFingers.proximal.localPosition, targetWeight);
            Quaternion proximalRot = Quaternion.Lerp(proximal.localRotation, targetHandPoseFingers.proximal.localRotation, targetWeight);
            Transform proximalTransform = finger.proximal.collider.transform;
            proximalTransform.SetPositionAndRotation(ragdollHand.transform.TransformPoint(proximalPos), ragdollHand.transform.TransformRotation(proximalRot));

            Vector3 intermediatePos = Vector3.Lerp(defaultHandPoseFingers.intermediate.localPosition, targetHandPoseFingers.intermediate.localPosition, targetWeight);
            Quaternion intermediateRot = Quaternion.Lerp(defaultHandPoseFingers.intermediate.localRotation, targetHandPoseFingers.intermediate.localRotation, targetWeight);
            Transform intermediateTransform = finger.intermediate.collider.transform;
            intermediateTransform.SetLocalPositionAndRotation(intermediatePos, intermediateRot);

            Vector3 distalPos = Vector3.Lerp(defaultHandPoseFingers.distal.localPosition, targetHandPoseFingers.distal.localPosition, targetWeight);
            Quaternion distalRot = Quaternion.Lerp(defaultHandPoseFingers.distal.localRotation, targetHandPoseFingers.distal.localRotation, targetWeight);
            Transform distalTransform = finger.distal.collider.transform;
            distalTransform.SetLocalPositionAndRotation(distalPos, distalRot);

#if UNITY_EDITOR
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
#endif            
        }

        public virtual void UpdateFinger(RagdollHand.Finger finger, HandPoseData.Pose.Finger defaultHandPoseFingers)
        {
            Transform proximalTransform = finger.proximal.collider.transform;
            proximalTransform.SetLocalPositionAndRotation(defaultHandPoseFingers.proximal.localPosition, defaultHandPoseFingers.proximal.localRotation);
            Transform intermediateTransform = finger.intermediate.collider.transform;
            intermediateTransform.SetLocalPositionAndRotation(defaultHandPoseFingers.intermediate.localPosition, defaultHandPoseFingers.intermediate.localRotation);
            Transform distalTransform = finger.distal.collider.transform;
            distalTransform.SetLocalPositionAndRotation(defaultHandPoseFingers.distal.localPosition, defaultHandPoseFingers.distal.localRotation);
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

            HandPoseData.Pose pose = handPoseData.GetCreaturePose(ragdollHand.creature);
            if (pose != null)
            {
                Transform handGrip = ragdollHand.grip;
                HandPoseData.Pose.Fingers fingers = pose.GetFingers(ragdollHand.side);
                handGrip.SetLocalPositionAndRotation(fingers.gripLocalPosition, fingers.gripLocalRotation);
                handGrip.localScale = Vector3.one;
            }
            else
            {
                Debug.LogError($"Could not find creature pose {handPoseData.id} for {ragdollHand.creature.data.name}");
            }
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
            defaultHandPoseData = Catalog.GetData<HandPoseData>(defaultHandPoseId);
            defaultHandPoseFingers = defaultHandPoseData.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
        }

        public bool poseComplete;

        public void SetTargetWeight(float weight, bool lerpFingers = false)
        {
            targetWeight = weight;

            if (lerpFingers)
            {
                float ragdollDataFingerSpeed = ragdollHand.ragdoll.creature.data.ragdollData.fingerSpeed;
                float deltaTime = ragdollHand.ragdoll.creature.isPlayer ? Time.unscaledDeltaTime : Time.deltaTime;

                UpdateFingerLerp(ref thumbCloseWeight, UpdatePoseThumb);
                UpdateFingerLerp(ref indexCloseWeight, UpdatePoseIndex);
                UpdateFingerLerp(ref middleCloseWeight, UpdatePoseMiddle);
                UpdateFingerLerp(ref ringCloseWeight, UpdatePoseRing);
                UpdateFingerLerp(ref littleCloseWeight, UpdatePoseLittle);

                return;

                // this saves some LoC...
                void UpdateFingerLerp(ref float closeWeight, Action<float> updatePoseFunc)
                {
                    closeWeight = Mathf.MoveTowards(closeWeight, weight, ragdollDataFingerSpeed * deltaTime);
                    updatePoseFunc(closeWeight);
                }
            }

            // thumbCloseWeight = indexCloseWeight = middleCloseWeight = ringCloseWeight = targetWeight;
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

        public float GetCloseWeight(HandPoseData.FingerType type)
        {
            return type switch
            {
                HandPoseData.FingerType.Thumb => thumbCloseWeight,
                HandPoseData.FingerType.Index => indexCloseWeight,
                HandPoseData.FingerType.Middle => middleCloseWeight,
                HandPoseData.FingerType.Ring => ringCloseWeight,
                HandPoseData.FingerType.Little => littleCloseWeight,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}