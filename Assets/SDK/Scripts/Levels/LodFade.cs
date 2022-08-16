using UnityEngine;
using System;
using System.Collections.Generic;

namespace ThunderRoad
{
	public class LodFade : MonoBehaviour
    {
        public List<MeshRenderer> meshRenderers;
        protected MaterialPropertyBlock materialPropertyBlock;

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void Set(float fade) // 1 is full opaque, 0 is fully transparent
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetVector("unity_LODFade", new Vector4(fade - 1, 0));
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}