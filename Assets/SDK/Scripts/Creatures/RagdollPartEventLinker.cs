using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollPartEventLinker")]
	[AddComponentMenu("ThunderRoad/Ragdoll Part Event Linker")]
    public class RagdollPartEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum PartEvent
        {
            OnDamage = 0,
            OnKill = 1,
            OnTouchStart = 2,
            OnTouchEnd = 3,
            OnTouchNoDamageStart = 4,
            OnTouchNoDamageEnd = 5,
            OnGrab = 6,
            OnUngrab = 7,
            OnTeleGrab = 8,
            OnTeleUngrab = 9,
            OnTeleRepelStart = 10,
            OnTeleRepelEnd = 11,
            OnTelePullStart = 12,
            OnTelePullEnd = 13,
            OnPreSlice = 14,
            OnPostSlice = 15,
        }

        public enum LifeState
        {
            Either,
            OnlyAlive,
            OnlyDead
        }

        [System.Serializable]
        public class PartUnityEvent
        {
            public PartEvent partEvent;
            public LifeState aliveState;
            public UnityEvent onActivate;
        }

        public RagdollPart part;
        public List<PartUnityEvent> partEvents = new List<PartUnityEvent>();
        public Ragdoll ragdoll { get; protected set; }
        public Creature creature { get; protected set; }
        protected Dictionary<PartEvent, List<PartUnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            part ??= GetComponent<RagdollPart>();
        }

    }
}
