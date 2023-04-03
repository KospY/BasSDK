using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ControlEventLinker")]
	[AddComponentMenu("ThunderRoad/Player Control Event Linker")]
    public class ControlEventLinker : EventLinker
    {
        public enum ControlEvent
        {
            OnJumpPress,
            OnJumpRelease,
            OnKickPress,
            OnKickRelease,
            RightHandUsePress,
            RightHandUseRelease,
            RightHandGripPress,
            RightHandGripRelease,
            RightHandAlternateUsePress,
            RightHandAlternateUseRelease,
            LeftHandUsePress,
            LeftHandUseRelease,
            LeftHandGripPress,
            LeftHandGripRelease,
            LeftHandAlternateUsePress,
            LeftHandAlternateUseRelease,
        }

        [System.Serializable]
        public class ControlUnityEvent
        {
            public ControlEvent controlEvent;
            public UnityEvent onActivate;
        }

        public List<ControlUnityEvent> controlEvents = new List<ControlUnityEvent>();
        protected Dictionary<ControlEvent, List<UnityEvent>> eventsDictionary;

    }
}
