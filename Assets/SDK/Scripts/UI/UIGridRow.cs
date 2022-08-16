using UnityEngine;

namespace ThunderRoad
{
    public class UIGridRow : MonoBehaviour
    {
        private int maxColumns;
        private int elements;

        public Canvas Canvas { get; private set; }
        public Transform CachedTransform { get; private set; }

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
            CachedTransform = transform;
        }

        public void Setup(int columns)
        {
            maxColumns = columns;
            elements = 0;
        }

        public bool IsRowFull()
        {
            return elements == maxColumns;
        }

        /// <summary>
        /// Add a new element to this row
        /// </summary>
        /// <param name="element">Element to add</param>
        /// <returns>Return true if the element was added and return false otherwise.</returns>
        public void AddElement(GameObject element)
        {
            if (element == null || elements == maxColumns) return;

            element.transform.SetParent(CachedTransform);
            elements++;
        }
    }
}