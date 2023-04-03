using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/PlayerTeleporter")]
    public class PlayerTeleporter : MonoBehaviour
    {
        public Transform targetTeleportTransform;
        public bool useFading = true;
        public float fadeInDuration = 1;
        public float fadeOutDuration = 1;

        public UnityEvent onPlayerTeleport;

        /// <summary>
        /// Teleport the player to the target transform. If use Fading is set to true, fades in a coroutine.
        /// </summary>
        public void Teleport()
        {
        }

        private void Teleport(Transform t)
        {
        }


        private void OnDrawGizmos()
        {
            if (!targetTeleportTransform) return;

            Gizmos.color = new Color(.05f, 1f, .7f);
            var position = targetTeleportTransform.position;
            Gizmos.DrawLine(position, position + Vector3.up * 100);
            var forward = targetTeleportTransform.forward;
            Gizmos.DrawLine(position, position + forward * 3);
            Gizmos.DrawLine(position + Vector3.up * 3, position + forward * 3);
        }
    }
}