﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/UnityEventGrouper")]
	public class UnityEventGrouper : EventLinker
    {
        [System.Serializable]
        public class NamedEvent
        {
            public string name;
            public UnityEvent onActivate = new UnityEvent();
        }

        public List<NamedEvent> namedEvents = new List<NamedEvent>();
        private Dictionary<string, List<UnityEvent>> eventsDictionary = new Dictionary<string, List<UnityEvent>>();

        [Button]
        public void ActivateNamedEvent(string name)
        {
        }


        public override void UnsubscribeNamedMethods()
        {
            // No named methods to unsubscribe
        }
    }
}
