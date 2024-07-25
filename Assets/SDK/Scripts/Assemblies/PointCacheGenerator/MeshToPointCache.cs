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


        private void SetMesh()
        {
        }
    }
}
