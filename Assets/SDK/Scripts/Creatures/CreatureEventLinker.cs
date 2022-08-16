using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/CreatureEventLinker")]
	[AddComponentMenu("ThunderRoad/Creature Event Linker")]
    public class CreatureEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum CreatureEvent
        {
            OnDespawn = 0,
            OnHeal = 1,
            OnDamage = 2,
            OnKill = 3,
            OnResurrect = 4,
            OnFall = 5,
            OnTouchStart = 6,
            OnTouchEnd = 7,
            OnTouchNoDamageStart = 8,
            OnTouchNoDamageEnd = 9,
            OnGrab = 10,
            OnUngrab = 11,
            OnTotalUngrab = 12,
            OnTeleGrab = 13,
            OnTeleUngrab = 14,
            OnTotalTeleUngrab = 15,
            OnTeleRepelStart = 16,
            OnTeleRepelEnd = 17,
            OnTelePullStart = 18,
            OnTelePullEnd = 19,
            OnRagdollPreSlice = 20,
            OnRagdollPostSlice = 21,
            OnMeleeAttackStart = 22,
            OnMeleeAttackFinish = 23,
            OnRangedAttackStart = 24,
            OnRangedAttackFinish = 25,
            OnSpellAttackStart = 26,
            OnSpellAttackFinish = 27,
        }

        public enum LifeState
        {
            Either,
            OnlyAlive,
            OnlyDead
        }

        [System.Serializable]
        public class CreatureUnityEvent
        {
            public CreatureEvent creatureEvent;
            public LifeState aliveState;
            public UnityEvent onActivate;
        }

        public Creature creature;
        public List<CreatureUnityEvent> creatureEvents = new List<CreatureUnityEvent>();
        public Ragdoll ragdoll { get; protected set; }
        protected Dictionary<CreatureEvent, List<CreatureUnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            creature ??= GetComponent<Creature>();
        }

    }
}
