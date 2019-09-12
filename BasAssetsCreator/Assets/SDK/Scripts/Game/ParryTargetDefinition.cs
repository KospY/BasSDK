using UnityEngine;

namespace BS
{
    public class ParryTargetDefinition : MonoBehaviour
    {
        public float length = 0.25f;

        public Vector3 GetLineStart()
        {
            return this.transform.rotation * new Vector3(0, length / 2, 0) + this.transform.position;
        }

        public Vector3 GetLineEnd()
        {
            return this.transform.rotation * new Vector3(0, -length / 2, 0) + this.transform.position;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(GetLineStart(), GetLineEnd());
        }
    }
}
