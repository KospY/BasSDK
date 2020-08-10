using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering;

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
        public MeshRenderer meshToHide;

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
