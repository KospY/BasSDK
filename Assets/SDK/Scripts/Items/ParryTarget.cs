using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ParryTarget")]
    public class ParryTarget : ThunderBehaviour
    {
        [Tooltip("Depicts the length of the ParryTarget. With this, AI will know how long your weapon is, and be able to parry it.\nCan be adjusted via button.")]
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
