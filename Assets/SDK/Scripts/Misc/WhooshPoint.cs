using System;
using UnityEngine;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/WhooshPoint")]
	[AddComponentMenu("ThunderRoad/Whoosh")]
    public class WhooshPoint : ThunderBehaviour
    {
        public Trigger trigger = Trigger.Always;
        public float minVelocity = 5;
        public float maxVelocity = 12;
        public float dampening = 0.1f;
        public bool stopOnSnap = true;

        public enum Trigger
        {
            Always,
            OnGrab,
            OnFly,
        }

    }
}
