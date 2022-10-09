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
            ItemOnlyOnThrow = 2,
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
            ItemOnlyOnTeleThrow = 17,
            ItemOnlyOnTeleSpinStartSuccess = 18,
            OnTeleSpinStartFail = 19,
            ItemOnlyOnTeleSpinEnd = 20,
            OnTeleRepelStart = 21,
            OnTeleRepelEnd = 22,
            OnTelePullStart = 23,
            OnTelePullEnd = 24,
        }

        public enum InteractorSide
        {
            Either = 0,
            Right = 1,
            Left = 2
        }

        [System.Serializable]
        public class HandleUnityEvent
        {
            public HandleEvent handleEvent;
            public InteractorSide side;
            public UnityEvent onActivate;
        }

        public Handle handle;
        public List<HandleUnityEvent> handleEvents = new List<HandleUnityEvent>();
        public Item item { get; protected set; }
        public RagdollPart part { get; protected set; }
        protected Dictionary<HandleEvent, List<HandleUnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            handle ??= GetComponent<Handle>();
        }
    }
}
