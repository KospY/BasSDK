using UnityEngine;

namespace BS
{
    public class ParryDefinition : MonoBehaviour
    {
        public float lenght = 0.25f;

        public Vector3 GetLineStart()
        {
            return this.transform.position + (this.transform.up * lenght);
        }

        public Vector3 GetLineEnd()
        {
            return this.transform.position + (-this.transform.up * lenght);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(GetLineStart(), GetLineEnd());
        }
    }
}
