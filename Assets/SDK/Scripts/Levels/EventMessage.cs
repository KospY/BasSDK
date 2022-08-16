using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace ThunderRoad
{
    public class EventMessage : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [TextArea(0, 20), HideLabel]
#endif
        public string text;
        public string localizationGroupId;
        public string localizationStringId;
        public int priority;
        public float showDelay = 0;
        public string imageAddress;
        public string videoAddress;
        public bool fitVideoHorizontally;

        public bool warnPlayer = true; // Uses haptics until the player looked at the message
        public Transform anchorTargetTransform; // Used only if anchor type is "Transform"

        public bool dismissAutomatically;
        public float dismissTime = 2f;

        public UnityEvent onMessageSkip;

        [HideInInspector] public bool startFromZoneTrigger;
        [HideInInspector] public bool stopFromZoneTrigger;
        [HideInInspector] public bool isTutorialMessage;

    }
}