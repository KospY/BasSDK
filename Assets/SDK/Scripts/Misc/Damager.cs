using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Damager")]
    public class Damager : MonoBehaviour
    {
        public ColliderGroup colliderGroup;        
        public Collider colliderOnly;
        [NonSerialized]
        public bool isColliderOnly = false;
        public Direction direction = Direction.All;
        public float penetrationLength = 0;
        public float penetrationDepth = 0f;
        [Tooltip("Experimental")]
        public bool penetrationExitOnMaxDepth;
        
        /// <summary>
        /// Buffer to hold raycast hits from the shield sweeptest.
        /// </summary>
        private static RaycastHit[] shieldSweepRayHits = new RaycastHit[10];

        public enum Direction
        {
            All,
            Forward,
            ForwardAndBackward,
        }

        public Vector3 GetMaxDepthPosition(bool reverted)
        {
            return this.transform.position + ((reverted ? this.transform.forward : -this.transform.forward) * penetrationDepth);
        }

        [ContextMenu("Set colliderOnly from this")]
        public void GetColliderOnlyFromThis()
        {
            colliderOnly = this.GetComponent<Collider>();
            isColliderOnly = colliderOnly;
        }

        protected void OnDrawGizmosSelected()
        {
            /*
            Vector3 sourcePosition = this.transform.position;
            Ray ray = new Ray(this.transform.position, -this.transform.forward);
            foreach (Collider collider in colliderGroup.colliders)
            {
                if (collider.Raycast(ray, out RaycastHit raycastHit, 2))
                {
                    collisionInstance.damageStruct.penetrationPoint.position = raycastHit.point;
                }
            }
    */
            // Damage
            Gizmos.color = Color.red;
            if (direction == Direction.Forward) Item.DrawGizmoArrow(this.transform.position, this.transform.forward * 0.05f, this.transform.right, Color.red, 0.05f, 10);
            if (direction == Direction.ForwardAndBackward)
            {
                Item.DrawGizmoArrow(this.transform.position + this.transform.forward * penetrationDepth, this.transform.forward * 0.05f, this.transform.right, Color.red, 0.05f, 10);
                Item.DrawGizmoArrow(this.transform.position + -this.transform.forward * penetrationDepth, -this.transform.forward * 0.05f, this.transform.right, Color.red, 0.05f, 10);
            }
            // Penetration
            if (penetrationDepth > 0)
            {
                Gizmos.color = Color.yellow;
                if (direction == Direction.Forward) Gizmos.DrawLine(this.transform.position, GetMaxDepthPosition(false));
                if (direction == Direction.ForwardAndBackward) Gizmos.DrawLine(this.transform.position + this.transform.forward * penetrationDepth, this.transform.position - this.transform.forward * penetrationDepth);
                if (penetrationLength > 0)
                {
                    Gizmos.DrawRay(this.transform.position, this.transform.up * (penetrationLength * 0.5f));
                    Gizmos.DrawRay(this.transform.position, this.transform.up * -(penetrationLength * 0.5f));
                }
            }
        }

    }
}
