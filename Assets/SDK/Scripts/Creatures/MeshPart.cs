using UnityEngine;
using System;


#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{


    public class MeshPart : MonoBehaviour
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public PhysicMaterial defaultPhysicMaterial;
        public Texture2D idMap;

    }
}