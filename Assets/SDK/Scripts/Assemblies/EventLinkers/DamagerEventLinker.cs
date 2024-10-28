using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/DamagerEventLinker.html")]
	[AddComponentMenu("ThunderRoad/Damager Event Linker")]
    public class DamagerEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum DamagerEvent
        {
            OnDamageDealt = 0,
            OnKillDealt = 1,
            OnLinkerStart = 2,
            OnPierceEnter = 3,
            OnUnpierce = 4,
            OnPierceThrough = 5,
        }

        [System.Serializable]
        public class DamagerUnityEvent
        {
            public DamagerEvent damagerEvent;
            public PierceTarget pierceTarget = PierceTarget.Anything;
            public UnityEvent<Damager> onActivate;
        }

        [System.Flags]
        public enum PierceTarget
        {
            Static = 1,
            Item = 2,
            Ragdoll = 4,
            Anything = 7,
        }

        public Damager damager;
        public List<DamagerUnityEvent> damagerEvents = new List<DamagerUnityEvent>();
        public CollisionHandler collisionHandler { get; protected set; }
        protected Dictionary<DamagerEvent, List<DamagerUnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            damager ??= GetComponent<Damager>();
        }


        public override void UnsubscribeNamedMethods()
        {
            // No named methods to unsubscribe
        }

    }
}
