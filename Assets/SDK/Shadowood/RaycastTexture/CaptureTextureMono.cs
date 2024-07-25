using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace Shadowood.RaycastTexture
{
    [ExecuteInEditMode]
    public class CaptureTextureMono : MonoBehaviour
    {
        public MeshRenderer targetMesh;
        public int res = 512;



#if UNITY_EDITOR
        public Texture2D result;
        public CaptureTexture captureTexture = new CaptureTexture();

        [Button]
        public void Capture()
        {
            result = captureTexture.CaptureTex(targetMesh, res);
        }

        [Button]
        public void Flatten()
        {
            var tmesh = captureTexture.FlattenMeshToUV(targetMesh);
            if (gameObject.GetComponent<MeshFilter>() == null) gameObject.AddComponent<MeshFilter>();
            gameObject.GetComponent<MeshFilter>().sharedMesh = tmesh;

            if (gameObject.GetComponent<MeshRenderer>() == null) gameObject.AddComponent<MeshRenderer>();
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = captureTexture.mat;
        }

        private void OnEnable()
        {
            if (captureTexture.showCamera) captureTexture.DrawNowAdd();
        }

        private void OnDisable()
        {
            captureTexture.DrawNowRemove();
        }
#endif        
    }

}
