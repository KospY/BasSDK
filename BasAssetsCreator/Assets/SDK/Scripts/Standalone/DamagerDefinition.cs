using UnityEngine;

namespace BS
{
    public class DamagerDefinition : MonoBehaviour
    {
        public string colliderGroupName = "Default";
        public Direction direction = Direction.All;
        public float penetrationLength = 0;
        public float penetrationDepth = 0f;

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
            if (direction == Direction.Forward) Common.DrawGizmoArrow(this.transform.position, this.transform.forward * 0.05f, Color.red, 0.05f, 10);
            if (direction == Direction.ForwardAndBackward)
            {
                Common.DrawGizmoArrow(this.transform.position, this.transform.forward * 0.05f, Color.red, 0.05f, 10);
                Common.DrawGizmoArrow(this.transform.position, -this.transform.forward * 0.05f, Color.red, 0.05f, 10);
            }
            // Penetration
            if (penetrationDepth > 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(this.transform.position, GetMaxDepthPosition(false));
                if (penetrationLength > 0)
                {
                    Gizmos.DrawRay(this.transform.position, this.transform.up * (penetrationLength * 0.5f));
                    Gizmos.DrawRay(this.transform.position, this.transform.up * -(penetrationLength * 0.5f));
                }
            }
        }
    }
}
