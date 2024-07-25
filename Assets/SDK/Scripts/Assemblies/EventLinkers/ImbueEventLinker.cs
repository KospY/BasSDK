using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/ImbueEventLinker.html")]
	[AddComponentMenu("ThunderRoad/Imbue Event Linker")]
    public class ImbueEventLinker : EventLinker
    {
        public enum ImbueEvent
        {
            OnEmpty = 0,
            OnNewSpell = 1,
            OnFill = 2,
            OnTryUse = 3,
            OnUseAbility = 4,
            OnHit = 5,
            OnHitEffect = 6,
            OnLinkerStart = 7,
        }

        [System.Serializable]
        public class ImbueUnityEvent
        {
            public ImbueEvent imbueEvent;
            public UnityEvent onActivate;
        }

        public ColliderGroup colliderGroup;
        public List<ImbueUnityEvent> imbueEvents = new List<ImbueUnityEvent>();

        private void OnValidate()
        {
            colliderGroup ??= GetComponent<ColliderGroup>();
        }


        public override void UnsubscribeNamedMethods()
        {
            // No named methods to unsubscribe
        }

    }
}
