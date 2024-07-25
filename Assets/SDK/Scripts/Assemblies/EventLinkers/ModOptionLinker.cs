using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public abstract class ModOptionLinker<T, V> : EventLinker where V : UpdateChecker<T>
    {

        [System.Serializable]
        public class ValueInvoker
        {
            public List<UnityEvent<T>> events;
        }

        //[Header("Mod option")]
        public string optionDataID;
        [Space]
        public bool ignoreFirstUpdate = true;
        public List<UnityEvent<T>> onValueUpdateEvents;
        public List<ValueInvoker> valueInvokers;
        public List<V> specificValueCheckEvents;

        [NonSerialized]
        public ModOptionData<T> optionData;
        [NonSerialized]
        public T lastValue = default(T);

        public void Start()
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

        }

        private void LinkedValueChange(object obj)
        {
            if (!listening) return;
            if (obj is T input)
            {
                if (ignoreFirstUpdate)
                {
                    ignoreFirstUpdate = false;
                    return;
                }
                lastValue = input;
                ExecuteInOrder(onValueUpdateEvents, lastValue, null);
            }
            else
            {
                Debug.LogError("Mod option type is different some mod option linker type");
            }
        }

        public void RunAllValueCheckers()
        {
            for (int i = 0; i < specificValueCheckEvents.Count; i++)
            {
                var valueChecker = specificValueCheckEvents[i];
                if (valueChecker.Check(lastValue))
                {
                    ExecuteInOrder(valueChecker.valueMatchEvents, lastValue);
                }
            }
        }

        public void CheckOptionValue(int index)
        {
            if (specificValueCheckEvents.CountCheck(count => index < count))
            {
                return;
            }
            var valueChecker = specificValueCheckEvents[index];
            if (valueChecker.Check(lastValue))
            {
                ExecuteInOrder(valueChecker.valueMatchEvents, lastValue);
            }
        }

        public void RunValueInvokeEvents(int index)
        {
            if (valueInvokers.CountCheck(count => index < count))
            {
                return;
            }
            var valueInvoker = valueInvokers[index];
            ExecuteInOrder(valueInvoker.events, lastValue);
        }
    }

    [System.Serializable]
    public abstract class UpdateChecker<T>
    {
        public enum Comparison
        {
            Equals,
            NotEquals,
            Lesser,
            LesserOrEqual,
            GreaterOrEqual,
            Greater
        }

        public T comparedValue;
        public Comparison comparison;
        public List<UnityEvent<T>> valueMatchEvents;

        private float lastPrintTime = 0f;

        public abstract bool Check(T value);

        protected void PrintMessage(string message, bool error)
        {
            if (Time.time - lastPrintTime < 1f) return;
            if (error) Debug.LogError(message);
            else Debug.LogWarning(message);
            lastPrintTime = Time.time;
        }
    }
}
