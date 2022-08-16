using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/WhooshEffect")]
	[AddComponentMenu("ThunderRoad/Whoosh Effect")]
    public class WhooshEffect : ThunderBehaviour
    {
        public Trigger trigger = Trigger.Always;
        public float minVelocity = 5;
        public float maxVelocity = 12;
        public float dampening = 1f;
        public bool stopOnSnap = true;

        public List<GameObject> toggleGameobjects = new List<GameObject>();

        public enum Trigger
        {
            Always,
            OnGrab,
            OnFly,
        }

    }
}
