using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Zone")]
    public class Zone : ThunderBehaviour
    {
        [System.Serializable]
        public class ZoneEvent : UnityEvent<UnityEngine.Object> { }

        public bool invokePlayerExitOnAwake;

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

        [Header("Creature settings")]
        public bool ignorePlayerCreature = false;
        public bool ignoreNonRootParts = true;

        [Header("Portals")]
        public List<ZonePortal> portals = new List<ZonePortal>();

        [Header("Event")]
        public ZoneEvent playerEnterEvent = new ZoneEvent();
        public ZoneEvent playerExitEvent = new ZoneEvent();
        public ZoneEvent creatureEnterEvent = new ZoneEvent();
        public ZoneEvent creatureExitEvent = new ZoneEvent();
        public ZoneEvent itemEnterEvent = new ZoneEvent();
        public ZoneEvent itemExitEvent = new ZoneEvent();

#if PrivateSDK

        private int playerMask;
        private int creatureMask;
        private int itemMask;
        protected Collider mainCollider;
        protected Vector3 orgColliderSize;
        protected float orgColliderRadius;
        private EffectData effectData;

        private void Awake()
        {
            this.gameObject.layer = Common.zoneLayer;
            mainCollider = GetComponent<Collider>();
            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.isTrigger = true;
                if (mainCollider == null) mainCollider = collider;
            }
#if PrivateSDK
            playerMask = (1 << GameManager.GetLayer(LayerName.PlayerLocomotion));
            creatureMask = (1 << GameManager.GetLayer(LayerName.NPC))
                | (1 << GameManager.GetLayer(LayerName.Ragdoll))
                | (1 << GameManager.GetLayer(LayerName.BodyLocomotion))
                | (1 << GameManager.GetLayer(LayerName.Avatar));
            itemMask = (1 << GameManager.GetLayer(LayerName.MovingItem))
                | (1 << GameManager.GetLayer(LayerName.DroppedItem));
#endif
            if (invokePlayerExitOnAwake)
            {
                PlayerZoneChange(false);
            }
        }

        public void TestEvent(UnityEngine.Object obj)
        {
            // obj can be Player, Creature or Item, depending of the event
            Debug.Log("Hello " + obj.name);
        }

        [ShowInInspector, ReadOnly]
        public int collidersInside = 0;
        [ShowInInspector, ReadOnly]
        public bool playerInZone { get; protected set; } = false;
        protected int playerCollidersEntered = 0;
        [ShowInInspector, ReadOnly]
        public Dictionary<Creature, int> creaturesInZone { get; protected set; } = new Dictionary<Creature, int>();
        [ShowInInspector, ReadOnly]
        public Dictionary<Item, int> itemsInZone { get; protected set; } = new Dictionary<Item, int>();

        protected override void ManagedOnDisable()
        {
            // Disable collider by reducing is size to zero (so it trigger onExit, there is maybe a better solution?)
            if (mainCollider is BoxCollider)
            {
                orgColliderSize = (mainCollider as BoxCollider).size;
                (mainCollider as BoxCollider).size = Vector3.zero;
            }
            else if (mainCollider is SphereCollider)
            {
                orgColliderRadius = (mainCollider as SphereCollider).radius;
                (mainCollider as SphereCollider).radius = 0;
            }
            else if (mainCollider is CapsuleCollider)
            {
                orgColliderRadius = (mainCollider as CapsuleCollider).radius;
                (mainCollider as CapsuleCollider).radius = 0;
            }
        }

        protected override void ManagedOnEnable()
        {
            if (mainCollider is BoxCollider && orgColliderSize != Vector3.zero)
            {
                (mainCollider as BoxCollider).size = orgColliderSize;
            }
            else if (mainCollider is SphereCollider && orgColliderRadius != 0)
            {
                (mainCollider as SphereCollider).radius = orgColliderRadius;
            }
            else if (mainCollider is CapsuleCollider && orgColliderRadius != 0)
            {
                (mainCollider as CapsuleCollider).radius = orgColliderRadius;
            }
        }

        private void OnTriggerEnter(Collider other) => HandleTriggerChange(other, true);

        private void OnTriggerExit(Collider other) => HandleTriggerChange(other, false);

        protected bool IsInLayerMask(int layerMask, int layer) => layerMask == (layerMask | (1 << layer));

        protected void HandleTriggerChange(Collider collider, bool enter)
        {
            //Profiler.BeginSample("Zone state change");
            // We generally want the layer of the rigidbody attached to the collider, but if there is no attached rigidbody (which should be *exceptionally* rare), go with the collider layer
            int objectLayer = collider.attachedRigidbody?.gameObject.layer ?? collider.gameObject.layer;
            collidersInside += enter ? 1 : -1;
            if (IsInLayerMask(playerMask, objectLayer))
            {
                PlayerZoneChange(enter);
            }
            if (IsInLayerMask(creatureMask, objectLayer))
            {
                // If we want to ignore the player creature, best to do it here rather than after get component stuff. We know the Avatar layer is used only for the player creature
                if (ignorePlayerCreature && objectLayer == GameManager.GetLayer(LayerName.Avatar)) return;
                CreatureZoneChange(collider, enter, objectLayer == GameManager.GetLayer(LayerName.BodyLocomotion));
            }
            if (IsInLayerMask(itemMask, objectLayer))
            {
                ItemZoneChange(collider, enter);
            }
            //Profiler.EndSample();
        }

        protected void PlayerZoneChange(bool enter)
        {
            // Since there's only ever one player, we can just use a single int for this
            playerCollidersEntered += enter ? 1 : -1;
            if (enter && !playerInZone)
            {
                // Player entered the zone for the first time
                if (killPlayer) Player.currentCreature?.Kill();
                if (teleportPlayer) Player.local.Teleport(customTeleportTarget ? customTeleportTarget : PlayerSpawner.current.transform, true);
                if (portals.Count > 0)
                {
                    bool doEnterEvent = true;
                    foreach (ZonePortal zonePortal in portals)
                    {
                        if (zonePortal.IsInside(Player.local.head.transform.position))
                        {
                            zonePortal.enterEvent.Invoke(Player.local);
                            if (zonePortal.preventZoneEnterEvent)
                            {
                                doEnterEvent = false;
                            }
                        }
                    }
                    if (doEnterEvent) playerEnterEvent.Invoke(Player.local);
                }
                else
                {
                    playerEnterEvent.Invoke(Player.local);
                }
            }
            playerInZone = playerCollidersEntered > 0;
            if (!enter && !playerInZone)
            {
                if (portals.Count > 0)
                {
                    bool doExitEvent = true;
                    foreach (ZonePortal zonePortal in portals)
                    {
                        if (zonePortal.IsInside(Player.local.head.transform.position))
                        {
                            zonePortal.exitEvent.Invoke(Player.local);
                            if (zonePortal.preventZoneExitEvent)
                            {
                                doExitEvent = false;
                            }
                        }
                    }
                    if (doExitEvent) playerExitEvent.Invoke(Player.local);
                }
                else
                {
                    playerExitEvent.Invoke(Player.local);
                }
                playerCollidersEntered = 0;
            }
        }

        protected void CreatureZoneChange(Collider collider, bool enter, bool bodyLoc)
        {
            // For simplicity, we check that the attachedRigidbody is not null. We expect an attached rigidbody for any creature
            if (collider.attachedRigidbody == null) return;
            // For creatures, we need to do ragdoll part tracking
            Creature creature = null;
            RagdollPart part = null;
            // If this is the body locomotion layer, we know the only rigidbody this could be is going to be the creature game object
            if (bodyLoc)
            {
                if (collider.attachedRigidbody.TryGetComponent(out creature))
                {
                    // If creature is spawning, wait before triggering 
                    if (!creature.initialized || !creature.loaded)
                    {
                        StartCoroutine(WaitCreatureToLoad(creature, collider, enter, bodyLoc));
                        return;
                    }
                    // If the creature is physics-on, we ignore the body locomotion colliders
                    if (creature.ragdoll.IsPhysicsEnabled()) return;
                }
                else
                {
                    // No creature on this object
                    return;
                }
            }
            else if (collider.attachedRigidbody.TryGetComponent(out part))
            {
                // If creature is spawning, wait before triggering 
                if (!part.initialized)
                {
                    Creature creatureFromPart = part.GetComponentInParent<Creature>();
                    if (creatureFromPart)
                    {
                        StartCoroutine(WaitCreatureToLoad(creatureFromPart, collider, enter, bodyLoc));
                    }
                    else
                    {
                        Debug.LogError("RagdollPart " + part.name + " have no creature at root!");
                    }
                    return;
                }
                // If we're ignoring non-root parts, return if this is not the root part
                if (ignoreNonRootParts && part != part.ragdoll.rootPart) return;
                creature = part.ragdoll.creature;
            }
            // If creature is null or the creature is the player and we're ignoring the player creature, return
            if (creature == null) return;
            int start = creaturesInZone.TryGetValue(creature, out int current) ? current : 0;
            // If a collider exits the zone, but wasn't in it in the first place, do nothing
            if (start == 0 && !enter) return;
            creaturesInZone[creature] = start + (enter ? 1 : -1);
            if (enter && start == 0)
            {
                // Creature entered for the first time
                creatureEnterEvent.Invoke(creature);
                creature.InvokeZoneEvent(this, true);
                if (spawnEffect)
                {
                    if (creature.ragdoll.IsPhysicsEnabled())
                    {
                        SpawnZoneEffect(part != null ? part.transform.position : creature.ragdoll.rootPart.transform.position, creature.ragdoll.totalMass, creature.ragdoll.rootPart.rb.velocity.magnitude);
                    }
                    else
                    {
                        SpawnZoneEffect(creature.transform.position, creature.ragdoll.totalMass, creature.locomotion.rb.velocity.magnitude);
                    }
                }
                if (!creature.loaded) return;
                if ((!creature.isPlayer && killNPC) || (creature.isPlayer && killPlayer))
                {
                    creature.Kill();
                }
                if (!creature.isPlayer && despawnNPC)
                {
                    creature.Despawn(despawnDelay);
                }
            }
            if (!enter && creaturesInZone[creature] == 0)
            {
                creatureExitEvent.Invoke(creature);
                creature.InvokeZoneEvent(this, false);
                creaturesInZone.Remove(creature);
            }
        }

        public IEnumerator WaitCreatureToLoad(Creature creature, Collider collider, bool enter, bool bodyLoc)
        {
            while (!creature.initialized || !creature.loaded)
            {
                yield return new WaitForEndOfFrame();
            }
            CreatureZoneChange(collider, enter, bodyLoc);
        }

        protected void ItemZoneChange(Collider collider, bool enter)
        {
            // Items should *never* have no attached rigidbody. If they do, we ignore this
            if (collider.attachedRigidbody == null) return;
            if (!collider.attachedRigidbody.TryGetComponent(out Item item))
            {
                // It's possible we got the handle of a bow, or a part of a clothing piece. Use GetComponentInParent here
                item = collider.attachedRigidbody?.GetComponentInParent<Item>();
            }
            // If this is null, problems. Avoid by returning
            if (item == null) return;
            int start = itemsInZone.TryGetValue(item, out int current) ? current : 0;
            // If a collider exits the zone, but wasn't in it in the first place, do nothing
            if (start == 0 && !enter) return;
            itemsInZone[item] = start + (enter ? 1 : -1);
            if (enter && start == 0)
            {
                // Item entered for the first time
                item.InvokeZoneEvent(this, true);
                if (spawnEffect) SpawnZoneEffect(item.transform.position, item.rb.mass, item.rb.velocity.magnitude);
                if (teleportItem)
                {
                    if (item.IsHanded())
                    {
                        for (int i = item.handlers.Count - 1; i >= 0; i--)
                        {
                            item.handlers[i].UnGrab(false);
                        }
                    }
                    if (item.isTelekinesisGrabbed)
                    {
                        foreach (Handle handle in item.handles)
                        {
                            if (handle.telekinesisHandler)
                            {
                                handle.telekinesisHandler.telekinesis.TryRelease();
                            }
                        }
                    }
                    if (customTeleportTarget != null)
                    {
                        item.transform.SetPositionAndRotation(customTeleportTarget.position, customTeleportTarget.rotation);
                    }
                    else
                    {
                        item.transform.SetPositionAndRotation(PlayerSpawner.current.transform.position, PlayerSpawner.current.transform.rotation);
                    }
                }
                if (despawnItem && !item.IsHanded() && !item.isTelekinesisGrabbed)
                {
                    item.Despawn(despawnDelay);
                }

                itemEnterEvent.Invoke(item);
            }
            if (!enter && itemsInZone[item] == 0)
            {
                item.InvokeZoneEvent(this, false);
                if (spawnEffect) SpawnZoneEffect(item.transform.position, item.rb.mass, item.rb.velocity.magnitude);
                itemExitEvent.Invoke(item);
                itemsInZone.Remove(item);
            }
        }

        public void SpawnZoneEffect(Vector3 position, float mass, float velocityMagnitude)
        {
            if (!string.IsNullOrEmpty(effectID))
            {
                if (effectData == null || effectData.id != effectID) effectData = Catalog.GetData<EffectData>(effectID);
                // If the effect data is null even after attempting to get it, return so we don't hit NRE
                if (effectData == null) return;

                Vector3 hitPoint = mainCollider.ClosestPoint(position);
                EffectInstance effectInstance = effectData.Spawn(hitPoint, transform.rotation * Quaternion.LookRotation(effectOrientation));
                effectInstance.SetIntensity(effectMassVelocityCurve.Evaluate(mass * velocityMagnitude));
                effectInstance.source = this;
                effectInstance.Play();
            }
        }
#endif
    }
}
