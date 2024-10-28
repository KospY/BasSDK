using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(RagdollPart))]
    public class WeakPointController : MonoBehaviour
    {
        public int priority;
        public List<Transform> weakPoints;
        public Vector2Int minMaxActive = new Vector2Int(1, 1);

        [NonSerialized]
        public int added;
        public List<Transform> selected;
        private List<Transform> shuffled;

        public static void SelectWeakPoints(Ragdoll ragdoll, int targetCount, Action<List<Item>> callback, bool ignoreMinimumPerController = false)
        {
            var allControllers = ragdoll.GetComponentsInChildren<WeakPointController>();
            allControllers.OrderByDescending(wpc => wpc.priority);
            List<Transform> selected = new List<Transform>();
            for (int i = 0; i < allControllers.Length; i++)
            {
                WeakPointController controller = allControllers[i];
                selected.AddRange(controller.GetMinRandomlyPickedPoints());
            }
            // if we're under the maximum, 
            if (selected.Count < targetCount)
            {
                while (selected.Count < targetCount)
                {
                    if (!allControllers.WeightedFilteredSelectInPlace(wpc => wpc.selected.Count < wpc.minMaxActive.y, wpc => wpc.priority, out WeakPointController addController))
                    {
                        Debug.LogError("Can't add more weak points without violating weak point controller rules!");
                        break;
                    }
                    selected.Add(addController.GetNewRandPoint());
                }
            }
            // this is a rare case, hopefully; if the configured number of "minimum weak points" adds up to more than the max weak points, cull randomly based on count per controller
            if (ignoreMinimumPerController && selected.Count > targetCount)
            {
                while (selected.Count > targetCount)
                {
                    if (!allControllers.WeightedFilteredSelectInPlace(wpc => wpc.selected.Count > 1, wpc => wpc.selected.Count, out WeakPointController cullController))
                    {
                        Debug.LogError("Couldn't reduce number of weak points without violating weak point controller rules!");
                        break;
                    }
                    selected.Remove(cullController.RandRemove());
                }
            }
            List<Item> breakableWeakPoints = new List<Item>();
            foreach (Transform weakPoint in selected)
            {
                Item item = weakPoint.GetComponentInChildren<Item>();
                if (item == null || item.breakable == null)
                {
                    Debug.LogError("Something went wrong, a weak point is not configured with a breakable item!");
                    continue;
                }
                breakableWeakPoints.Add(item);
            }
            for (int i = 0; i < allControllers.Length; i++)
            {
                allControllers[i].RemoveAllNonSelected();
            }
            callback?.Invoke(breakableWeakPoints);
        }

        public List<Transform> GetMinRandomlyPickedPoints()
        {
            if (shuffled.IsNullOrEmpty()) Init();
            for (int i = 0; i < Mathf.Min(minMaxActive.x, shuffled.Count); i++)
            {
                selected.Add(shuffled[0]);
                shuffled.RemoveAt(0);
            }
            return selected;
        }

        public Transform GetNewRandPoint()
        {
            Transform point = shuffled[0];
            selected.Add(point);
            shuffled.Remove(point);
            return point;
        }

        public Transform RandRemove()
        {
            Transform point = selected[UnityEngine.Random.Range(0, selected.Count)];
            shuffled.Add(point);
            selected.Remove(point);
            return point;
        }

        public void RemoveAllNonSelected()
        {
            for (int i = weakPoints.Count - 1; i >= 0; i--)
            {
                if (selected.Contains(weakPoints[i])) continue;
                foreach (Item item in weakPoints[i].GetComponentsInChildren<Item>()) item.Despawn();
                Destroy(weakPoints[i].gameObject);
            }
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            selected = new List<Transform>();
            shuffled = new List<Transform>();
            shuffled.AddRange(weakPoints);
            shuffled.Shuffle();
            for (int i = shuffled.Count - 1; i >= 0; i--)
            {
                if (shuffled[i].GetComponentInChildren<Item>() is Item item && item.breakable != null) continue;
                shuffled.RemoveAt(i);
            }
            if (shuffled.Count < minMaxActive.x)
            {
                Debug.LogError($"{name}: Non-breakable weak points have been discarded! There are not enough weak points to meet this controller's minimum!");
            }
        }

        [Button]
        public void AddAllChildBreakableItemsAsWeakPoints(int includeParents = 0)
        {
            foreach (Breakable breakable in GetComponentsInChildren<Breakable>())
            {
                Transform weakPoint = breakable.transform;
                if (includeParents > 0)
                    for (int i = 0; i < includeParents; i++)
                        weakPoint = weakPoint.parent;
                weakPoints.Add(weakPoint);
            }
        }

        [Button]
        public void AttachWeakPoints()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("No RB on controller!");
                return;
            }
            foreach (Item item in GetComponentsInChildren<Item>())
            {
                item.gameObject.AddComponent<FixedJoint>().connectedBody = rb;
            }
        }
    }
}
