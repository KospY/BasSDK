using UnityEngine;

namespace ThunderRoad
{
    public class MoveTransform : MonoBehaviour
    {
        public Transform target;
        public float speed = 1;
        public MovementType movementType = MovementType.Translate;

        protected Vector3 originalPosition;

        public enum MovementType
        {
            Translate,
            Lerp,
        }


        private void OnDrawGizmosSelected()
        {
            if (target == null)
            { return; }

            Vector3 origin = Application.isPlaying ? originalPosition : transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(origin, 0.15f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, target.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(target.position, 0.15f);
        }
    }
}