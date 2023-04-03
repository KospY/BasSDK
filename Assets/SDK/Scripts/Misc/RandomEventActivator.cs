using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RandomEventActivator")]
    public class RandomEventActivator : ThunderBehaviour
    {
        public List<UnityEvent> eventList = new List<UnityEvent>();

        public void ShuffleEventList() => eventList = eventList.Shuffle();


        public void ActivateRandomFromAll() => ActivateRandom(eventList.Count, false);

        public void ActivateRandomFromAllMoveToEnd() => ActivateRandom(eventList.Count, true);

        public void ActivateRandomInRange(int range) => ActivateRandom(range, false);

        public void ActivateRandomInRangeMoveToEnd(int range) => ActivateRandom(range, true);

        public void ActivateRandom(int range, bool moveToEnd) => ActivateEventAtIndex(Random.Range(0, range), moveToEnd);


        public void ActivateEventAtIndex(int index) => ActivateEventAtIndex(index, false);

        public void ActivateEventAtIndexMoveToEnd(int index) => ActivateEventAtIndex(index, true);

        public void ActivateEventAtIndex(int index, bool moveToEnd)
        {
            UnityEvent toActivate = eventList[index];
            toActivate?.Invoke();
            if (moveToEnd)
            {
                eventList.RemoveAt(index);
                eventList.Add(toActivate);
            }
        }
    }
}
