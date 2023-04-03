using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

namespace ThunderRoad
{
    public class InputHoldTimerEvents : MonoBehaviour
    {
        public AnimationCurve inputTimeCurve = new AnimationCurve();
        public bool autoActivateIfPastCurveEnd = false;
        public bool useRealTime = false;
        [Tooltip("Input time, output an integer value corresponding with to the event in the list below. Lists and arrays start at 0 and go up from there! The events are always in order!")]
        public List<UnityEvent> events = new List<UnityEvent>();

        [NonSerialized]
        public float inputTrueTime;
        [NonSerialized]
        public bool input = false;
        [NonSerialized]
        public float inputFalseTime;
        [NonSerialized]
        public int lastEventOutput;

        protected float curveStartTime;
        protected float curveEndTime;

        private Coroutine waitUntilEndTime;

        private float currentTime => useRealTime ? Time.realtimeSinceStartup : Time.time;

        private void Start()
        {
            curveStartTime = inputTimeCurve.GetFirstTime();
            curveEndTime = inputTimeCurve.GetLastTime();
        }

        public void SetInput(bool active)
        {
            if (active && !input)
            {
                inputTrueTime = currentTime;
                if (autoActivateIfPastCurveEnd) waitUntilEndTime = StartCoroutine(WaitForCurveEnd());
            }
            if (!active && input)
            {
                inputFalseTime = currentTime;
                if (waitUntilEndTime != null) StopCoroutine(waitUntilEndTime);
                ActivateEvent(inputFalseTime - inputTrueTime);
            }
            input = active;
        }

        private void ActivateEvent(float inputTime)
        {
            if (inputTime < curveStartTime) return;
            int curveOutput = Mathf.RoundToInt(inputTimeCurve.Evaluate(Mathf.Clamp(inputTime, curveStartTime, curveEndTime)));
            if (curveOutput >= events.Count)
            {
                Debug.LogError($"Curve output ({curveOutput}) is out of the index range for events! Perhaps you forgot to remove curve keys, or adjust curve output values?");
                return;
            }
            events[curveOutput].Invoke();
        }

        private IEnumerator WaitForCurveEnd()
        {
            if (useRealTime) yield return Yielders.ForRealSeconds(curveEndTime);
            else yield return Yielders.ForSeconds(curveEndTime);
            ActivateEvent(curveEndTime);
            input = false;
            waitUntilEndTime = null;
        }
    }
}
