using UnityEngine;

namespace ThunderRoad
{
    [RequireComponent(typeof(Rigidbody))]
    public class IKTargetZone : MonoBehaviour
    {

        public Transform[] points = new Transform[0];
        public bool randomizeFoot;
        public bool interpolate;
        public Vector3 bounds;

        [SerializeField] private Side defaultFoot = Side.Left;
        public Vector3 centerOffset;

    }
}