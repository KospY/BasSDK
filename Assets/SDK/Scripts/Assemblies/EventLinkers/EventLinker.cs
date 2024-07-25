using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EventLinker")]
	public abstract class EventLinker : LambdaEventHandler
    {
        public bool listening = true;


        public override void CheckConfiguredEvents()
        {
            base.CheckConfiguredEvents();
            UnsubscribeNamedMethods();
        }

        protected override void ManagedOnEnable()
        {
        }

        protected override void ManagedOnDisable()
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnsubscribeNamedMethods();
        }

        public abstract void UnsubscribeNamedMethods();

        // We want to make sure that the bool gets changed *next* frame, if it gets changed during the current frame, it may cause unintended behaviour
        public void SetListen(bool active)
        {
        }

        // Allows mod-makers to print debug statements in their event linker event handlers
        public void PrintDebug(string message)
        {
        }

        public void WaitForFixedFrames(int frames)
        {
        }

        public void WaitForFrames(int frames)
        {
        }

        public void WaitForSeconds(float time)
        {
        }

        public void WaitForRealtimeSeconds(float time)
        {
        }

        protected void ExecuteInOrder(List<UnityEvent> events, Action preInvoke = null)
        {
        }

        protected void ExecuteInOrder<T>(List<UnityEvent<T>> events, T value, Action preInvoke = null)
        {
        }

        protected IEnumerator SetListeningBool(bool active)
        {
            yield return null;
        }
    }
}
