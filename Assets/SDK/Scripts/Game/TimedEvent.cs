using UnityEngine;
using UnityEngine.Events;

using EasyButtons;

namespace ThunderRoad
{
    public class TimedEvent : MonoBehaviour
    {
        public bool beginOnStart;
        public float minDelay = 2;
        public float maxDelay = 2;

        [System.Serializable]
        public class TimeEvent : UnityEvent { }

        public TimeEvent timedEvent = new TimeEvent();

        public virtual void Start()
        {
            if (beginOnStart)
            {
                Invoke();
            }
        }

        [Button]
        public void Invoke()
        {
            float delay = Random.Range(minDelay, maxDelay);
            Invoke("InvokeNow", delay);
        }

        [Button]
        public void InvokeNow()
        {
            if (timedEvent != null) timedEvent.Invoke();
        }
    }
}