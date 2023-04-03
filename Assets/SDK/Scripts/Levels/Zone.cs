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

        [Tooltip("Causes the Player Exit event to be invoked immediately when the zone is loaded into the level.")]
        public bool invokePlayerExitOnAwake;

        [Header("Navigation")]
        [Tooltip("When ticked, Adjusts the speed of the player/NPC when inside the zone.")]
        public bool navSpeedModifier;
        [Tooltip("Speed adjustment of the player/NPC when inside the zone")]
        public float runSpeed = 0;

        [Header("Kill")]
        [Tooltip("When player enter the zone, the player dies.")]
        public bool killPlayer;
        [Tooltip("When NPC enter the zone, the NPC dies.")]
        public bool killNPC;

        [Header("Despawn")]
        [Tooltip("When NPC enters the zone, the NPC despawns.")]
        public bool despawnNPC;
        [Tooltip("When an Item enters the zone, the item despawns.")]
        public bool despawnItem;
        [Tooltip("Delay of which the NPC/Item despawns once they enter the zone.")]
        public float despawnDelay = 0;

        [Header("FX")]
        [Tooltip("Spawns Effect when the zone is entered by Player/NPC/Item")]
        public bool spawnEffect;
        [Tooltip("ID of the effect spawned.")]
        public string effectID;
        [Tooltip("This curve maps the (mass * velocity) of the interactor at the moment of entry to the intensity of the effect.")]
        public AnimationCurve effectMassVelocityCurve;
        [Tooltip("The euler rotation of the spawned effect.")]
        public Vector3 effectOrientation = Vector3.forward;

        [Header("Teleport")]

        [Tooltip("Teleports player to the Custom Teleport Target when player enters zone.")]
        public bool teleportPlayer;
        [Tooltip("Whether or not to keep the player's velocity when they teleport.")]
        public bool keepPlayerVelocity;
        [Tooltip("Teleports item(s) to the Custom Teleport Target when the item(s) enter the zone")]
        public bool teleportItem;
        [Tooltip("Whether or not to keep an item's velocity when it teleports.")]
        public bool keepItemVelocity;
        [Tooltip("GameObject of which the position is where the player/item teleports to.")]
        public Transform customTeleportTarget;

        [Header("Creature settings")]
        [Tooltip("If enabled, this zone will not work with the player creature.")]
        public bool ignorePlayerCreature = false;
        [Tooltip("If enabled, the zone will only react to the root bone of creatures (hip bone).")]
        public bool ignoreNonRootParts = true;

        [Header("Portals")]
        public List<ZonePortal> portals = new List<ZonePortal>();

        [Header("Event")]
        public ZoneEvent playerEnterEvent = new ZoneEvent();
        public ZoneEvent playerExitEvent = new ZoneEvent();
        public ZoneEvent creatureEnterEvent = new ZoneEvent();
        public ZoneEvent creatureExitEvent = new ZoneEvent();
        public ZoneEvent firstCreatureEnterEvent = new ZoneEvent();
        public ZoneEvent lastCreatureExitEvent = new ZoneEvent();
        public ZoneEvent itemEnterEvent = new ZoneEvent();
        public ZoneEvent itemExitEvent = new ZoneEvent();
        public ZoneEvent firstItemEnterEvent = new ZoneEvent();
        public ZoneEvent lastItemExitEvent = new ZoneEvent();
        public ZoneEvent firstEntityEnterEvent = new ZoneEvent();
        public ZoneEvent lastEntityExitEvent = new ZoneEvent();

        protected Collider mainCollider;

    }
}