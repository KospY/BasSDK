using UnityEngine;
using System.Collections.Generic;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/Rope.html")]
    public class Rope : Handle
    {
        [Tooltip("Position of where the rope starts.")]
        public Transform ropeStart;
        [Tooltip("Position of where the rope ends.")]
        public Transform ropeTarget;
        [Tooltip("Depicts the radius of the rope mesh (Default : 0.03")]
        public float ropeRadius = 0.03f;
        [Tooltip("What material the rope uses when generated.")]
        public Material ropeMaterial;
        [Tooltip("Depicts if the rope has a collider")]
        public bool ropeUseCollider;
        [Tooltip("Depicts what layer the rope is on (default: 17)")]
        public int ropeLayer;
        [Tooltip("Depicts what physics material the rope uses.")]
        public PhysicMaterial ropePhysicMaterial;
        [Tooltip("(Obsolete) Depicts what is stated on the handle when the hand is in the handle radius.")]
        public string ropeTag = "Rope";

        [Header("Dynamic height")]
        [Tooltip("When enabled, the Rope Target is ignored, and instead the rope will use a raycast to depict the length of the rope.")]
        public bool dynamicHeight = false;
        [Tooltip("The maximum range of the raycast used to determine the rope length")]
        public float raycastRange = 50.0f;
        [Tooltip("Once the Dynamic height raycast is complete, how far from the ground do you want the rope to cut back by.")]
        public float heightFromGround = 2.0f;

        private bool isGenerated = false;


        protected virtual void OnDrawGizmos()
        {
            if (ropeTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ropeStart.position, ropeTarget.position);
            }
        }
    }
}
