using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ThunderRoad
{
    public static class PointCacheGenerator
    {

        public enum MeshBakeMode
        {
            Vertex,
            Triangle
        }

        public enum Distribution
        {
            Sequential,
            Random,
            RandomUniformArea
        }

    }
}