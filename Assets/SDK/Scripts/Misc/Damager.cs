using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/Damager.html")]
    public class Damager : MonoBehaviour
    {
        public static bool easyDismemberment = false;
        
        [Tooltip("Specify Collider Group of which this damager will apply to.")]
        public ColliderGroup colliderGroup;
        [Tooltip("(Optional) Can reference collider inside group if it is only using one collider")]
        public Collider colliderOnly;
        [NonSerialized]
        public bool isColliderOnly = false;
        [Tooltip("Specify which direction the damager will deal damage in.\nAll is best for Blunt Damage.\nForward and Back is best for Slash Damage.\nForward is best for Piercing Damage.")]
        public Direction direction = Direction.All;
        [Tooltip("Length of which the item can pierce/slash with.\nSet to 0 for Blunt damage and single-point Pierce damage.")]
        public float penetrationLength = 0;
        [Tooltip("Depth of which a damager can deal slash damage.\nSet to 0 for Blunt damage")]
        public float penetrationDepth = 0f;
        [Tooltip("Once the Penetration Depth has reached its' max, unpierce from the object")]
        public bool penetrationExitOnMaxDepth;

 // ProjectCore
        public enum Direction
        {
            All,
            Forward,
            ForwardAndBackward,
        }

        [Flags]
        public enum PenetrationConditions
        {
            None = 0,
            DataAllowsPierce = 1,
            MaterialAllowsPierce = 2,
            HasDepth = 4,
            NotNPCHandler = 8,
            VelocityThreshold = 16,
            TargetAllowsPierce = 32,
            PartNotBlockingPierce = 64,
        }

        public Vector3 GetMaxDepthPosition(bool reverted = false)
        {
            return this.transform.position + ((reverted ? this.transform.forward : -this.transform.forward) * penetrationDepth);
        }

        public Vector3 GetMaxDepthPosition(float structDepth, bool reverted = false)
        {
            return this.transform.position + ((reverted ? this.transform.forward : -this.transform.forward) * structDepth);
        }

        [ContextMenu("Set colliderOnly from this")]
        public void GetColliderOnlyFromThis()
        {
            colliderOnly = this.GetComponent<Collider>();
            isColliderOnly = colliderOnly;
        }
#if UNITY_EDITOR
        protected float activePierceDepth = 0f;
        protected Vector3 localPierceDir = Vector3.zero;

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
                if (direction == Direction.Forward) Gizmos.DrawLine(this.transform.position, GetMaxDepthPosition());
                if (direction == Direction.ForwardAndBackward) Gizmos.DrawLine(this.transform.position + this.transform.forward * penetrationDepth, this.transform.position - this.transform.forward * penetrationDepth);
                if (penetrationLength > 0)
                {
                    Gizmos.DrawRay(this.transform.position, this.transform.up * (penetrationLength * 0.5f));
                    Gizmos.DrawRay(this.transform.position, this.transform.up * -(penetrationLength * 0.5f));
                }
            }
            Gizmos.color = Color.magenta;
            Vector3 pierceDepth = transform.position + (transform.TransformDirection(localPierceDir) * activePierceDepth);
            Gizmos.DrawLine(transform.position, pierceDepth);
        }
#endif        


        public void UnPenetrateAll()
        {
        }
    }
}
