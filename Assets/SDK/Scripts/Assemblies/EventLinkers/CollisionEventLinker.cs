using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/CollisionEventLinker.html")]
	[AddComponentMenu("ThunderRoad/Collision Event Linker")]
    public class CollisionEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum CollisionEvent
        {
            OnCollisionEnter = 0,
            OnHarmlessCollision = 1,
            OnCollisionExit = 2,
            OnDamageDealt = 3,
            OnKillDealt = 4,
            OnLinkerStart = 5,
        }

        [System.Serializable]
        public class CollisionUnityEvent
        {
            public CollisionEvent collisionEvent;
            public UnityEvent onActivate;
        }

        public ColliderGroup colliderGroup;
        public List<CollisionUnityEvent> collisionEvents = new List<CollisionUnityEvent>();
        public CollisionHandler collisionHandler { get; protected set; }
        protected Dictionary<CollisionEvent, List<CollisionUnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            colliderGroup ??= GetComponent<ColliderGroup>();
        }


        public override void UnsubscribeNamedMethods()
        {
            // No named methods to unsubscribe
        }

    }
}
