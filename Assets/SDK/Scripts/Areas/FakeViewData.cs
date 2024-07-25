namespace ThunderRoad
{
    using UnityEngine;

    public class FakeViewData : ScriptableObject
    {
        public int resolution = 512;
        public LayerMask mask = ~0;

        public Vector3 capturePosition = Vector3.zero;

        public Vector3 roomVolumePosition = Vector3.zero;
        public Vector3 roomVolumeRotation = Vector3.zero;
        public Vector3 roomVolumeScale = Vector3.zero;

        public Matrix4x4 capturedMatrix;
        public Cubemap captureTexture = null;
    }
}