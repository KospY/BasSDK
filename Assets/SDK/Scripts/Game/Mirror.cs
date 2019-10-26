using UnityEngine;

namespace BS
{
    public class Mirror : MonoBehaviour
    {
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
        public MeshRenderer meshToHide;

        private Vector3 reflectionLocalDirection;
        private Vector3 reflectionWorldDirection;
        private Camera reflectionCamera;
        private RenderTexture leftEyeRenderTexture;
        private RenderTexture rightEyeRenderTexture;

        public enum ReflectionDirection
        {
            Up,
            Down,
            Forward,
            Back,
            Left,
            Right,
        }
    }
}
