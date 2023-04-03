using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIWorldMap : MonoBehaviour
    {
        public string label;
        public Texture2D texture;
        public RawImage background;

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

        private void  OnTransformChildrenChanged()
        {
            int i = 0;
            foreach (Transform child in this.transform)
            {
                child.name = i++.ToString();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (Transform child in this.transform)
            {
                Gizmos.matrix = child.localToWorldMatrix;
                Gizmos.DrawWireCube(new Vector3(0, -0.008f / child.lossyScale.x, 0), new Vector3(0.06f / child.lossyScale.x, 0.06f / child.lossyScale.x, 0));
            }
        }

    }
}