using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class EventMessage : MonoBehaviour
    {
        
#if ODIN_INSPECTOR
        [HideLabel] 
#endif
        [TextArea(0, 20)]
        public string text;
        public string localizationGroupId;
        public string localizationStringId;
        public int priority;
        public float showDelay = 0;
        public string imageAddress;
        public string videoAddress;
        public bool fitVideoHorizontally;

        public bool warnPlayer = true; // Uses haptics until the player looked at the message
        public MessageAnchorType anchorType = MessageAnchorType.HandLeft;
        public Transform anchorTargetTransform; // Used only if anchor type is "Transform"

        public bool dismissAutomatically;
        public float dismissTime = 2f;

        [HideInInspector] public bool startFromZoneTrigger;
        [HideInInspector] public bool stopFromZoneTrigger;
        [HideInInspector] public bool isTutorialMessage;
        [HideInInspector] public bool isSkippable = true;

        public void ShowMessage()
        {
        }


        /// <summary>
        /// Reset the state of this event message
        /// </summary>
        public void ResetMessage()
        {
        }

        public void StopMessage()
        {
        }
    }
}