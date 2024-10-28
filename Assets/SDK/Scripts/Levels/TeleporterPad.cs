using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/TeleporterPad.html")]
    public class TeleporterPad : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The teleporter activation zone")]
        public Zone activateZone;
        [Tooltip("Indicates the point which the player root will levitate to during teleportation.")]
        public Transform levitationTarget;
        [Tooltip("The FXController that the teleporter pad starts up to")]
        public FxController startupFxController;
        [Tooltip("The FXController that the teleporter pad plays when it is locked")]
        public FxController lockFxController;
        [Tooltip("The FXController that the teleporter pad plays when it is about to teleport")]
        public FxController teleportFxController;

        [Header("Parameters")]
        [Tooltip("Depicts the duration of the teleporter startup")]
        public float startupDuration = 4;
        [Tooltip("Depicts the duration of the teleporting period.")]
        public float teleportingDuration = 6;
        [Tooltip("Depicts the duration of the flash period of the teleporter.")]
        public float teleportingFlashDuration = 1;
        [Tooltip("Depicts the Animation curve of the levitation animation.")]
        public AnimationCurve levitationForceCurve;

        [Header("Events")]
        [Tooltip("Does the teleporter play an event? Reference an \"Event Load Level\" to load a specific level.")]
        public bool fireTeleportEvent;
        public UnityEvent onTeleport;

        protected bool teleporting;
        protected Coroutine startupCoroutine;
        protected Coroutine teleportingCoroutine;

    }
}