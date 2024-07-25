using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class WeakPointRandomizer : MonoBehaviour
    {
        public List<Group> groups;

        [System.Serializable]
        public class Group
        {
#if ODIN_INSPECTOR
            [LabelText("Group parent")]
#endif
            public Transform parent;
            public int priority;
            public Vector2 minMaxWeakPoints;

            public int selectedCount => selected.Count;

            private bool initialized = false;
            private List<Transform> allWeakPointOptions;
            private List<Transform> shuffled;
            private List<Transform> selected;

            private void Init()
            {
                allWeakPointOptions = new List<Transform>();
                shuffled = new List<Transform>();
                selected = new List<Transform>();
                foreach (Transform child in parent)
                {
                    if (child.GetComponentInChildren<SimpleBreakable>(true) != null) shuffled.Add(child);
                }
                if (shuffled.Count < minMaxWeakPoints.x)
                {
                    Debug.LogError($"All childs of {parent.name} without simple breakables have been ignored, which leaves it without enough for the specified minimum weakpoint count!");
                }
                allWeakPointOptions.AddRange(shuffled);
                shuffled.Shuffle();
                initialized = true;
            }

            public List<Transform> GetMinRandomlyPickedPoints()
            {
                if (!initialized) Init();
                int pick = Mathf.Min((int)minMaxWeakPoints.x, shuffled.Count);
                for (int i = 0; i < pick; i++)
                {
                    selected.Add(shuffled[0]);
                    shuffled.RemoveAt(0);
                }
                return selected;
            }

            public Transform GetNewRandPoint()
            {
                if (!initialized) Init();
                Transform point = shuffled[0];
                selected.Add(point);
                shuffled.Remove(point);
                return point;
            }

            public Transform RandRemove()
            {
                if (!initialized) Init();
                Transform point = selected[UnityEngine.Random.Range(0, selected.Count)];
                shuffled.Add(point);
                selected.Remove(point);
                return point;
            }

            public void RemoveAllNonSelected()
            {
                if (!initialized) Init();
                for (int i = allWeakPointOptions.Count - 1; i >= 0; i--)
                {
                    if (selected.Contains(allWeakPointOptions[i])) continue;
                    allWeakPointOptions[i].gameObject.SetActive(false);
                }
            }
        }

        public List<Transform> RandomizeWeakPoints(int targetCount, bool ignoreMinimumPerController = false)
        {
            groups.OrderByDescending(group => group.priority);
            List<Transform> selected = new List<Transform>();
            for (int i = 0; i < groups.Count; i++) selected.AddRange(groups[i].GetMinRandomlyPickedPoints());
            // if we're under the target, do weighted filtered select until we hit the target
            if (selected.Count < targetCount)
            {
                while (selected.Count < targetCount)
                {
                    if (!groups.WeightedFilteredSelectInPlace(group => group.selectedCount < group.minMaxWeakPoints.y, group => group.priority, out Group addGroup))
                    {
                        Debug.LogError("Can't add more weak points without violating weak point controller rules!");
                        break;
                    }
                    selected.Add(addGroup.GetNewRandPoint());
                }
            }
            // this is a rare case, hopefully; if the configured number of "minimum weak points" adds up to more than the max weak points, cull randomly based on count per controller
            if (selected.Count > targetCount)
            {
                while (selected.Count > targetCount)
                {
                    if (!groups.WeightedFilteredSelectInPlace(group => group.selectedCount > 1, group => group.selectedCount, out Group cullGroup) && !ignoreMinimumPerController)
                    {
                        Debug.LogError("Couldn't reduce number of weak points without violating weak point controller rules!");
                        break;
                    }
                    selected.Remove(cullGroup.RandRemove());
                }
            }
            for (int i = 0; i < groups.Count; i++) groups[i].RemoveAllNonSelected();
            return selected;
        }
    }
}