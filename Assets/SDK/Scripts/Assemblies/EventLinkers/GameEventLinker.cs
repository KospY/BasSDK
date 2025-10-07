using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TriInspector;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/GameEventLinker.html")]
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
            OnPlayerSpawn = 8,
            OnPlayerPossessCreature = 9,
            OnLinkerStart = 10,
            ItemBreakStart = 11,
            ItemBreakEnd = 12
        }

        public enum FromType
        {
            None,
            Any,
            Player,
            FriendNPC,
            EnemyNPC
        }

        public enum ToType
        {
            Any,
            Self,
            Player,
            FriendNPC,
            EnemyNPC
        }

        [System.Serializable]
        [DeclareHorizontalGroup("FromTo")]
        public class GameUnityEvent
        {
            public GameEvent gameEvent;
            private bool showFromTo => gameEvent == GameEvent.OnCreatureHealed
                || gameEvent == GameEvent.OnCreatureHit
                || gameEvent == GameEvent.OnCreatureKill
                || gameEvent == GameEvent.OnCreatureParry
                || gameEvent == GameEvent.OnCreatureDeflect;
            [ShowIf(nameof(showFromTo))]
            [Group("FromTo")]
            [Tooltip("Defines the source creature for this event.")]
            public FromType from;
            [ShowIf(nameof(showFromTo))]
            [Group("FromTo")]
            [Tooltip("Defines the target creature for this event.")]
            public ToType to;
            public UnityEvent onActivate;
        }

        [System.Serializable]
        public class LevelUnityEvent
        {
            public string levelID = "*";
            public string modeName = string.Empty;
            public List<LevelOptionCondition> optionValues = new List<LevelOptionCondition>();
            public UnityEvent onActivate;
        }

        [System.Serializable]
        public class LevelOptionCondition
        {
            public LevelOption levelOption = LevelOption.PlayerSpawnerId;
            public FilterLogic filter;
            public string value = "default";

            public bool Match(string input)
            {
                bool sameValue = input.ToLowerInvariant() == value.ToLowerInvariant();
                bool anyExcept = filter == FilterLogic.AnyExcept;
                return anyExcept != sameValue;
            }
        }

        public List<GameUnityEvent> gameEvents = new List<GameUnityEvent>();
        public List<LevelUnityEvent> linkerStartLevelEvents;
        public List<LevelUnityEvent> levelLoadEvents;
        public List<LevelUnityEvent> levelUnloadEvents;
        protected Dictionary<GameEvent, List<GameUnityEvent>> eventsDictionary;


        public override void UnsubscribeNamedMethods()
        {
        }

    }
}
