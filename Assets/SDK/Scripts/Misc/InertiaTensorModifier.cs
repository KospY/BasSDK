using UnityEngine;
using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class InertiaTensorModifier : MonoBehaviour
    {
        public Collider referenceCollider;
        public Vector3 customInertiaTensor;
        public Quaternion customInertiaTensorRotation;
        public bool applyCustomInertiaTensorOnStart = true;

    }
}
