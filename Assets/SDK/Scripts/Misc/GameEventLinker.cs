using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/GameEventLinker")]
	[AddComponentMenu("ThunderRoad/Game Event Linker")]
    public class GameEventLinker : EventLinker
    {
        // These are explicitly assigned int values so that even if the order of the list gets changed, the assignments in prefabs and scenes will remain the same
        public enum GameEvent
        {
            OnCreatureSpawn = 0,
            OnCreatureHealed = 1,
            OnCreatureHit = 2,
            OnCreatureKill = 3,
            OnCreatureParry = 4,
            OnCreatureDeflect = 5,
            OnItemSpawn = 6,
            OnItemDespawn = 7,
        }

        public enum FromType
        {
            None,
            Any,
            Player,
            FriendNPC,
            EnemyNPC,
        }

        public enum ToType
        {
            Any,
            Self,
            Player,
            FriendNPC,
            EnemyNPC,
        }

        [System.Serializable]
        public class GameUnityEvent
        {
            public GameEvent gameEvent;
            [Tooltip("Defines the source creature for this event.")]
            public FromType from;
            [Tooltip("Defines the target creature for this event.")]
            public ToType to;
            public UnityEvent onActivate;
        }

        public List<GameUnityEvent> gameEvents = new List<GameUnityEvent>();
        protected Dictionary<GameEvent, List<GameUnityEvent>> eventsDictionary;

    }
}
