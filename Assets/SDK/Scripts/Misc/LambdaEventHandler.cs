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
            if (anonymousHandlerUnsubscribers == null) anonymousHandlerUnsubscribers = new List<Action>();
        }

    }
}
