using System.Collections.Generic;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/ItemEventLinker.html")]
	[AddComponentMenu("ThunderRoad/Items/Item Event Linker")]
    public class ItemEventLinker : EventLinker, IToolControllable
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum ItemEvent
        {
            OnSpawn = 0,
            OnDespawn = 1,
            OnSnap = 2,
            OnUnsnap = 3,
            OnGrab = 4,
            OnUngrab = 5,
            OnDrop = 6,
            OnThrow = 7,
            OnGrabbedUsePress = 8,
            OnGrabbedUseRelease = 9,
            OnGrabbedAlternateUsePress = 10,
            OnGrabbedAlternateUseRelease = 11,
            OnNonGrabbedUsePress = 12,
            OnNonGrabbedUseRelease = 13,
            OnNonGrabbedAlternateUsePress = 14,
            OnNonGrabbedAlternateUseRelease = 15,
            OnTeleGrab = 16,
            OnTeleUngrab = 17,
            OnTeleDrop = 18,
            OnTeleThrow = 19,
            OnTeleSpinStartSuccess = 20,
            OnTeleSpinStartFail = 21,
            OnTeleSpinEnd = 22,
            OnFlyStart = 23,
            OnFlyEnd = 24,
            OnMagnetCatch = 25,
            OnMagnetRelease = 26,
            OnCollisionEnter = 27,
            OnHarmlessCollision = 28,
            OnCollisionExit = 29,
            OnDamageDealt = 30,
            OnKillDealt = 31,
            OnDamageReceived = 32,
            OnTeleRepelStart = 33,
            OnTeleRepelEnd = 34,
            OnTelePullStart = 35,
            OnTelePullEnd = 36,
            OnConsumed = 37,
            OnLinkerStart = 38,
        }

        [System.Serializable]
        public class ItemUnityEvent
        {
            public ItemEvent itemEvent;
            public UnityEvent onActivate;

            public ItemUnityEvent Copy()
            {
                return new ItemUnityEvent()
                {
                    itemEvent = this.itemEvent,
                    onActivate = this.onActivate
                };
            }
        }

        public Item item;
        public float justSpawnedTolerance = 0.1f;
        public List<ItemUnityEvent> itemEvents = new List<ItemUnityEvent>();
        protected Dictionary<ItemEvent, List<UnityEvent>> eventsDictionary;

        private void OnValidate()
        {
            item ??= GetComponent<Item>();
        }

        public void CopyFrom(IToolControllable original)
        {
        }

        public void Remove()
        {
        }


        public override void UnsubscribeNamedMethods()
        {
        }

    }
}
