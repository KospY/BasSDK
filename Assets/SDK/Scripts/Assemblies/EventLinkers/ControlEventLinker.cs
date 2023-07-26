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
            OnJumpPress = 0,
            OnJumpRelease = 1,
            OnKickPress = 2,
            OnKickRelease = 3,
            RightHandUsePress = 4,
            RightHandUseRelease = 5,
            RightHandGripPress = 6,
            RightHandGripRelease = 7,
            RightHandAlternateUsePress = 8,
            RightHandAlternateUseRelease = 9,
            LeftHandUsePress = 10,
            LeftHandUseRelease = 11,
            LeftHandGripPress = 12,
            LeftHandGripRelease = 13,
            LeftHandAlternateUsePress = 14,
            LeftHandAlternateUseRelease = 15,
            SpellPowerButtonPress = 16,
            OnLinkerStart = 17,
            OnPlayerCrouch = 18,
            OnPlayerUncrouch = 19,
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
