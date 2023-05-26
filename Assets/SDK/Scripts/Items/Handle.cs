using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Handle")]
    [AddComponentMenu("ThunderRoad/Handle")]
    public class Handle : Interactable
    {
        [Range(-1, 1)]
        [Tooltip("Define where the handle is automatically grabbed along the axis length")]
        public float defaultGrabAxisRatio;
        public Vector3 ikAnchorOffset;

        [NonSerialized]
        public List<HandlePose> orientations = new List<HandlePose>();
        [Tooltip("The default handpose to be grabbed (Right Hand)")]
        public HandlePose orientationDefaultRight;
        [Tooltip("The default handpose to be grabbed (Left Hand)")]
        public HandlePose orientationDefaultLeft;
		[Tooltip("When linked handle is grabbed, ungrip this handle.")]
        public Handle releaseHandle;

        [Tooltip("When ticked, no sound will play when the handle is grabbed")]
        public bool silentGrab = false;
        [Tooltip("When ticked, the player will ungrab the handle when grounded (touching the ground)")]
        public bool forceAutoDropWhenGrounded;
        [Tooltip("The default handpose to be grabbed (Right Hand)")]
        public bool ignoreClimbingForceOverride;

		[Tooltip("Lets AI know how far the item is away from the player. You can use the button to calculate this automatically, so long as a ColliderGroup is set up with sufficient colliders.")]
        public float reach = 0.5f;
		[Tooltip("(Optional)Disables listed colliders once the handle is grabbed.")]
        public List<Collider> handOverlapColliders;
		[Tooltip("(Optional) Allows you to add a custom rigidbody to the handle. (Do not reference item!)")]
        public Rigidbody customRigidBody;
		[Tooltip("(Optional) When player hand reaches the top of the handle via slide, it will switch to listed handle once the top is reached.")]
        public Handle slideToUpHandle;
        [Tooltip("(Optional) When player hand reaches the bottom of the handle via slide, it will switch to listed handle once the bottom is reached.")]
        public Handle slideToBottomHandle;
        [Tooltip("(Optional) Offset of the bottom and top slide up handles. Can switch handle when reaching 0.2 meters away from the top/bottom, for example.")]
        public float slideToHandleOffset = 0.01f;
        [Tooltip("Allows you to enable/disable handle sliding (Only works on handles with a length greater than 0!)")]
        public SlideBehavior slideBehavior = SlideBehavior.CanSlide;

        [Tooltip("(Optional) When you slide up the handle, and the axis length is 0, sliding will instead snap to the referenced handle.")]
        public Handle moveToHandle;
        [Tooltip("Axis Position for the \"Move To Handle\" handle")]
        public float moveToHandleAxisPos = 0;

        [NonSerialized]
        public bool updatePosesWhenWeightChanges = false;

        [Obsolete, Header("This is Obsolete. Please use Handle Pose button instead.")]
        public List<Orientation> allowedOrientations = new List<Orientation>();


        [Header("Controller axis override")]
        public bool redirectControllerAxis;
        public UnityEvent<float> controllerAxisOutputX = new UnityEvent<float>();
        public UnityEvent<float> controllerAxisOutputY = new UnityEvent<float>();

        public new enum HandSide
        {
            None,
            Right,
            Left,
            Both,
        }

        public enum SlideBehavior
        {
            CanSlide,
            KeepSlide,
            DisallowSlide,
        }

        [Serializable]
        public class Orientation
        {
            public Vector3 rotation;
            public Vector3 positionOffset;
            public HandSide allowedHand = HandSide.Both;
            public HandSide isDefault = HandSide.None;
            public Orientation(Vector3 position, Vector3 rotation, HandSide allowedHand, HandSide isDefault)
            {
                this.rotation = rotation;
                this.positionOffset = position;
                this.allowedHand = allowedHand;
                this.isDefault = isDefault;
            }
        }
        public enum TwoHandedMode
        {
            Position,
            Dominant,
            AutoFront,
            AutoRear,
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif
            if (this.InPrefabScene()) return;
            if (!gameObject.activeInHierarchy) return;
            SetupDefaultComponents();
        }

        private void SetupDefaultComponents()
        {
            if (orientations.Count == 0) CheckOrientations();
        }

        [Button("Update Orientations")]
        public virtual void CheckOrientations()
        {
            orientations = new List<HandlePose>(this.GetComponentsInChildren<HandlePose>());
            if (orientations.Count == 0)
            {
                if (allowedOrientations.Count > 0)
                {
                    foreach (Orientation allowedOrientation in allowedOrientations)
                    {
                        if (allowedOrientation.allowedHand == HandSide.Both || allowedOrientation.allowedHand == HandSide.Right)
                        {
                            HandlePose handleOrientation = AddOrientation(Side.Right, allowedOrientation.positionOffset, Quaternion.Euler(allowedOrientation.rotation));
                            if (orientationDefaultRight == null && (allowedOrientation.isDefault == HandSide.Both || allowedOrientation.isDefault == HandSide.Right))
                            {
                                orientationDefaultRight = handleOrientation;
                            }
                        }
                        if (allowedOrientation.allowedHand == HandSide.Both || allowedOrientation.allowedHand == HandSide.Left)
                        {
                            HandlePose handleOrientation = AddOrientation(Side.Left, allowedOrientation.positionOffset, Quaternion.Euler(allowedOrientation.rotation));
                            if (orientationDefaultLeft == null && (allowedOrientation.isDefault == HandSide.Both || allowedOrientation.isDefault == HandSide.Left))
                            {
                                orientationDefaultLeft = handleOrientation;
                            }
                        }
                    }
                }
                else
                {
                    orientationDefaultRight = AddOrientation(Side.Right, Vector3.zero, Quaternion.identity);
                    orientationDefaultLeft = AddOrientation(Side.Left, Vector3.zero, Quaternion.identity);
                }
            }
        }

        public virtual HandlePose AddOrientation(Side side, Vector3 position, Quaternion rotation)
        {
            GameObject orient = new GameObject("Orient");
            orient.transform.SetParent(this.transform);
            orient.transform.localPosition = position;
            orient.transform.localRotation = rotation;
            orient.transform.localScale = Vector3.one;
            HandlePose handleOrientation = orient.AddComponent<HandlePose>();
            handleOrientation.side = side;
#if UNITY_EDITOR
            handleOrientation.UpdateName();
#endif
            orientations.Add(handleOrientation);
            return handleOrientation;
        }

        public virtual float GetDefaultAxisLocalPosition()
        {
            if (axisLength == 0) return 0;
            else return defaultGrabAxisRatio * (axisLength / 2);
        }

        public virtual Vector3 GetDefaultAxisPosition(Side side)
        {
            return this.transform.TransformPoint(0, GetDefaultAxisLocalPosition(), 0);
        }

        public virtual HandlePose GetDefaultOrientation(Side side)
        {
            if (side == Side.Right && orientationDefaultRight)
            {
                return orientationDefaultRight;
            }
            if (side == Side.Left && orientationDefaultLeft)
            {
                return orientationDefaultLeft;
            }
            Debug.LogError("No default orientation found! Please check the prefab " + this.transform.parent.name + "/" + this.name);
            return null;
        }

        public virtual HandlePose GetNearestOrientation(Transform grip, Side side)
        {
            float higherDot = -Mathf.Infinity;
            HandlePose orientationResult = null;
            foreach (HandlePose orientation in orientations)
            {
                if (orientation.side == side)
                {
                    float dot = Vector3.Dot(grip.forward, orientation.transform.rotation * Vector3.forward) + Vector3.Dot(grip.up, orientation.transform.rotation * Vector3.up);
                    if (dot > higherDot)
                    {
                        higherDot = dot;
                        orientationResult = orientation;
                    }
                }
            }
            return orientationResult;
        }

        public virtual float GetNearestAxisPosition(Vector3 position)
        {
            Vector3 referenceLocalPos = this.transform.InverseTransformPoint(position);
            return Mathf.Clamp(referenceLocalPos.y, -(axisLength / 2), (axisLength / 2));
        }

        public virtual Vector3 GetNearestPositionAlongAxis(Vector3 position)
        {
            return this.transform.TransformPoint(new Vector3(0, GetNearestAxisPosition(position), 0));
        }

        public virtual bool IsAllowed(Side side)
        {
            foreach (HandlePose orientation in orientations)
            {
                if (side == orientation.side)
                {
                    return true;
                }
            }
            return false;
        }

        [Button("Calculate Reach")]
        public virtual void CalculateReach()
        {
            float farthestDamagerDist = 0;
            foreach (ColliderGroup colliderGroup in this.GetComponentInParent<Item>().GetComponentsInChildren<ColliderGroup>())
            {
                foreach (Collider collider in colliderGroup.GetComponentsInChildren<Collider>())
                {
                    Vector3 farthestPoint = collider.ClosestPointOnBounds(this.transform.position + (this.transform.up.normalized * 10));
                    float dist = this.transform.InverseTransformPoint(farthestPoint).y;
                    if (dist > farthestDamagerDist)
                    {
                        farthestDamagerDist = dist;
                    }
                }
            }
            reach = farthestDamagerDist - GetDefaultAxisLocalPosition();
        }

        public virtual void SetUpdatePoses(bool active)
        {
            updatePosesWhenWeightChanges = active;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(Vector3.zero, reach);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(0, GetDefaultAxisLocalPosition(), 0), 0.03f);
        }
    }
}
