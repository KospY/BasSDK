using UnityEngine;

namespace BS
{
    public class DamagerDefinition : MonoBehaviour
    {
        public ColliderGroup colliderGroup;
        public Direction direction = Direction.All;
        public float penetrationLength = 0;
        public float penetrationDepth = 0f;
        public float penetrationContactMaxRadius = 0;

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

        protected void OnDrawGizmosSelected()
        {
            // Damage
            Gizmos.color = Color.red;
            if (direction == Direction.Forward) ItemDefinition.DrawGizmoArrow(this.transform.position, this.transform.forward * 0.05f, this.transform.right, Color.red, 0.05f, 10);
            if (direction == Direction.ForwardAndBackward)
            {
                ItemDefinition.DrawGizmoArrow(this.transform.position + this.transform.forward * penetrationDepth, this.transform.forward * 0.05f, this.transform.right, Color.red, 0.05f, 10);
                ItemDefinition.DrawGizmoArrow(this.transform.position + -this.transform.forward * penetrationDepth, -this.transform.forward * 0.05f, this.transform.right, Color.red, 0.05f, 10);
            }
            // Penetration
            if (penetrationDepth > 0)
            {
                Gizmos.color = Color.yellow;
                if (direction == Direction.Forward) Gizmos.DrawLine(this.transform.position, GetMaxDepthPosition(false));
                if (direction == Direction.ForwardAndBackward) Gizmos.DrawLine(this.transform.position + this.transform.forward * penetrationDepth, this.transform.position - this.transform.forward * penetrationDepth);
                Gizmos.DrawWireSphere(this.transform.position, penetrationContactMaxRadius);
                if (penetrationLength > 0)
                {
                    Gizmos.DrawRay(this.transform.position, this.transform.up * (penetrationLength * 0.5f));
                    Gizmos.DrawRay(this.transform.position, this.transform.up * -(penetrationLength * 0.5f));
                }
            }
        }
    }
}
