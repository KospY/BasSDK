using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad
{
    public class GravityZone : Zone
    {
        public static int duplicateId = Animator.StringToHash("GravityZone");
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.FixedUpdate;

        [Header("Effect")]
        public string loopEffectId = "GravityElevator";

        protected EffectData loopEffectData;

        [Header("Set up")]
        public Vector3 liftRoot;
        public Vector3 liftTop;

        [Header("Enter")]
        public bool pushOnEnter;
        public float enterPushForce = 50f;

        [Header("Creature")]
        public bool destabilize = true;

        [Header("Swimming")]
        public bool allowSwimming;
        public float swimmingForce = 5;

        [Header("Locomotion")]
        public bool useGravityMultiplierForLocomotion;
        public float gravityMultiplierToApplyToLocomotion;

        [Header("HeadLocomotion")]
        public bool useHeadLocomotion;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("useHeadLocomotion")]
#endif //ODIN_INSPECTOR*
        public Vector3 headLocomotionLocalDirection;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("useHeadLocomotion")]
#endif //ODIN_INSPECTOR
        public AnimationCurve headLocomotionDotCurve;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("useHeadLocomotion")]
#endif //ODIN_INSPECTOR
        public float headLocomotionVelocity;

        [Header("Item")]
        public bool useGravityMultiplierForItem;
        public float gravityMultiplierToApplyToItem = 1;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            if (!mainCollider) mainCollider = GetComponent<Collider>();
        }
#endif


        #region Gizmos

        /// <summary>
        /// Checks if the given point is inside the given collider.
        /// </summary>
        /// <param name="collider">Collider to check for.</param>
        /// <param name="point">Point to check for.</param>
        /// <returns>True if the point is inside the given collider, false otherwise.</returns>
        public static bool IsPointWithinCollider(Collider collider, Vector3 point)
        {
            if (!collider) return false;
            return (collider.ClosestPoint(point) - point).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var root = transform.TransformPoint(liftRoot);
            var top = transform.TransformPoint(liftTop);
            Gizmos.DrawLine(root, top);

            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(root,
                Quaternion.identity,
                new Vector3(1, 0, 1));
            Gizmos.DrawWireSphere(Vector3.zero, mainCollider.bounds.extents.x);

            Gizmos.matrix = Matrix4x4.TRS(top,
                Quaternion.identity,
                new Vector3(1, 0, 1));
            Gizmos.DrawWireSphere(Vector3.zero, mainCollider.bounds.extents.x);
            Gizmos.matrix = oldMatrix;

            if (allowSwimming && mainCollider)
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Gizmos.color = new Color(.5f, .1f, 0.8f, 0.3f);
                if (mainCollider is BoxCollider box)
                    Gizmos.DrawCube(box.center, mainCollider.bounds.size);
                else if (mainCollider is SphereCollider sphere)
                    Gizmos.DrawSphere(sphere.center, sphere.radius);
                else if (mainCollider is CapsuleCollider capsule)
                {
                    DrawCapsule(capsule.center, Vector3.up, new Color(.5f, .1f, 0.8f, 0.3f), capsule.height / 2f,
                        capsule.radius);
                }
                else if (mainCollider is MeshCollider meshCollider)
                {
                    Gizmos.DrawMesh(meshCollider.sharedMesh, Vector3.zero, Quaternion.identity, Vector3.one);
                }
            }
        }

        private void DrawCapsule(Vector3 pos, Vector3 direction, Color color, float capsuleLength, float capsuleRadius)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(pos + (direction.normalized) * capsuleLength - (direction.normalized) * capsuleRadius,
                capsuleRadius);
            Gizmos.DrawWireSphere(pos - (direction.normalized) * capsuleLength + (direction.normalized) * capsuleRadius,
                capsuleRadius);

            Gizmos.DrawRay((Vector3.right) * capsuleRadius, (Vector3.up) * capsuleLength / 2f);
            Gizmos.DrawRay((Vector3.right) * capsuleRadius, (Vector3.down) * capsuleLength / 2f);

            Gizmos.DrawRay((Vector3.left) * capsuleRadius, (Vector3.up) * capsuleLength / 2f);
            Gizmos.DrawRay((Vector3.left) * capsuleRadius, (Vector3.down) * capsuleLength / 2f);

            Gizmos.DrawRay((Vector3.forward) * capsuleRadius, (Vector3.up) * capsuleLength / 2f);
            Gizmos.DrawRay((Vector3.forward) * capsuleRadius, (Vector3.down) * capsuleLength / 2f);

            Gizmos.DrawRay((Vector3.back) * capsuleRadius, (Vector3.up) * capsuleLength / 2f);
            Gizmos.DrawRay((Vector3.back) * capsuleRadius, (Vector3.down) * capsuleLength / 2f);
        }

        private void OnDrawGizmosSelected()
        {
            if (!mainCollider) return;

            var bounds = mainCollider.bounds;
            var size = Mathf.Round(Mathf.Log(bounds.extents.magnitude * 4f) + 1f);
            var arrowSize = Mathf.Clamp(size, 0f, 1f);
            var points = ScatterPoints(mainCollider, (int)(size));
            var offsetPoints = ScatterPoints(mainCollider, (int)(size), true);

            foreach (var p in points)
            {
                var f = useGravityMultiplierForLocomotion ? gravityMultiplierToApplyToLocomotion : 0;

                if (f == 0)
                {
                    Gizmos.color = Bilinear(new[,] { { Color.blue, Color.blue }, { Color.red, Color.red } }, Vector2.zero);

                    Gizmos.DrawSphere(p, .065f);
                    continue;
                }

                Gizmos.color = Bilinear(new[,] { { Color.black, Color.blue }, { Color.blue, Color.red } },
                    new Vector2(Mathf.Abs(f / 2), Mathf.Abs(f / 2)));

                ArrowGizmo(
                    p,
                    -f * .35f * arrowSize,
                    .1f * arrowSize);
            }

            foreach (var p in offsetPoints)
            {
                var f = useGravityMultiplierForItem ? gravityMultiplierToApplyToItem : 0;

                if (f == 0)
                {
                    Gizmos.color = Bilinear(new[,] { { Color.green, Color.green }, { Color.yellow, Color.yellow } },
                        Vector2.zero);

                    Gizmos.DrawSphere(p, .065f);
                    continue;
                }

                Gizmos.color = Bilinear(new[,] { { Color.black, Color.green }, { Color.green, Color.yellow } },
                    new Vector2(Mathf.Abs(f / 2), Mathf.Abs(f / 2)));


                ArrowGizmo(
                    p,
                    -f * .35f * arrowSize,
                    .1f * arrowSize);
            }
        }

        private static Color Bilinear(Color[,] corners, Vector2 uv)
        {
            var cTop = Color.Lerp(corners[0, 1], corners[1, 1], uv.x);
            var cBot = Color.Lerp(corners[0, 0], corners[1, 0], uv.x);
            var cUV = Color.Lerp(cBot, cTop, uv.y);
            return cUV;
        }

        private List<Vector3> ScatterPoints(Collider c, int count, bool offset = false)
        {
            var bounds = mainCollider.bounds;

            var sx = bounds.size.x / count * (offset ? 1.5f : 1f);
            var sy = bounds.size.y / count * (offset ? 1.5f : 1f);
            var sz = bounds.size.z / count * (offset ? 1.5f : 1f);

            var points = new List<Vector3>();
            for (int x = 0; x < count + 1; x++)
            {
                for (int y = 0; y < count + 1; y++)
                {
                    for (int z = 0; z < count + 1; z++)
                    {
                        var p = bounds.center - (offset ? bounds.extents : Vector3.zero) + new Vector3(
                            x * sx - bounds.extents.x,
                            y * sy - bounds.extents.y,
                            z * sz - bounds.extents.z);

                        if (IsPointWithinCollider(c, p))
                            points.Add(p);
                    }
                }
            }

            return points;
        }

        private static void ArrowGizmo(Vector3 pos, float magnitude, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (magnitude == 0) return;

            var direction = Vector3.up * magnitude;

            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);

            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        #endregion
    }
}