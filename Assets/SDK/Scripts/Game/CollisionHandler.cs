using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Collision handler")]
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionHandler : MonoBehaviour
    {
        public bool active = true;
        public bool checkMinVelocity = true;
        public bool enterOnly = false;

    }
}
