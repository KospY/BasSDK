using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public abstract class LambdaEventHandler : ThunderBehaviour
    {
        protected List<Action> anonymousHandlerUnsubscribers;

        [Button]
        public virtual void CheckConfiguredEvents()
        {
            UnsubscribeAllAnonymous();
            if (anonymousHandlerUnsubscribers == null) anonymousHandlerUnsubscribers = new List<Action>();
        }

        protected void UnsubscribeAllAnonymous()
        {
            if (anonymousHandlerUnsubscribers.IsNullOrEmpty()) return;
            foreach (Action anonymousHandlerUnsubscriber in anonymousHandlerUnsubscribers)
            {
                anonymousHandlerUnsubscriber.Invoke();
            }
            anonymousHandlerUnsubscribers.Clear();
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeAllAnonymous();
        }
    }
}
