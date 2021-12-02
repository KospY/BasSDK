using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll hand")]
    public class RagdollHand : RagdollPart
    {
        [Header("Hand")]
        public Side side = Side.Right;
        public RagdollPart lowerArmPart;
        public RagdollPart upperArmPart;

        public bool meshFixedScale = true;
        public Vector3 meshGlobalScale = Vector3.one;

        public Vector3 axisThumb = Vector3.up;
        public Vector3 axisPalm = Vector3.left;

        public Collider touchCollider;

        public WristStats wristStats;

        [Header("Fingers")]
        public Finger fingerThumb = new Finger();
        public Finger fingerIndex = new Finger();
        public Finger fingerMiddle = new Finger();
        public Finger fingerRing = new Finger();
        public Finger fingerLittle = new Finger();
        public List<Finger> fingers = new List<Finger>();
        public Collider palmCollider;
        public Collider simplifiedCollider;

        [Serializable]
        public class Finger
        {
            public Bone proximal = new Bone();
            public Bone intermediate = new Bone();
            public Bone distal = new Bone();
            public Transform tip;

            [Serializable]
            public class Bone
            {
                public Transform mesh;
                public Transform animation;
                public CapsuleCollider collider;
            }
        }

        [NonSerialized]
        public RagdollHand otherHand;
        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public Transform grip;
        [NonSerialized]
        public SpellCaster caster;


        protected override void OnValidate()
        {
            base.OnValidate();
            IconManager.SetIcon(this.gameObject, null);
            if (!this.gameObject.activeInHierarchy) return;
            grip = this.transform.Find("Grip");
            if (!grip) grip = CreateDefaultGrip();
            if (editPose == null) editPose = new EditPose(this);
            if (editPose.ragdollHand == null) editPose = new EditPose(this);
            if (creature == null) creature = this.GetComponentInParent<Creature>();
            if (creature)
            {
                if (openPose) openPoseFingers = openPose.GetFingers(side);
                if (closePose) closePoseFingers = closePose.GetFingers(side);
                if (openPose && closePose)
                {
                    UpdatePoseThumb(globalRatio ? closeWeight : thumbCloseWeight);
                    UpdatePoseIndex(globalRatio ? closeWeight : indexCloseWeight);
                    UpdatePoseMiddle(globalRatio ? closeWeight : middleCloseWeight);
                    UpdatePoseRing(globalRatio ? closeWeight : ringCloseWeight);
                    UpdatePoseLittle(globalRatio ? closeWeight : littleCloseWeight);
                }
            }
        }

        [Button]
        public void MirrorFingersToOtherHand()
        {
            if (!creature) creature = this.GetComponentInParent<Creature>();
            foreach (RagdollHand ragdollHand in creature.GetComponentsInChildren<RagdollHand>())
            {
                if (ragdollHand != this)
                {
                    otherHand = ragdollHand;
                    break;
                }
            }
            DestroyImmediate(otherHand.palmCollider.gameObject);
            GameObject otherFingers = Instantiate(palmCollider.gameObject, otherHand.transform);
            otherFingers.name = palmCollider.name;
            otherFingers.transform.MirrorChilds(new Vector3(1, -1, 1));
            // Dirty fix to avoid negative scale
            foreach (Transform transform in otherFingers.GetComponentsInChildren<Transform>())
            {
                transform.localScale = Vector3.one;
            }
            otherHand.SetupFingers();
        }

        [Button]
        public virtual void SetGripToDefaultPosition()
        {
            Transform grip = this.transform.Find("Grip");
            if (grip) DestroyImmediate(grip.gameObject);
            grip = CreateDefaultGrip();
        }

        public virtual Transform CreateDefaultGrip()
        {
            Transform newGrip = new GameObject("Grip").transform;
            newGrip.transform.SetParent(this.transform);
            newGrip.transform.localScale = Vector3.one;
            if (side == Side.Left)
            {
                newGrip.transform.localPosition = new Vector3(-0.042f, -0.01f, 0.003f);
                newGrip.transform.localRotation = Quaternion.Euler(0, 220, -90);
            }
            if (side == Side.Right)
            {
                newGrip.transform.localPosition = new Vector3(0.042f, -0.01f, 0.003f);
                newGrip.transform.localRotation = Quaternion.Euler(0, 140, 90);
            }
            return newGrip;
        }

        [Button]
        public virtual void SetupFingers()
        {
            creature = this.GetComponentInParent<Creature>();

            fingers = new List<Finger>();
            Transform meshBone = null;

            palmCollider = this.transform.Find("Palm")?.GetComponent<Collider>();
            if (palmCollider == null)
            {
                palmCollider = new GameObject("Palm").AddComponent<BoxCollider>();
                palmCollider.transform.SetParentOrigin(this.transform);
                (palmCollider as BoxCollider).size = new Vector3(0.1f, 0.1f, 0.03f);
                palmCollider.gameObject.AddComponent<ColliderGroup>();
            }

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbProximal : HumanBodyBones.LeftThumbProximal); if (meshBone) fingerThumb.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbIntermediate : HumanBodyBones.LeftThumbIntermediate); if (meshBone) fingerThumb.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbDistal : HumanBodyBones.LeftThumbDistal); if (meshBone) fingerThumb.distal.mesh = meshBone;
            SetupFinger(fingerThumb, "Thumb");
            if (fingerThumb.proximal.mesh) fingers.Add(fingerThumb);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexProximal : HumanBodyBones.LeftIndexProximal); if (meshBone) fingerIndex.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexIntermediate : HumanBodyBones.LeftIndexIntermediate); if (meshBone) fingerIndex.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexDistal : HumanBodyBones.LeftIndexDistal); if (meshBone) fingerIndex.distal.mesh = meshBone;
            SetupFinger(fingerIndex, "Index");
            if (fingerIndex.proximal.mesh) fingers.Add(fingerIndex);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleProximal : HumanBodyBones.LeftMiddleProximal); if (meshBone) fingerMiddle.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleIntermediate : HumanBodyBones.LeftMiddleIntermediate); if (meshBone) fingerMiddle.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleDistal : HumanBodyBones.LeftMiddleDistal); if (meshBone) fingerMiddle.distal.mesh = meshBone;
            SetupFinger(fingerMiddle, "Middle");
            if (fingerMiddle.proximal.mesh) fingers.Add(fingerMiddle);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingProximal : HumanBodyBones.LeftRingProximal); if (meshBone) fingerRing.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingIntermediate : HumanBodyBones.LeftRingIntermediate); if (meshBone) fingerRing.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingDistal : HumanBodyBones.LeftRingDistal); if (meshBone) fingerRing.distal.mesh = meshBone;
            SetupFinger(fingerRing, "Ring");
            if (fingerRing.proximal.mesh) fingers.Add(fingerRing);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleProximal : HumanBodyBones.LeftLittleProximal); if (meshBone) fingerLittle.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleIntermediate : HumanBodyBones.LeftLittleIntermediate); if (meshBone) fingerLittle.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleDistal : HumanBodyBones.LeftLittleDistal); if (meshBone) fingerLittle.distal.mesh = meshBone;
            SetupFinger(fingerLittle, "Little");
            if (fingerLittle.proximal.mesh) fingers.Add(fingerLittle);
        }

        protected virtual void SetupFinger(Finger finger, string name)
        {
            // Ragdoll proximal
            if (!finger.proximal.collider) finger.proximal.collider = palmCollider.transform.Find(name + "Proximal")?.GetComponent<CapsuleCollider>();
            if (!finger.proximal.collider)
            {
                finger.proximal.collider = new GameObject(name + "Proximal").AddComponent<CapsuleCollider>();
                finger.proximal.collider.radius = 0.01f;
                finger.proximal.collider.height = 0.05f;
                finger.proximal.collider.direction = 0;
                finger.proximal.collider.transform.SetParent(palmCollider.transform);
                finger.proximal.collider.transform.localPosition = finger.proximal.mesh.localPosition;
                finger.proximal.collider.transform.localRotation = finger.proximal.mesh.localRotation;
            }
            if (!finger.intermediate.collider) finger.intermediate.collider = finger.proximal.collider.transform.Find(name + "Intermediate")?.GetComponent<CapsuleCollider>();
            // Ragdoll intermediate
            if (!finger.intermediate.collider)
            {
                finger.intermediate.collider = new GameObject(name + "Intermediate").AddComponent<CapsuleCollider>();
                finger.intermediate.collider.radius = 0.01f;
                finger.intermediate.collider.height = 0.05f;
                finger.intermediate.collider.direction = 0;
                finger.intermediate.collider.transform.SetParent(finger.proximal.collider.transform);
                finger.intermediate.collider.transform.localPosition = finger.intermediate.mesh.localPosition;
                finger.intermediate.collider.transform.localRotation = finger.intermediate.mesh.localRotation;
            }
            // Ragdoll distal
            if (!finger.distal.collider) finger.distal.collider = finger.intermediate.collider.transform.Find(name + "Distal")?.GetComponent<CapsuleCollider>();
            if (!finger.distal.collider)
            {
                finger.distal.collider = new GameObject(name + "Distal").AddComponent<CapsuleCollider>();
                finger.distal.collider.radius = 0.01f;
                finger.distal.collider.height = 0.05f;
                finger.distal.collider.direction = 0;
                finger.distal.collider.transform.SetParent(finger.intermediate.collider.transform);
                finger.distal.collider.transform.localPosition = finger.distal.mesh.localPosition;
                finger.distal.collider.transform.localRotation = finger.distal.mesh.localRotation;
            }
            // Tip
            string tipName = name + "Tip";
            finger.tip = finger.distal.collider.transform.Find(tipName);
            if (!finger.tip)
            {
                finger.tip = new GameObject(tipName).transform;
                finger.tip.SetParent(finger.distal.collider.transform);
                finger.tip.localRotation = Quaternion.identity;
                finger.tip.localPosition = Vector3.zero;
            }
        }

        /////////////////////////////////////////// POSES ///////////////////////////////////////////

        [Header("Poses")]
        public HandPose openPose;
        [NonSerialized]
        public HandPose.Fingers openPoseFingers;

        public HandPose closePose;
        [NonSerialized]
        public HandPose.Fingers closePoseFingers;

        public bool globalRatio;
        [Range(0f, 1f)]
        public float closeWeight;
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


        private void Update()
        {
#if UNITY_EDITOR
            if (editPose.alignLock)
            {
                editPose.AlignTestItem();
            }
#endif
        }


        public EditPose editPose;

        [Serializable]
        public class EditPose
        {
            public Item grabItem;
            public Handle grabHandle;
            public int handleOrientationIndex;
            public Grip useGripFrom = Grip.Current;
#if UNITY_EDITOR
            public bool alignLock;
#endif
            public enum Grip
            {
                Current,
                OpenPose,
                ClosePose,
            }
            [NonSerialized]
            public RagdollHand ragdollHand;
            protected Handle previousHandle;

            public EditPose(RagdollHand ragdollHand)
            {
                this.ragdollHand = ragdollHand;
            }

            [Button]
            public void AlignTestItem()
            {
                if (grabItem) grabHandle = ragdollHand.side == Side.Right ? grabItem.mainHandleRight : grabItem.mainHandleLeft;
                if (!grabHandle) return;

                if (previousHandle && previousHandle != grabHandle)
                {
                    Item previousItem = previousHandle.GetComponentInParent<Item>();
                    if (previousItem)
                    {
                        previousItem.transform.localPosition = new Vector3(0, 100, 0);
                        previousItem.transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        previousHandle.transform.root.localPosition = new Vector3(0, 100, 0);
                        previousHandle.transform.root.localRotation = Quaternion.identity;
                    }
                }

                if (useGripFrom == Grip.OpenPose)
                {
                    ragdollHand.SetGripFromOpenPose();
                }
                else if (useGripFrom == Grip.ClosePose)
                {
                    ragdollHand.SetGripFromClosePose();
                }

                grabHandle.CheckOrientations();
                HandleOrientation handleOrientation = grabHandle.orientationDefaultRight;
                if (ragdollHand.side == Side.Left) handleOrientation = grabHandle.orientationDefaultLeft;

                if (handleOrientationIndex > 0)
                {
                    int i = 0;
                    foreach (HandleOrientation ho in grabHandle.orientations)
                    {
                        if (grabHandle.orientationDefaultLeft == ho) continue;
                        if (grabHandle.orientationDefaultRight == ho) continue;
                        if (ho.side == ragdollHand.side)
                        {
                            if ((handleOrientationIndex - 1) == i) handleOrientation = ho;
                            i++;
                        }
                    }
                }

                Item item = grabHandle.GetComponentInParent<Item>();
                Transform alignObject = grabHandle.transform.root;
                if (item != null) alignObject = item.transform;

                Transform objectGrip = new GameObject("ObjectGrip").transform;
                objectGrip.SetParent(item ? item.transform : grabHandle.transform.root);
                objectGrip.position = handleOrientation.transform.position + (handleOrientation.handle.transform.up * handleOrientation.handle.GetDefaultAxisLocalPosition());
                objectGrip.rotation = handleOrientation.transform.rotation;

                alignObject.MoveAlign(objectGrip, ragdollHand.grip.position, ragdollHand.grip.rotation);

                DestroyImmediate(objectGrip.gameObject);
                previousHandle = grabHandle;
            }

            [Button]
            public virtual void SaveCurrentToOpenPose()
            {
                if (!ragdollHand.openPose) return;
                ragdollHand.SaveCurrentToPose(ragdollHand.openPose);
            }

            [Button]
            public virtual void SaveCurrentToClosePose()
            {
                if (!ragdollHand.closePose) return;
                ragdollHand.SaveCurrentToPose(ragdollHand.closePose);
            }
        }

        public void SetGripFromOpenPose()
        {
            grip.localPosition = openPose.GetFingers(side).gripLocalPosition;
            grip.localRotation = openPose.GetFingers(side).gripLocalRotation;
            grip.localScale = Vector3.one;
        }

        public void SetGripFromClosePose()
        {
            grip.localPosition = closePose.GetFingers(side).gripLocalPosition;
            grip.localRotation = closePose.GetFingers(side).gripLocalRotation;
            grip.localScale = Vector3.one;
        }

        public void SetCloseWeight(float weight)
        {
            closeWeight = weight;
            UpdatePoseThumb(closeWeight);
            UpdatePoseIndex(closeWeight);
            UpdatePoseMiddle(closeWeight);
            UpdatePoseRing(closeWeight);
            UpdatePoseLittle(closeWeight);
        }


        public void UpdatePoseThumb(float weight)
        {
            UpdateFinger(fingerThumb, openPoseFingers.thumb, closePoseFingers.thumb, weight);
        }

        public void UpdatePoseIndex(float weight)
        {
            UpdateFinger(fingerIndex, openPoseFingers.index, closePoseFingers.index, weight);
        }

        public void UpdatePoseMiddle(float weight)
        {
            UpdateFinger(fingerMiddle, openPoseFingers.middle, closePoseFingers.middle, weight);
        }

        public void UpdatePoseRing(float weight)
        {
            UpdateFinger(fingerRing, openPoseFingers.ring, closePoseFingers.ring, weight);
        }

        public void UpdatePoseLittle(float weight)
        {
            UpdateFinger(fingerLittle, openPoseFingers.little, closePoseFingers.little, weight);
        }

        public virtual void UpdateFinger(Finger finger, HandPose.Finger openPoseFinger, HandPose.Finger closePoseFinger, float ratio)
        {
            finger.proximal.collider.transform.localPosition = Vector3.Lerp(openPoseFinger.proximal.localPosition, closePoseFinger.proximal.localPosition, ratio);
            finger.proximal.collider.transform.localRotation = Quaternion.Lerp(openPoseFinger.proximal.localRotation, closePoseFinger.proximal.localRotation, ratio);
            finger.intermediate.collider.transform.localPosition = Vector3.Lerp(openPoseFinger.intermediate.localPosition, closePoseFinger.intermediate.localPosition, ratio);
            finger.intermediate.collider.transform.localRotation = Quaternion.Lerp(openPoseFinger.intermediate.localRotation, closePoseFinger.intermediate.localRotation, ratio);
            finger.distal.collider.transform.localPosition = Vector3.Lerp(openPoseFinger.distal.localPosition, closePoseFinger.distal.localPosition, ratio);
            finger.distal.collider.transform.localRotation = Quaternion.Lerp(openPoseFinger.distal.localRotation, closePoseFinger.distal.localRotation, ratio);
            if (!Application.isPlaying)
            {
                finger.proximal.mesh.transform.localPosition = Vector3.Lerp(openPoseFinger.proximal.localPosition, closePoseFinger.proximal.localPosition, ratio);
                finger.proximal.mesh.transform.localRotation = Quaternion.Lerp(openPoseFinger.proximal.localRotation, closePoseFinger.proximal.localRotation, ratio);
                finger.intermediate.mesh.transform.localPosition = Vector3.Lerp(openPoseFinger.intermediate.localPosition, closePoseFinger.intermediate.localPosition, ratio);
                finger.intermediate.mesh.transform.localRotation = Quaternion.Lerp(openPoseFinger.intermediate.localRotation, closePoseFinger.intermediate.localRotation, ratio);
                finger.distal.mesh.transform.localPosition = Vector3.Lerp(openPoseFinger.distal.localPosition, closePoseFinger.distal.localPosition, ratio);
                finger.distal.mesh.transform.localRotation = Quaternion.Lerp(openPoseFinger.distal.localRotation, closePoseFinger.distal.localRotation, ratio);
            }
        }

        public virtual void SaveCurrentToPose(HandPose handPose)
        {
            HandPose.Fingers fingers = handPose.GetFingers(side);
            SaveFinger(fingers.thumb, fingerThumb);
            SaveFinger(fingers.index, fingerIndex);
            SaveFinger(fingers.middle, fingerMiddle);
            SaveFinger(fingers.ring, fingerRing);
            SaveFinger(fingers.little, fingerLittle);
            fingers.gripLocalPosition = grip.localPosition;
            fingers.gripLocalRotation = grip.localRotation;
            handPose.Save(side);
        }

        protected virtual void SaveFinger(HandPose.Finger poseFinger, Finger finger)
        {
            poseFinger.proximal.localPosition = finger.proximal.mesh.transform.localPosition;
            poseFinger.proximal.localRotation = finger.proximal.mesh.transform.localRotation;
            poseFinger.intermediate.localPosition = finger.intermediate.mesh.transform.localPosition;
            poseFinger.intermediate.localRotation = finger.intermediate.mesh.transform.localRotation;
            poseFinger.distal.localPosition = finger.distal.mesh.transform.localPosition;
            poseFinger.distal.localRotation = finger.distal.mesh.transform.localRotation;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            foreach (Finger finger in fingers)
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
            if (grip)
            {
                Gizmos.matrix = grip.localToWorldMatrix;
                Gizmos.color = Common.HueColourValue(HueColorName.Purple);
                Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(0.01f, 0.05f, 0.01f));
                Gizmos.DrawWireCube(new Vector3(0f, 0.03f, 0.01f), new Vector3(0.01f, 0.01f, 0.03f));
            }
        }

    }
}