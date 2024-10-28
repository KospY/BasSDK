using UnityEngine;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class RotateTowardHorizontally : MonoBehaviour
    {
        public Transform target;
        public bool onEnable = true;
        public float onEnableDelay = 0;
        public bool onUpdate;

        public void OnEnable()
        {
            StartCoroutine(EnableCoroutine());
        }

        private IEnumerator EnableCoroutine()
        {
            yield return null;
            if (onEnableDelay > 0) yield return new WaitForSeconds(onEnableDelay);
            UpdateTransform();
        }

        private void Update()
        {
            if (onUpdate)
            {
                UpdateTransform();
            }
        }

        public void UpdateTransform()
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.forward, Vector3.up), Vector3.up);
        }
    }
}