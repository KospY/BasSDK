using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class Zone : MonoBehaviour
    {
        [System.Serializable]
        public class ZoneEvent : UnityEvent<Object> { }

        [Header("Navigation")]
        public bool navSpeedModifier;
        public float runSpeed = 0;

        [Header("Kill")]
        public bool killPlayer;
        public bool killNPC;

        [Header("Despawn")]
        public bool despawnNPC;
        public bool despawnItem;
        public float despawnDelay = 0;

        [Header("FX")]
        public bool spawnEffect;
        public string effectID;
        public AnimationCurve effectMassVelocityCurve;
        public Vector3 effectOrientation = Vector3.forward;

        [Header("Teleport")]
        public bool teleportPlayer;
        public bool teleportItem;
        public Transform customTeleportTarget;

        [Header("Event")]
        public ZoneEvent playerEnterEvent = new ZoneEvent();
        public ZoneEvent playerExitEvent = new ZoneEvent();
        public ZoneEvent creatureEnterEvent = new ZoneEvent();
        public ZoneEvent creatureExitEvent = new ZoneEvent();
        public ZoneEvent itemEnterEvent = new ZoneEvent();
        public ZoneEvent itemExitEvent = new ZoneEvent();

        private void Awake()
        {
            this.gameObject.layer = LayerMask.NameToLayer("Zone");
            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.isTrigger = true;
            }
        }

        public void TestEvent(Object obj)
        {
            // obj can be Player, Creature or Item, depending of the event
            Debug.Log("Hello " + obj.name);
        }
    }
}
