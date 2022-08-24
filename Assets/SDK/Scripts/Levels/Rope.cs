using UnityEngine;
using System.Collections.Generic;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Rope")]
    public class Rope : Handle
    {
        public Transform ropeStart;
        public Transform ropeTarget;
        public float ropeRadius = 0.03f;
        public Material ropeMaterial;
        public bool ropeUseCollider;
        public int ropeLayer;
        public PhysicMaterial ropePhysicMaterial;
        public string ropeTag = "Rope";

        [Header("Dynamic height")]
        public bool dynamicHeight = false;
        public float raycastRange = 50.0f;
        public float heightFromGround = 2.0f;


        protected override void OnDrawGizmosSelected()
        {
            if (ropeStart == null)
                return;

            // Dynamic
            if (dynamicHeight)
            {
                if (Physics.Raycast(ropeStart.position, -ropeStart.up, out RaycastHit hit, raycastRange, -1, QueryTriggerInteraction.Ignore))
                {
                    Gizmos.color = Color.blue;
                    Common.DrawGizmoCylinderFull(ropeStart.position, hit.point + (Vector3.up * heightFromGround), ropeRadius, 8);
                }
            }

            // Simple
            else if (ropeTarget != null)
            {
                Gizmos.color = Color.cyan;
                Common.DrawGizmoCylinderFull(ropeStart.position, ropeTarget.position, ropeRadius, 8);
            }

            // Base alters the gizmo matrix so call after we are done
            base.OnDrawGizmosSelected();
        }

        protected virtual void OnDrawGizmos()
        {
            if (ropeStart == null)
                return;

            // Dynamic
            if (dynamicHeight)
            {
                Gizmos.color = Color.red;
                Common.DrawGizmoArrow(ropeStart.position, -Vector3.up, Color.red, 0.1f);
            }

            // Simple
            else if (ropeTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ropeStart.position, ropeTarget.position);
            }


        }
    }
}
