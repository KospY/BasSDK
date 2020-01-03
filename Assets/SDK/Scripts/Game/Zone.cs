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
        public bool spawnEffect;
        public string effectID;
        public AnimationCurve effectMassCurve;
        public AnimationCurve effectVelocityCurve;
        public Vector3 effectOrientation = Vector3.forward;
        [Header("Teleport")]
        public bool teleportPlayer;
        public bool teleportItem;
        public Transform customTeleportTarget;
    }
}
