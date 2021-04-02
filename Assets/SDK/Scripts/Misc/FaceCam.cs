using UnityEngine;

namespace ThunderRoad
{
    public class FaceCam : MonoBehaviour
    {
        public bool useWorldUp = false;

        protected virtual void Update()
        {
            if (Camera.main)
            {
                this.transform.LookAt(2 * this.transform.position - Camera.main.transform.position, useWorldUp ? Vector3.up : Camera.main.transform.up);
            }
        }
    }
}
