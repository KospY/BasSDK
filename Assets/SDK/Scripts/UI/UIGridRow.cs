using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIGridRow : MonoBehaviour
    {
        private int maxColumns;
        private int elements;
        private bool componentsEnabled = true; //components are enabled in the prefab by default
        private bool raycastTargetEnabled = true;
        
        public Canvas Canvas { get; private set; }
        public GraphicRaycaster GraphicRaycaster { get; private set; }
        //Holds reference to RaycastTarget components
        public List<RaycastTarget> RaycastableGraphics { get; private set; }
    
        public Transform CachedTransform { get; private set; }

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
            GraphicRaycaster = GetComponent<GraphicRaycaster>();
            CachedTransform = transform;
            
            RaycastableGraphics = new List<RaycastTarget>();

        }

        public void Setup(int columns)
        {
            maxColumns = columns;
            elements = 0;
            RaycastableGraphics ??= new List<RaycastTarget>();
            RaycastableGraphics.Clear();
            
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
            
            //We want to grab all the child RaycastTarget components in this element
            //So that we can disable them when they go out of view later
            var allGraphics = element.GetComponentsInChildren<RaycastTarget>();
            for (var i = 0; i < allGraphics.Length; i++)
            {
                RaycastTarget graphic = allGraphics[i];
                RaycastableGraphics.Add(graphic);
            }
            //if the components are disabled, then the raycastTargets should be disabled
            ToggleRaycastTargets(componentsEnabled);
        }

        
        public void ToggleComponents(bool active)
        {
            if (componentsEnabled != active)
            {
                componentsEnabled = active;
                if (Canvas) Canvas.enabled = active;
                if (GraphicRaycaster) GraphicRaycaster.enabled = active;
            }
            ToggleRaycastTargets(active);
        }

        public void ToggleRaycastTargets(bool active)
        {
            if(RaycastableGraphics.IsNullOrEmpty()) return;
  
            int raycastableGraphicsCount = RaycastableGraphics.Count;
            for (var i = 0; i < raycastableGraphicsCount; i++)
            {
                RaycastTarget raycastTarget = RaycastableGraphics[i];
                raycastTarget.raycastTarget = active;
            }
            
        }
    }
}