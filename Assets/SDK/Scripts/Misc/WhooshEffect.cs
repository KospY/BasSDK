using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Whoosh Effect")]
    public class WhooshEffect : MonoBehaviour
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