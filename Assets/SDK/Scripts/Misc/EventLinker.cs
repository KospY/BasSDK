using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EventLinker")]
	public abstract class EventLinker : LambdaEventHandler
    {
        public bool listening = true;

        protected Coroutine currentWait;
        protected WaitType currentWaitType;
        protected float currentWaitDuration;

        protected enum WaitType
        {
            FixedFrames,
            Frames,
            Seconds,
            SecondsRealtime,
        }


        // We want to make sure that the bool gets changed *next* frame, if it gets changed during the current frame, it may cause unintended behaviour
        public void SetListen(bool active)
        {
            StartCoroutine(SetListeningBool(active));
        }

        // Allows mod-makers to print debug statements in their event linker event handlers
        public void PrintDebug(string message)
        {
            if (message.StartsWith("!!")) Debug.LogError(message.Substring(2));
            else if (message.StartsWith("!")) Debug.LogWarning(message.Substring(1));
            else Debug.Log(message);
        }

        protected bool IsAlreadyWaiting()
        {
            if (currentWait != null)
            {
                Debug.LogError($"{gameObject.name} is already waiting! Currently performing a {currentWaitType} wait with a duration of {currentWaitDuration}");
                return true;
            }
            return false;
        }

        public void WaitForFixedFrames(int frames) => SetWait(WaitType.FixedFrames, frames);

        public void WaitForFrames(int frames) => SetWait(WaitType.Frames, frames);

        public void WaitForSeconds(float time) => SetWait(WaitType.Seconds, time);

        public void WaitForRealtimeSeconds(float time) => SetWait(WaitType.SecondsRealtime, time);

        protected void SetWait(WaitType waitType, float duration)
        {
            if (IsAlreadyWaiting()) return;
            if ((int)duration == 0) Debug.LogWarning($"{waitType} wait on {gameObject.name} has a duration of 0, and will not be executed.");
            currentWaitType = waitType;
            currentWaitDuration = duration;
            currentWait = StartCoroutine(ConfigWait(waitType, duration));
        }

        protected IEnumerator ConfigWait(WaitType waitType, float duration)
        {
            yield return null;
        }

        protected IEnumerator ExecuteInOrder(List<UnityEvent> events)
        {
            yield return null;
        }

        protected IEnumerator SetListeningBool(bool active)
        {
            yield return null;
        }
    }
}
