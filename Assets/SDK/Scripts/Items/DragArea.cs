using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class DragArea : ThunderBehaviour
    {
        /// <summary>
        /// Struct used to define fluids (air, water, etc.).
        /// The higher the density, the higher the drag.
        /// Flow is made to simulate world space "current".
        /// </summary>
        [Serializable]
        public struct Fluid
        {
            /// <summary>
            /// Over simplification of the real world mass densities.
            /// Simplified to return the density only.
            /// </summary>
            public float MassDensity => density;

            public float density;
            public Vector3 flow;

            public Fluid(float density, Vector3 flow)
            {
                this.density = density;
                this.flow = flow;
            }

            public Vector3 FlowVelocity(Vector3 velocity)
            {
                return velocity + flow;
            }
        }

        public enum VelocityType
        {
            Estimated,
            FromRigidbody
        }

        [Header("Velocity")] [Tooltip("Type of velocity to use. Estimated mode does not need any rigidbody")]
        public VelocityType velocityType = VelocityType.Estimated;

        [Tooltip("Rigidbody to get the velocity from. Only used when velocityType = 'FromRigidbody'.")]
        public Rigidbody rbToGetVelocityFrom;

        [Tooltip("Number of frames used to estimate the velocity. Only used when velocityType = 'Estimated'.")]
        public int velocityAverageFrames = 5;


        [Header("Coefficients")] [Tooltip("Multiplies the drag by this value")]
        public float dragCoef = 1f;

        [Tooltip("Percentage of drag to convert into lift")]
        public float dragToLiftRatio = .5f;

        [Tooltip(
            "Eases the drag across the normal. When drag is in the same direction than the normal, we sample at 1. when it's perpendicular, we sample at 0.")]
        public AnimationCurve dragAngleEasing = AnimationCurve.Linear(0, 1f, 1f, 1f);

        [Header("Bodies")] [Tooltip("Rigidbodies that will be dragged by the area.")]
        public List<Rigidbody> bodiesToDrag;

        [Tooltip("Rigidbodies that will be lifted by the area.")]
        public List<Rigidbody> bodiesToLift;

        [Tooltip("Will drag the creature grabbing this item (locomotion)")]
        public bool dragGrabbingCreatureBody;

        [Tooltip("Will lift the creature grabbing this item (locomotion)")]
        public bool liftGrabbingCreatureBody;

        [Tooltip("Will use the creature locomotion position as the velocity origin")]
        public bool useCreatureLocomotionAsVelocityOrigin;

        [Header("Surface")] [Tooltip("Area surface as a plane")]
        public Vector2 surfaceDimension;

        [Tooltip("Point to add the drag forces at")]
        public Transform center;

        [Tooltip("Origin used to estimate the velocity. Only used when velocityType = 'Estimated'")]
        public Transform velocityOrigin;

        [Tooltip("Is the area two sided? If not, drag will only apply in one way")]
        public bool twoSided = true;

        [Header("Item")]
        [Tooltip("Is the area on an Item? If yes, then item callbacks will be used (grab, un-grab, etc.)")]
        public bool listenToItemCallbacks = true;

        [Header("Misc")]
        [Tooltip(
            "Prevent drag & lift to be computed when the creature turns (causing fast and abrupt changes of position)")]
        public bool preventUpdatesWhenCreatureTurns;

        [Tooltip("Check if the area enters and exit from the water")]
        public bool checkForWater = true;

        [NonSerialized] public Fluid currentFluid;

        public Vector3 Normal => transform.forward;
        public float Area => surfaceDimension.x * surfaceDimension.y;

        public UnityEvent onStart;
        public UnityEvent onStop;

        [Header("This event is fired (every physic frames) when the area moves in any direction")]
        public UnityEvent<float> onAreaMoves;

        [Header("This event is fired (only once) when we first detect a motion that causes drag")]
        public UnityEvent<float> onAreaStartDragging;

        [Header("This event is fired (every physic frames) when we detect a motion that causes drag")]
        public UnityEvent<float> onAreaDrags;

        [Header("This event is fired (every physic frames) when we detect a motion that is not causing drag")]
        public UnityEvent<float> onAreaPulls;

        [Header("This event is fired (only once) when we stop detecting a motion that causes drag")]
        public UnityEvent<float> onAreaStopDragging;

        [Header("Item related events")] public UnityEvent onItemSnaps;
        public UnityEvent<Handle, RagdollHand> onItemIsGrabbed;
        public UnityEvent<Handle, RagdollHand> onItemIsUnGrabbed;


        private void OnDrawGizmosSelected()
        {
            var matrix = Gizmos.matrix;
            var t = transform;

            Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation,
                new Vector3(surfaceDimension.x, surfaceDimension.y, 0));
            Gizmos.color = new Color(1f, .3f, .1f, .1f);
            Gizmos.DrawCube(Vector3.zero + Vector3.forward / 100f, Vector3.one);
            Gizmos.color = new Color(1f, .3f, .1f);
            Gizmos.DrawWireCube(Vector3.zero + Vector3.forward / 100f, Vector3.one);

            Gizmos.matrix = matrix;

            Gizmos.color = new Color(1f, .3f, .1f);
            ArrowGizmo(center.position, Normal, .075f, .05f);
            Gizmos.color = new Color(.1f, .3f, 1f);
            if (twoSided) ArrowGizmo(center.position, -Normal, .075f, .05f);


            Gizmos.color = new Color(.35f, .5f, 1f);
            var position = center.position;
            Gizmos.DrawLine(position + center.up * surfaceDimension.y / 2f,
                position - center.up * surfaceDimension.y / 2f);

            float n = 10f;
            for (int i = 0; i < n; i++)
            {
                var pos = t.position - transform.right * surfaceDimension.x / 2f;
                
                var easing = dragAngleEasing.Evaluate(i / n);
                var invertedEasing = dragAngleEasing.Evaluate(1f - i / n);

                Gizmos.matrix = Matrix4x4.TRS( pos + transform.right * (i * (surfaceDimension.x / 2f / n)), t.rotation,
                    new Vector3(surfaceDimension.x / n, surfaceDimension.y, 0));
                
                Gizmos.color = new Color(1f, .3f, .1f, easing);
                Gizmos.DrawCube(Vector3.zero + Vector3.forward / 100f, Vector3.one);
                
                Gizmos.matrix = Matrix4x4.TRS( pos + transform.right * (surfaceDimension.x / 2f + i * (surfaceDimension.x / 2f / n)), t.rotation,
                    new Vector3(surfaceDimension.x / n, surfaceDimension.y, 0));
                
                Gizmos.color = new Color(1f, .3f, .1f, invertedEasing);
                Gizmos.DrawCube(Vector3.zero + Vector3.forward / 100f, Vector3.one);
            } 
            Gizmos.matrix = matrix;

        }

        private static void ArrowGizmo(Vector3 pos, Vector3 normal, float magnitude, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (magnitude <= 0.0001f) return;
            if (normal.sqrMagnitude <= 0.0001f) return;

            var direction = normal * magnitude;

            Gizmos.DrawRay(pos, direction);

            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                        new Vector3(0, 0, 1);

            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                       new Vector3(0, 0, 1);

            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}