using UnityEngine;
using UnityEngine.XR;

namespace BS
{
    public class Mirror : MonoBehaviour
    {
        public ReflectionDirection reflectionDirection = ReflectionDirection.Up;
        [Range(0, 1)]
        public float quality = 1;
        public int maximumPerPixelLights = 2;
        public Color backgroundColor = Color.black;
        public LayerMask cullingMask = ~0;
        public Collider workingArea;
        public GameObject mirrorMesh;
        public MeshRenderer meshToHide;

        private Vector3 reflectionLocalDirection;
        private Vector3 reflectionWorldDirection;
        private Camera reflectionCamera;
        private Material material;
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
