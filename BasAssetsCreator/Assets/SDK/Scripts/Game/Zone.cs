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
        [Header("Teleport")]
        public bool teleportPlayer;
        public Transform customTeleportTarget;
    }
}
