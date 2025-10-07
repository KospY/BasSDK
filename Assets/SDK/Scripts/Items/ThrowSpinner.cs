// Copyright (c) WarpFrog. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using UnityEngine.Animations;

namespace ThunderRoad
{

    public class ThrowSpinner : ThunderBehaviour
    {
        public const int circleSegments = 120;
        public const float gizmoSize = 0.15f;


        public float rotationSpeed = 5f;
        public Vector2 allowances = new Vector2(180f, 15f);

        [LabelText("Rotate FlyRef On Grab (Hover for more info)")]
        [Tooltip("Rotating the FlyRef on grab requires the rotation of all handles on the item to match the default rotation of the FlyRef." +
            "\nWhen grabbed, the FlyRef automatically rotates to match the rotation of the handle relative to the default hand orientations on that handle.")]
        public bool rotateFlyRefOnGrab = false;
#if UNITY_EDITOR
        private bool handlesMisaligned
        {
            get
            {
                if (!rotateFlyRefOnGrab) return false;
                if (currentAligned != null) return false;
                foreach (Handle handle in GetComponentsInChildren<Handle>())
                {
                    if (!Quaternion.Angle(handle.transform.rotation, flyRef.transform.rotation).IsApproximately(0f))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
#endif
        [ReadOnly]
        public Transform rotationCenter;
        private bool rotationCenterMissing => meshRotationReference == null;
        [ReadOnly]
        public Transform meshRotationReference;
        private bool meshRotationRefMissing => meshRotationReference == null;
        [ReadOnly]
        public Transform physicsRotationReference;
        private bool physicRotationRefMissing => physicsRotationReference == null;
        public List<GameObject> meshObjects;
        private bool meshError => meshObjects.IsNullOrEmpty();
        public List<SpinnerDamager> targetDamagers;

#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private List<Component> addedComponents;
        public bool debugRotation;
        [Range(-1f, 2f)]
        [HideLabel]
        [EnableIf(nameof(debugRotation))]
        public float debugRotationNormalized = 0f;
        private bool waitValidate;
        private HandlePose currentAligned;
#endif

        [HideInInspector]
        public Item item;
        public SpinnerDamager forwardSpinnerDamager { get; protected set; }
        public bool isSpinning { get; protected set; } = false;
        [NonSerialized]
        [ShowInInspector]

        [ReadOnly]
        public float currentRotation = 0f;
        [NonSerialized]
        [ShowInInspector]

        [LabelText("Mesh")]
        
        [ReadOnly]
        public float currentMeshRotation = 0f;
        [NonSerialized]
        [ShowInInspector]

        [LabelText("Physics")]
        
        [ReadOnly]
        public float currentPhysicRotation = 0f;
        private bool missingItem => item == null;
        private bool missingFlyRef => !missingItem && item?.flyDirRef == null;

        protected Transform flyRef => item?.flyDirRef != null ? item.flyDirRef : transform;

        [Serializable]
        public class SpinnerDamager
        {
            
            public Damager damager;
            private bool notForward => damager?.direction != Damager.Direction.Forward;
            
            [ReadOnly]
            public Vector2 angleRange;
            [HideInInspector]
            public ThrowSpinner spinner;

            public Transform transform => damager.transform;

            public SpinnerDamager(Damager damager)
            {
                this.damager = damager;
            }

            public static implicit operator SpinnerDamager(Damager damager) => new SpinnerDamager(damager);
        }

        private void OnValidate()
        {
            item = GetComponent<Item>();
#if UNITY_EDITOR
            if (Application.isPlaying) return;
            if (debugRotation)
            {
                SetRotation(debugRotationNormalized * 360f);
                waitValidate = true;
            }
            else if (!debugRotationNormalized.IsApproximately(0f))
            {
                SetRotation(0f);
                debugRotationNormalized = 0f;
                if (waitValidate)
                {
                    waitValidate = false;
                    return;
                }
                SetTargetDamagers();
            }
#endif
        }

        private void OnDrawGizmosSelected()
        {
            Color orgColor = Gizmos.color;
            Vector3 center = flyRef.position;
            Gizmos.color = Color.red;
            foreach (SpinnerDamager damager in targetDamagers)
            {
                Gizmos.DrawLine(center, damager.transform.position);
            }
            Common.DrawGizmosCircle(center, flyRef.right, flyRef.forward, gizmoSize, circleSegments, angle =>
            {
                Gizmos.color = Color.black;
                if (GetDamagerForAngle(angle) is SpinnerDamager damager)
                {
                    float damagerAngle = angle - damager.angleRange.x;
                    if (damagerAngle < 0) damagerAngle += 360f;
                    float totalRange = damager.angleRange.y - damager.angleRange.x;
                    Gizmos.color = Color.Lerp(Color.red, Color.green, damagerAngle / totalRange);
                }
            });
            Common.DrawGizmoArrow(center + (flyRef.up * gizmoSize), flyRef.forward * 0.05f, Color.yellow, 0.05f);
            Common.DrawGizmoArrow(center + (-flyRef.up * gizmoSize), -flyRef.forward * 0.05f, Color.yellow, 0.05f);
            Common.DrawGizmoArrow(center + (flyRef.forward * gizmoSize), -flyRef.up * 0.05f, Color.yellow, 0.05f);
            Common.DrawGizmoArrow(center + (-flyRef.forward * gizmoSize), flyRef.up * 0.05f, Color.yellow, 0.05f);
            Gizmos.color = orgColor;
        }

 //ProjectCore    
        private void SetTargetDamagers()
        {
            if (targetDamagers != null)
            {
                for (int i = targetDamagers.Count - 1; i >= 0; i--)
                {
                    if (targetDamagers[i].damager == null) targetDamagers.RemoveAt(i);
                }
            }
            if (targetDamagers.IsNullOrEmpty())
            {
                targetDamagers = new List<SpinnerDamager>();
                foreach (Damager damager in GetComponentsInChildren<Damager>(false))
                {
                    if (damager.direction != Damager.Direction.Forward) continue;
                    targetDamagers.Add(damager);
                }
            }
            Vector3 center = flyRef.position;
            foreach (SpinnerDamager damager in targetDamagers)
            {
                damager.spinner = this;
                damager.angleRange.y = Vector3.SignedAngle(flyRef.forward, damager.transform.forward, flyRef.right);
                if (damager.angleRange.y <= 0) damager.angleRange.y += 360f;
                damager.angleRange.y += allowances.y;
            }
            targetDamagers = targetDamagers.OrderByDescending(spinner => spinner.angleRange.y).ToList();
            for (int i = 0; i < targetDamagers.Count; i++)
            {
                SpinnerDamager damager = targetDamagers[i];
                damager.angleRange.x = i + 1 < targetDamagers.Count ? targetDamagers[i + 1].angleRange.y : MakeNormalizedAngle(targetDamagers[0].angleRange.y);
                damager.angleRange.x = Mathf.Round(Mathf.MoveTowards(damager.angleRange.y, damager.angleRange.x, allowances.x) * 100f) / 100f;
            }
        }

        private float MakeNormalizedAngle(float angle)
        {
            angle = (angle + 0.0001f) % 360f;
            if (angle < 0f) angle += 360f;
            angle -= 0.0001f;
            return angle;
        }

        public SpinnerDamager GetDamagerForAngle(float angle)
        {
            angle = MakeNormalizedAngle(angle);
            foreach (SpinnerDamager damager in targetDamagers)
            {
                if (damager.angleRange.ValueBetween(angle) || damager.angleRange.ValueBetween(angle + 360f))
                {
                    return damager;
                }
            }
            return null;
        }

        public void MoveTowardsRotation(float target, float speed)
        {
            target = MakeNormalizedAngle(target);
            float angle = Mathf.MoveTowards(currentRotation, target, speed);
            SetRotation(angle);
        }

        public void ChangeRotation(float angle)
        {
            SetRotation(currentRotation + angle);
        }

        public void SetRotation(float angle)
        {
            angle = MakeNormalizedAngle(angle);
            SetMeshRotation(angle);
            SetPhysicsRotation(angle);
            currentRotation = angle;
        }

        private void SetMeshRotation(float angle)
        {
            float diff = angle - currentMeshRotation;
            currentMeshRotation = angle;
            meshRotationReference.localEulerAngles = new Vector3(angle, 0f, 0f);
        }

        private void SetPhysicsRotation(float angle)
        {
            Vector3 center = flyRef.position;
            SpinnerDamager newDamager = GetDamagerForAngle(angle);
            float finalAngle = MakeNormalizedAngle(newDamager != null ? (newDamager.angleRange.y - allowances.y) : angle);
            if (newDamager != null)
            {
                if (newDamager == forwardSpinnerDamager) return;
                forwardSpinnerDamager = newDamager;
            }
            else
            {
                forwardSpinnerDamager = null;
            }
            physicsRotationReference.localEulerAngles = new Vector3(finalAngle, 0f, 0f);
            currentPhysicRotation = finalAngle;
        }

#if UNITY_EDITOR
        [Button]
        public void UpdateTargetDamagers()
        {
            targetDamagers = null;
            SetTargetDamagers();
        }

        [Button]
        public void MoveFlyRefToCenterOfMass()
        {
            Transform flyRef = item.flyDirRef;
            flyRef.position = item.physicBody.transform.TransformPoint(item.physicBody.centerOfMass);
        }

        [Button]
        public void SetConstraints()
        {
            if (!addedComponents.IsNullOrEmpty())
            {
                foreach (var component in addedComponents)
                {
                    DestroyImmediate(component);
                }
            }
            if (rotationCenter != null && rotationCenter.gameObject is GameObject rotationCenterObject) DestroyImmediate(rotationCenterObject);
            if (meshRotationReference != null && meshRotationReference.gameObject is GameObject meshRotateObject) DestroyImmediate(meshRotateObject);
            if (physicsRotationReference != null && physicsRotationReference.gameObject is GameObject physicRotateObject) DestroyImmediate(physicRotateObject);
            addedComponents = new List<Component>();
            rotationCenter = new GameObject("RotationCenter").transform;
            rotationCenter.parent = transform;
            rotationCenter.SetPositionAndRotation(flyRef.position, flyRef.rotation);
            meshRotationReference = new GameObject("MeshRotationReference").transform;
            meshRotationReference.SetParentOrigin(rotationCenter, Vector3.zero, Quaternion.identity);
            physicsRotationReference = new GameObject("PhysicRotationReference").transform;
            physicsRotationReference.SetParentOrigin(rotationCenter, Vector3.zero, Quaternion.identity);

            // set constraints on all the mesh objects
            ConstraintSource meshConstraintSource = new ConstraintSource() { sourceTransform = meshRotationReference, weight = 1f };
            foreach (GameObject meshObject in meshObjects)
            {
                addedComponents.Add(AddConstraintAndOffsets(meshObject, meshConstraintSource));
            }

            // set constraints on all the non-mesh objects
            ConstraintSource physicConstraintSource = new ConstraintSource() { sourceTransform = physicsRotationReference, weight = 1f };
            foreach (Transform transform in transform)
            {
                if (transform == flyRef) continue;
                if (transform == rotationCenter) continue;
                if (meshObjects.Contains(transform.gameObject)) continue;
                addedComponents.Add(AddConstraintAndOffsets(transform.gameObject, physicConstraintSource));
            }
        }

        private ParentConstraint AddConstraintAndOffsets(GameObject gameObject, ConstraintSource constraintSource)
        {
            ParentConstraint parentConstraint = gameObject.AddComponent<ParentConstraint>();
            parentConstraint.AddSource(constraintSource);
            parentConstraint.constraintActive = true;
            parentConstraint.SetTranslationOffset(0, constraintSource.sourceTransform.InverseTransformPoint(gameObject.transform.position));
            parentConstraint.SetRotationOffset(0, constraintSource.sourceTransform.InverseTransformRotation(gameObject.transform.rotation).eulerAngles);
            parentConstraint.locked = true;
            return parentConstraint;
        }
#endif
    }
}
