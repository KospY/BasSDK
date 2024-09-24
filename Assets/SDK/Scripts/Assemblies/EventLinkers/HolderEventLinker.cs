using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/HolderEventLinker.html")]
	[AddComponentMenu("ThunderRoad/Holder Event Linker")]
    public class HolderEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum HolderEvent
        {
            OnSnap = 0,
            OnSnapFull = 1,
            OnUnsnap = 2,
            OnUnsnapEmpty = 3,
            OnHoverUsePress = 4,
            OnHoverUseRelease = 5,
            OnHoverAlternateUsePress = 6,
            OnHoverAlternateUseRelease = 7,
            OnLinkerStart = 8,
        }

        [System.Serializable]
        public class HolderUnityEvent
        {
            public HolderEvent holderEvent;
            public UnityEvent<Holder> onActivate;
        }

        public Holder holder;
        public List<HolderUnityEvent> holderEvents = new List<HolderUnityEvent>();
        protected Dictionary<HolderEvent, List<UnityEvent<Holder>>> eventsDictionary;

        private void OnValidate()
        {
            holder ??= GetComponent<Holder>();
        }



        public override void UnsubscribeNamedMethods()
        {
            // No named methods to unsubscribe
        }

    }
}
