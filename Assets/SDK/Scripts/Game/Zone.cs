using UnityEngine;

namespace BS
{
    public class Zone : MonoBehaviour
    {
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
        public bool spawnFx;
        public string lightFxID;
        public float massFxThreshold = 10;
        public float maxVelocityFxRatio = 8;
        public string heavyFxID;
        public Vector3 fxOrientation = Vector3.forward;
        [Header("Teleport")]
        public bool teleportPlayer;
        public bool teleportItem;
        public Transform customTeleportTarget;
    }
}
