using System;
using UnityEngine;

namespace ThunderRoad
{
    /// <summary>
    /// This script allows events based around a creatures mouth(s) to be triggered and hooked into.
    /// </summary>
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/CreatureMouthRelay.html")]
    [RequireComponent(typeof(Rigidbody))]
    public class CreatureMouthRelay : ThunderBehaviour
    {
        [Tooltip("How big is the detection radius?")] public float mouthRadius = 0.05f;
        [Tooltip("Can this mouth receive food/liquid?")] public bool isMouthActive = true;
        [Tooltip("If enabled this relay will only be active if the current creature is the player.")] public bool playerOnly = false;

#if UNITY_EDITOR
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif
            if (!gameObject.activeInHierarchy) return;
            // Get the rigidbody.
            Rigidbody rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
#endif


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, mouthRadius);
        }
    }
}