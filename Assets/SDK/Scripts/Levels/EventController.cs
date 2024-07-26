using UnityEngine;
using UnityEngine.Events;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/EventController.html")]
    [AddComponentMenu("ThunderRoad/Levels/Event Controller")]
    public class EventController : MonoBehaviour
    {
        [Header("General")]
        public bool beginOnStart;
        public int maxInvoke = 999999999;
        public ConcurentInvokeBehaviour concurentInvokeBehaviour = ConcurentInvokeBehaviour.IgnoreIfRunning;

        public enum ConcurentInvokeBehaviour
        {
            IgnoreIfRunning,
            StopAndReplace,
            RunParallel,
        }

        [Header("Loop")]
        public float loopCount = 1;

        [Header("Delay")]
        public float minDelay = 0;
        public float maxDelay = 0;

        [Header("Multiple conditions"), Tooltip("Number of different index in case of multiple conditions")]
        public int invokeMaxIndex = 0;

        public UnityEvent timedEvent = new UnityEvent();
        [Button]
        public void Invoke()
        {
        }

        [Button]
        public void Invoke(int index)
        {
            
        }

        [Button]
        public void StopInvoke(int index)
        {
        }

        [Button]
        public void InvokeNow()
        {
        }

        [Button]
        public void StopInvoke()
        {
            
        }
    }
}