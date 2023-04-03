using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ZonePortal")]
    public class ZonePortal : MonoBehaviour
    {
        public Vector3 size = Vector3.one;

        public bool preventZoneEnterEvent;
        public bool preventZoneExitEvent;

        public UnityEvent<UnityEngine.Object> enterEvent = new UnityEvent<UnityEngine.Object>();
        public UnityEvent<UnityEngine.Object> exitEvent = new UnityEvent<UnityEngine.Object>();


        private void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = new Color(0, 0, 0.5f, 0.25f);
            Gizmos.DrawCube(new Vector3(0, 0, size.z / 2), size);
            Gizmos.color = new Color(0, 0, 0.5f, 1f);
            Gizmos.DrawWireCube(new Vector3(0, 0, size.z / 2), size);
        }
    }
}