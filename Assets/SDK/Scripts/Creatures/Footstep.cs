using UnityEngine;

namespace ThunderRoad
{
    [RequireComponent(typeof(SphereCollider))]
    public class Footstep : MonoBehaviour
    {
        public string materialId = "Footstep";
        public float maxVelocity = 10;

    }
}