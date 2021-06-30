using UnityEngine;
using UnityEngine.Events;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
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

        public UnityEvent timedEvent = new UnityEvent();

        protected int invokeCount = 0;
        protected Coroutine coroutine;

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
            if (timedEvent != null && invokeCount < maxInvoke)
            {
                if (concurentInvokeBehaviour == ConcurentInvokeBehaviour.IgnoreIfRunning)
                {
                    if (coroutine != null) return;
                    coroutine = StartCoroutine(InvokeCoroutine());
                }
                else if (concurentInvokeBehaviour == ConcurentInvokeBehaviour.StopAndReplace)
                {
                    if (coroutine != null) StopCoroutine(coroutine);
                    coroutine = StartCoroutine(InvokeCoroutine());
                }
                else if (concurentInvokeBehaviour == ConcurentInvokeBehaviour.RunParallel)
                {
                    StartCoroutine(InvokeCoroutine());
                }
                invokeCount++;
            }
        }

        [Button]
        public void InvokeNow()
        {
            if (timedEvent != null && invokeCount < maxInvoke)
            {
                timedEvent.Invoke();
                invokeCount++;
            }
        }

        [Button]
        public void StopInvoke()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        IEnumerator InvokeCoroutine()
        {
            for (int i = 0; i < loopCount; i++)
            {
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
                timedEvent.Invoke();
            }
            coroutine = null;
        }
    }
}