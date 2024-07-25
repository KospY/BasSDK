using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/PriceTagPoint.html")]
    [AddComponentMenu("ThunderRoad/Items/Price Tag Point")]
    public class PriceTagPoint : MonoBehaviour
    {
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {

            Gizmos.matrix = this.transform.localToWorldMatrix;

            Gizmos.DrawWireCube(new Vector3(0.02647252f, 0, -0.002977276f), new Vector3(0.07794505f, 0.025f, 0.006489262f));
            Common.DrawGizmoArrow(new Vector3(0, 0, -0.03f), Vector3.forward * 0.03f, Color.blue, 0.01f, 15);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Vector3(0, 0, -0.03f), Vector3.up * 0.01f);
        }
#endif
    }
}