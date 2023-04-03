using System.Collections;
using UnityEngine;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class BrainModuleElectrocute : BrainData.Module
    {
        public Vector2Int deadRotationSpringRange = new Vector2Int(10, 100);

        public int boltCount= 3;
        public float boltMinTime = 0.2f;

        public int imbueHitCount = 2;
        public float imbueHitMinTime = 2f;

    }
}