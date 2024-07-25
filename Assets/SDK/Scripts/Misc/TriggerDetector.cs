using UnityEngine;

namespace ThunderRoad
{
    public class TriggerDetector : MonoBehaviour
    {
        public delegate void TriggerEvent(Collider other);

        public event TriggerEvent OnTriggerEnterEvent;
        public event TriggerEvent OnTriggerExitEvent;
        public event TriggerEvent OnTriggerStayEvent;
        private void OnTriggerEnter(Collider other) => OnTriggerEnterEvent?.Invoke(other);
        private void OnTriggerExit(Collider other) => OnTriggerExitEvent?.Invoke(other);
        private void OnTriggerStay(Collider other) => OnTriggerStayEvent?.Invoke(other);
    }

}
