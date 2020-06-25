using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class HandleDefinition : InteractableDefinition
    {
        [Range(-1, 1)]
        public float defaultGrabAxisRatio;
        public Vector3 ikAnchorOffset;
        
        [NonSerialized]
        public List<HandleOrientation> orientations;
        public HandleOrientation orientationDefaultRight;
        public HandleOrientation orientationDefaultLeft;

        public HandleDefinition releaseHandle;

        public float reach = 0.5f;
        public Rigidbody customRigidBody;
        public HandleDefinition slideToUpHandle;
        public HandleDefinition slideToBottomHandle;
        public float slideToHandleOffset = 0.01f;

        [Obsolete, Header("Obsolete! Use child HandleOrientation instead")]
        public List<Orientation> allowedOrientations = new List<Orientation>();

        public enum HandSide
        {
            None,
            Right,
            Left,
            Both,
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

        protected override void Awake()
        {
            base.Awake();
            CheckOrientations();
        }

        [Button("Update Orientations")]
        public virtual void CheckOrientations()
        {
            orientations = new List<HandleOrientation>(this.GetComponentsInChildren<HandleOrientation>());
            if (orientations.Count == 0)
            {
                if (allowedOrientations.Count > 0)
                {
                    foreach (Orientation allowedOrientation in allowedOrientations)
                    {
                        if (allowedOrientation.allowedHand == HandSide.Both || allowedOrientation.allowedHand == HandSide.Right)
                        {
                            HandleOrientation handleOrientation = AddOrientation(Side.Right, allowedOrientation.positionOffset, Quaternion.Euler(allowedOrientation.rotation));
                            if (orientationDefaultRight == null && (allowedOrientation.isDefault == HandSide.Both || allowedOrientation.isDefault == HandSide.Right))
                            {
                                orientationDefaultRight = handleOrientation;
                            }
                        }
                        if (allowedOrientation.allowedHand == HandSide.Both || allowedOrientation.allowedHand == HandSide.Left)
                        {
                            HandleOrientation handleOrientation = AddOrientation(Side.Left, allowedOrientation.positionOffset, Quaternion.Euler(allowedOrientation.rotation));
                            if (orientationDefaultLeft == null && (allowedOrientation.isDefault == HandSide.Both || allowedOrientation.isDefault == HandSide.Left))
                            {
                                orientationDefaultLeft = handleOrientation;
                            }
                        }
                    }
                }
                else
                {
                    if (!Application.isPlaying)
                    {
                        orientationDefaultRight = AddOrientation(Side.Right, Vector3.zero, Quaternion.identity);
                        orientationDefaultLeft = AddOrientation(Side.Left, Vector3.zero, Quaternion.identity);
                    }
                }
            }
        }

        public virtual HandleOrientation AddOrientation(Side side, Vector3 position, Quaternion rotation)
        {
            GameObject orient = new GameObject("Orient");
            orient.transform.SetParent(this.transform);
            orient.transform.localPosition = position;
            orient.transform.localRotation = rotation;
            orient.transform.localScale = Vector3.one;
            HandleOrientation handleOrientation = orient.AddComponent<HandleOrientation>();
            handleOrientation.side = side;
            handleOrientation.UpdateName();
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

        public virtual HandleOrientation GetDefaultOrientation(Side side)
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

        public virtual HandleOrientation GetNearestOrientation(Transform grip, Side side)
        {
            float higherDot = -Mathf.Infinity;
            HandleOrientation orientationResult = null;
            foreach (HandleOrientation orientation in orientations)
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
            foreach (HandleOrientation orientation in orientations)
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
            foreach (ColliderGroup colliderGroup in this.GetComponentInParent<ItemDefinition>().GetComponentsInChildren<ColliderGroup>())
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

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
        }
    }
}
