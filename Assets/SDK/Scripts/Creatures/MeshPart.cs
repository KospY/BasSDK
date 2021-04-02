using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
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