using UnityEngine;

namespace ThunderRoad
{
    public class CollisionListener : MonoBehaviour
    {
        public delegate void CollisionEvent(Collision other);
        public event CollisionEvent OnCollisionEnterEvent;
        public event CollisionEvent OnCollisionExitEvent;

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterEvent?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollisionExitEvent?.Invoke(other);
        }
    }
}
