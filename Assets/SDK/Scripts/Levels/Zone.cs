using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/Zone.html")]
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
        [FormerlySerializedAs("orientToEntranceXZNormal")]
        [Tooltip("Whether to orient the effect to point outwards from the point of exit, projected onto the XZ plane.")]
        public bool orientEffectToEntranceXZNormal = false;

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

        [Header("Statuses")]

        [Tooltip("If true, inflicts the status effects constantly to anything in the zone.")]
        public bool constantStatus;


        [Tooltip("When NPC/Item is in zone, does it apply a status effect?")]
        public bool playStatusEffects = true;
        [Tooltip("Does the status affect Players?")]
        public bool statusOnPlayer;
        [Tooltip("Does the status affect Creatures?")]
        public bool statusOnCreature;
        [Tooltip("Does the status affect Items?")]
        public bool statusOnItem;
        [Tooltip("When enabled, it will disable armor detection on NPCs inside the zone.")]
        public bool disableArmorDetection = false;
        [NonSerialized]
        public Dictionary<StatusData, (float, float?)> statuses;

        public enum CreatureForceMode
        {
            NoForce,
            ForceRoot,
            ForceParts,
        }

        [Header("Force")]
        [Tooltip("Does the applied force affect non-creatures?")]
        public bool forceNonCreatures;
        [Tooltip("Does the applied force affect the player?")]
        public bool forcePlayer;
        [Tooltip("How should the zone apply force to NPCs?\n\nNo Force: Applies no force\n\nForce Root: Applies force on the root of the creature\n\nForce Parts: Applies force to all parts of the creature. This will distabilize creatures beforehand")]
        public CreatureForceMode creatureForceMode = CreatureForceMode.NoForce;
        [Tooltip("Sets the origin point and orientation for forces applied by this zone")]
        public Transform forceTransform;
        [Tooltip("Does the force push the object in a linear direction? (X, Y, Z)")]
        public bool linearForceActive;
        [Tooltip("What force mode does the linear force apply?")]
        public ForceMode linearForceMode = ForceMode.Force;
        [Tooltip("What direction (relative to the force transform) does the linear force push the object?")]
        public Vector3 linearForce;
        [Tooltip("Does the force push outwards from the force transform point?")]
        public bool radialForceActive;
        [Tooltip("What force mode does the radial force apply?")]
        public ForceMode radialForceMode = ForceMode.Force;
        [Tooltip("Output radial force by distance from origin\n\nVertical axis: Force\n\nHorizontal axis: Distance")]
        public AnimationCurve radialForce = new AnimationCurve(new Keyframe(0f, 1f));
        [Tooltip("Output radial force gets multiplied by this value")]
        public float radialForceMultiplier = 1f;
        [Tooltip("Should downwards force be negated when applying radial force?")]
        public bool noDownwardsForce = false;
        [Tooltip("Does the force try to swirl objects around the force transform point?")]
        public bool swirlForceActive;
        [Tooltip("What force mode does the swirl force force apply?")]
        public ForceMode swirlForceMode = ForceMode.Force;
        [Tooltip("Output swirl force by distance from origin\n\nVertical axis: Force\n\nHorizontal axis: Distance")]
        public AnimationCurve swirlForce = new AnimationCurve(new Keyframe(0f, 1f));
        [Tooltip("Output swirl force gets multiplied by this value")]
        public float swirlForceMultiplier = 1f;
        [Tooltip("Picks a random direction (forwards or backwards) for every object that gets pushed by the swirl force")]
        public bool swirlRandomDirection;
        [Tooltip("How far objects should be pulled when swirled")]
        public float swirlForceDegrees;
        [Tooltip("Determines the axis around which objects are swirled, relative to the force transform.")]
        public Vector3 swirlLocalAxis = Vector3.up;
        [Tooltip("Does the force resist an object's velocity?")]
        public bool resistiveForceActive;
        [Tooltip("The amount of force to apply opposite to an object's movement")]
        public float resistiveForce;
        [Tooltip("What force mode does the resistive force apply?")]
        public ForceMode resistiveForceMode = ForceMode.Force;
        [Tooltip("A minimum velocity for the resistive force to activate")]
        public float minimumResistedVelocity = 0f;
        [Tooltip("Should the resistive force only work in one direction?" +
            "\n\nIf true, this allows the resistance force direction to update with the force transform direction" +
            "\n\nIf false, use the direction specified in resistive force direction")]
        public bool resistInForceTransformForward;
        [Tooltip("Should the resistive force only work in one direction? Only works if the magnitude of this vector is greater than 0")]
        public Vector3 resistiveForceDirection = Vector3.zero;
        [Tooltip("A secondary zone; if an object is in this zone, it doesn't receive force from this zone")]
        public Zone forceExclusionZone;

        [Tooltip("Cancel the existing velocity of incoming items")]
        public bool cancelItemVelocity = false;
        [Tooltip("Cancel the existing velocity of incoming NPCs")]
        public bool cancelCreatureVelocity = false;
        [Tooltip("Cancel the existing velocity of incoming player")]
        public bool cancelPlayerVelocity = false;

        [Tooltip("Multiply the velocity of an item on enter")]
        public float itemVelocityMultOnEnter = 1;
        [Tooltip("Multiply the velocity of an NPC on enter")]
        public float creatureVelocityMultOnEnter = 1;
        [Tooltip("Multiply the velocity of the player on enter")]
        public float playerVelocityMultOnEnter = 1;

        [Tooltip("An additional multiplier on force applied to items")]
        public float itemForceMult = 1;
        [Tooltip("An additional multiplier on force applied to NPCs")]
        public float creatureForceMult = 1;
        [Tooltip("An additional multiplier on force applied to the player")]
        public float playerForceMult = 1;

        [Header("Creature settings")]
        [Tooltip("If enabled, this zone will not work with the player creature.")]
        public bool ignorePlayerCreature = false;
        [Tooltip("If enabled, the zone will only react to the root bone of creatures (hip bone).")]
        public bool ignoreNonRootParts = true;
        [Tooltip("If enabled, this zone will not react to creatures who are idle.")]
        public bool ignoreIdleCreatures = false;
        [Tooltip("If enabled, creatures in or touching this zone will have physics culling forced off.")]
        public bool blockPhysicsCulling = false;

        public Ragdoll.State physicalCreatureState = Ragdoll.State.Destabilized;

        [Header("Portals")]
        public List<ZonePortal> portals = new List<ZonePortal>();

        [Header("Breakables")]
        public bool breakBreakables = false;

        [Header("Event")]
        public ZoneEvent playerEnterEvent = new ZoneEvent();
        public ZoneEvent playerExitEvent = new ZoneEvent();
        public ZoneEvent golemEnterEvent = new ZoneEvent();
        public ZoneEvent golemExitEvent = new ZoneEvent();
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