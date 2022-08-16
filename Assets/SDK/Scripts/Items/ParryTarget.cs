using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ParryTarget")]
    public class ParryTarget : MonoBehaviour
    {
        public float length = 0.25f;

        public Vector3 GetLineStart()
        {
            return this.transform.position + (this.transform.up * length);
        }

        public Vector3 GetLineEnd()
        {
            return this.transform.position + (-this.transform.up * length);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(GetLineStart(), GetLineEnd());
        }

    }
}
