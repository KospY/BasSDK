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
        }

        [System.Serializable]
        public class DamagerUnityEvent
        {
            public DamagerEvent damagerEvent;
            public UnityEvent<Damager> onActivate;
        }

        public Damager damager;
        public List<DamagerUnityEvent> damagerEvents = new List<DamagerUnityEvent>();
        public CollisionHandler collisionHandler { get; protected set; }
        protected Dictionary<DamagerEvent, List<UnityEvent<Damager>>> eventsDictionary;

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
