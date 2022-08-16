using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
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
        public float defaultGrabAxisRatio;
        public Vector3 ikAnchorOffset;

        [NonSerialized]
        public List<HandlePose> orientations = new List<HandlePose>();
        public HandlePose orientationDefaultRight;
        public HandlePose orientationDefaultLeft;

        public Handle releaseHandle;

        public bool silentGrab = false;
        public bool forceAutoDropWhenGrounded;

        public float reach = 0.5f;
        public List<Collider> handOverlapColliders;
        public Rigidbody customRigidBody;
        public Handle slideToUpHandle;
        public Handle slideToBottomHandle;
        public float slideToHandleOffset = 0.01f;
        [Tooltip("Only works on handles with a length greater than 0!")]
        public SlideBehavior slideBehavior = SlideBehavior.CanSlide;

        public Handle moveToHandle;
        public float moveToHandleAxisPos = 0;

        [NonSerialized]
        public bool updatePosesWhenWeightChanges = false;

        [Obsolete, Header("Obsolete! Use child HandleOrientation instead")]
        public List<Orientation> allowedOrientations = new List<Orientation>();

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

        public bool IsAllowed(Side side)
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
        public void CalculateReach()
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

        public void SetUpdatePoses(bool active)
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
