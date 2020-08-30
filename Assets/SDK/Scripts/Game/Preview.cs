using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Items/Preview")]
    public class Preview : MonoBehaviour
    {
        public float size = 1;

        protected virtual void OnDrawGizmosSelected()
        {
            size = transform.lossyScale.x;
            Gizmos.color = Common.HueColourValue(HueColorName.Green);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(size / this.transform.lossyScale.x, size / this.transform.lossyScale.y, 0));
            Common.DrawGizmoArrow(Vector3.zero, (Vector3.back * 0.3f) / this.transform.lossyScale.z, Color.blue, 0.15f / this.transform.lossyScale.z);
            Common.DrawGizmoArrow(Vector3.zero, (Vector3.up * 0.3f) / this.transform.lossyScale.y, Common.HueColourValue(HueColorName.Green), 0.15f / this.transform.lossyScale.y);
        }
    }
}