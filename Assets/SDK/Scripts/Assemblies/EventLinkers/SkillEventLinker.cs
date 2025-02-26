using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/SkillEventLinker")]
    [AddComponentMenu("ThunderRoad/Skill Event Linker")]
    public class SkillEventLinker : EventLinker
    {
        public enum SkillEvent
        {
            OnLinkerStart = 0,
            PlayerSkillLoad = 1,
            PlayerSkillLateLoad = 2,
            PlayerSkillUnload = 3,
            NPCSkillLoad = 4,
            NPCSkillLateLoad = 5,
            NPCSkillUnload = 6,
        }

        [System.Serializable]
        public class SkillUnityEvent
        {
            public SkillEvent skillEvent;
            [Tooltip("Leave blank to link to any skill. Otherwise, put in a skill ID to make this work with a specific skill")]
            public string skillID;
            public UnityEvent<Creature> onActivate;

            public SkillUnityEvent Copy()
            {
                return new SkillUnityEvent()
                {
                    skillEvent = this.skillEvent,
                    onActivate = this.onActivate
                };
            }
        }

        [System.Serializable]
        public class PlayerSkillCounter
        {
            public bool exactMatch = false;
            public string skillIDSearch = "";
            public int minCount = 1;
            public int maxCount = 1;
            public UnityEvent onSuccess;
            public UnityEvent onFailure;

            public void Count()
            {

            }
        }

        public List<SkillUnityEvent> skillEvents = new List<SkillUnityEvent>();
        public List<PlayerSkillCounter> playerSkillCounters = new List<PlayerSkillCounter>();


        public override void UnsubscribeNamedMethods()
        {
        }

        public void CheckCounter(int index)
        {
        }

    }
}
