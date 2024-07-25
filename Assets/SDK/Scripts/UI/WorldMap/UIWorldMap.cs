using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public class UIWorldMap : MonoBehaviour
    {
        public string label;
        public Texture2D texture;
        public RawImage background;
        public ToggleGroup locationsToggleGroup;
        
        private void OnValidate()
        {
            background = this.GetComponentInChildren<RawImage>();
            background.texture = texture;
            int i = 0;
            foreach (Transform child in this.transform)
            {
                child.name = i++.ToString();
            }
        }

        private void OnTransformChildrenChanged()
        {
            int i = 0;
            foreach (Transform child in this.transform)
            {
                child.name = i++.ToString();
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float scaleX = transform.lossyScale.x;
            float scaleY = transform.lossyScale.y;

            foreach (Transform child in this.transform)
            {
                Gizmos.matrix = child.localToWorldMatrix;

                Handles.DrawWireDisc(child.position, child.forward, 0.03f);
                //draw a handle with the index
                Handles.Label(child.position, child.GetSiblingIndex().ToString());
            }
        }
#endif // UNITY_EDITOR
 
    }
}