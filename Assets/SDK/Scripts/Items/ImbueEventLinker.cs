using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ImbueEventLinker")]
    [AddComponentMenu("ThunderRoad/Imbue Event Linker")]
    public class ImbueEventLinker : EventLinker
    {
        public enum ImbueEvent
        {
            OnEmpty,
            OnNewSpell,
            OnFill,
            OnTryUse,
            OnUseAbility,
            OnHit,
            OnHitEffect,
        }

        [System.Serializable]
        public class ImbueUnityEvent
        {
            public ImbueEvent imbueEvent;
            public UnityEvent onActivate;
        }

        public ColliderGroup colliderGroup;
        public List<ImbueUnityEvent> imbueEvents = new List<ImbueUnityEvent>();
        public CollisionHandler collisionHandler { get; protected set; }
        protected Dictionary<ImbueEvent, List<UnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            colliderGroup ??= GetComponent<ColliderGroup>();
        }

    }
}
