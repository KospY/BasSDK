using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace ThunderRoad
{
    public class Mirror : MonoBehaviour
    {
        public static Mirror local;
        public ReflectionDirection reflectionDirection = ReflectionDirection.Up;
        [Range(0, 1)]
        public float quality = 1;
        [Range(0, 1)]
        public float Intensity = 1f;
        public bool reflectionWithoutGI = true;
        public Color backgroundColor = Color.black;
        public LayerMask cullingMask = ~0;
        public Collider workingArea;
        public MeshRenderer mirrorMesh;
        public MeshRenderer[] meshToHide;

        public enum Side
        {
            Right,
            Left,
        }

        public enum ReflectionDirection
        {
            Up,
            Down,
            Forward,
            Back,
            Left,
            Right,
        }

        private Vector3 reflectionLocalDirection;
        private Vector3 reflectionWorldDirection;

        void OnValidate()
        {
            Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            if (reflectionDirection == ReflectionDirection.Forward) reflectionLocalDirection = Vector3.forward;
            else if (reflectionDirection == ReflectionDirection.Up) reflectionLocalDirection = Vector3.up;
            else if (reflectionDirection == ReflectionDirection.Back) reflectionLocalDirection = Vector3.back;
            else if (reflectionDirection == ReflectionDirection.Down) reflectionLocalDirection = Vector3.down;
            else if (reflectionDirection == ReflectionDirection.Left) reflectionLocalDirection = Vector3.left;
            else if (reflectionDirection == ReflectionDirection.Right) reflectionLocalDirection = Vector3.right;
            reflectionWorldDirection = this.transform.TransformDirection(reflectionLocalDirection);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            DrawGizmoArrow(this.transform.position, reflectionWorldDirection * 0.5f, Color.blue);
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}
