using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ThunderRoad
{
    public class MeshToPointCache : MonoBehaviour
    {
        public Mesh mesh;

        public int pointCacheMapSize = 512;
        public int pointCachePointCount = 4096;
        public int pointCacheSeed = 0;
        public PointCacheGenerator.Distribution pointCacheDistribution = PointCacheGenerator.Distribution.RandomUniformArea;
        public PointCacheGenerator.MeshBakeMode pointCacheBakeMode = PointCacheGenerator.MeshBakeMode.Triangle;

        private VisualEffect vfx;

        private void Start()
        {
            vfx = GetComponent<VisualEffect>();
            SetMesh();
        }


        private void SetMesh()
        {
            if (mesh.isReadable)
            {
                PointCacheGenerator.PCache pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, pointCacheMapSize, pointCachePointCount, pointCacheSeed, pointCacheDistribution, pointCacheBakeMode);
                vfx.SetTexture("PositionMap", pCache.positionMap);
                //if (vfx.HasTexture("NormalMap")) vfx.SetTexture("NormalMap", pCache.normalMap);
            }
            else
            {
                Debug.LogError("Cannot access vertices on mesh " + mesh.name + " for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
            }
        }
    }
}
