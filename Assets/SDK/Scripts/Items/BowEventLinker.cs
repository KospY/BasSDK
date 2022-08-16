using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/BowEventLinker")]
    [AddComponentMenu("ThunderRoad/Bows/Bow Event Linker")]
    public class BowEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum BowEvent
        {
            OnArrowAdd = 0,
            OnArrowRemoved = 1,
            OnArrowFired = 2,
            OnStringPullStart = 3,
            OnStringReleaseStart = 4,
            OnStringReleaseEnd = 5,
        }

        [System.Serializable]
        public class BowUnityEvent
        {
            public BowEvent bowEvent;
            public bool hasArrow;
            [Range(0f, 1f)]
            public float pullPercent = 0f;
            [Range(0f, 1f)]
            public float allowance = 1f;
            public UnityEvent onActivate;
        }

        public BowString bowString;
        public List<BowUnityEvent> bowEvents = new List<BowUnityEvent>();
        protected Dictionary<BowEvent, List<BowUnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            bowString ??= GetComponent<BowString>();
        }

    }
}
