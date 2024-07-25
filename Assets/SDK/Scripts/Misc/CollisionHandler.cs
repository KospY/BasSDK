using UnityEngine;
using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/CollisionHandler")]
    [AddComponentMenu("ThunderRoad/Collision handler")]
    public class CollisionHandler : ThunderBehaviour
    {
        public bool active = true;
        public bool checkMinVelocity = true;
        public bool forceFullIntensity = false;
        public bool enterOnly = false;
        public int forceAllowHitLocomotionLayer = -1;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<CollisionHandler> penetratedObjects = new List<CollisionHandler>();

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<Holder> holders = new List<Holder>();

    }
}
