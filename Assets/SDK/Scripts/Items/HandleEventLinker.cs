using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/HandleEventLinker")]
	[AddComponentMenu("ThunderRoad/Handle Event Linker")]
    public class HandleEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum HandleEvent
        {
            OnGrab = 0,
            OnRelease = 1,
            OnThrow = 2,
            OnSlideStart = 3,
            OnSlideStop = 4,
            OnSlideToUpHandle = 5,
            OnSlideToBottomHandle = 6,
            OnGrabbedUsePress = 7,
            OnGrabbedUseRelease = 8,
            OnGrabbedAlternateUsePress = 9,
            OnGrabbedAlternateUseRelease = 10,
            OnNonGrabbedUsePress = 11,
            OnNonGrabbedUseRelease = 12,
            OnNonGrabbedAlternateUsePress = 13,
            OnNonGrabbedAlternateUseRelease = 14,
            OnTeleGrab = 15,
            OnTeleRelease = 16,
            OnTeleThrow = 17,
            OnTeleSpinStartSuccess = 18,
            OnTeleSpinStartFail = 19,
            OnTeleSpinEnd = 20,
            OnTeleRepelStart = 21,
            OnTeleRepelEnd = 22,
            OnTelePullStart = 23,
            OnTelePullEnd = 24,
        }

        [System.Serializable]
        public class HandleUnityEvent
        {
            public HandleEvent handleEvent;
            public UnityEvent onActivate;
        }

        public Handle handle;
        public List<HandleUnityEvent> handleEvents = new List<HandleUnityEvent>();
        public Item item { get; protected set; }
        public RagdollPart part { get; protected set; }
        protected Dictionary<HandleEvent, List<UnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            handle ??= GetComponent<Handle>();
        }

    }
}
