using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/ItemShelf.html")]
    public class ItemShelf : ThunderBehaviour
    {
        public List<Transform> shelfSpots;
        public Vector3 shelfSpotSize;
        public bool itemMustFitInBounds = true;
        public bool displayAtRandomSpots = false;
        private int previousChildCount = 0;

#if UNITY_EDITOR
        public int boxDrawCount = 100;
#endif

        private void Awake()
        {
            GetAllSpots();
        }

        private void OnValidate()
        {
            GetAllSpots();
        }

        private void GetAllSpots()
        {
            shelfSpots = new List<Transform>();
            foreach (Transform child in transform)
            {
                shelfSpots.Add(child);
            }
            shelfSpots = shelfSpots.OrderBy(s => Vector3.Distance(transform.position, s.position)).ToList();
            previousChildCount = shelfSpots.Count;
        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmos();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (UnityEditor.Selection.activeTransform?.parent == transform)
            {
                DrawGizmos();
            }
#endif
        }

        private void DrawGizmos()
        {
#if UNITY_EDITOR
            if (previousChildCount != transform.childCount)
            {
                GetAllSpots();
            }
            int count = 0;
#endif
            foreach (Transform position in shelfSpots)
            {
                Gizmos.matrix = Matrix4x4.TRS(position.position, position.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, shelfSpotSize);
#if UNITY_EDITOR
                count++;
                if (count >= boxDrawCount) break;
#endif
            }
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
