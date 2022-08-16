using UnityEngine;

namespace ThunderRoad
{
    public class FaceCam : MonoBehaviour
    {
        public bool useWorldUp = false;

        private Transform mainCameraTransform;
        private Transform cachedTransform;

        private void Awake()
        {
            cachedTransform = transform;
        }

        private void Update()
        {
            if (mainCameraTransform == null)
            {
                if (Camera.main != null)
                {
                    mainCameraTransform = Camera.main.transform;
                }
            }
            else
            {
                cachedTransform.LookAt(2 * cachedTransform.position - mainCameraTransform.position, useWorldUp ? Vector3.up : mainCameraTransform.up);
            }
        }
    }
}
