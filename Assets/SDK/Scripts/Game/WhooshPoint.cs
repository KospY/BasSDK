using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Whoosh")]
    public class WhooshPoint : MonoBehaviour
    {
        public Trigger trigger = Trigger.Always;
        public float minVelocity = 5;
        public float maxVelocity = 12;
        public bool stopOnSnap = true;

        public enum Trigger
        {
            Always,
            OnGrab,
            OnFly,
        }

    }
}