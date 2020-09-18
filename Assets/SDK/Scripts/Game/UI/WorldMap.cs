using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class WorldMap : MonoBehaviour
    {
        public string mapId = "Default";
        public string orbAddress = "Bas.WorldMenu.MapOrb";
        public string detailAddress = "Bas.WorldMenu.MapDetail";
        public Transform locationRoot;
        public ToggleGroup orbGroup;
        public Transform canvasDetails;


        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 0));
            if (canvasDetails != null)
            {
                Gizmos.matrix = canvasDetails.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
            }
            if (locationRoot != null)
            {
                foreach (Transform child in locationRoot)
                {
                    Gizmos.matrix = child.localToWorldMatrix;
                    Gizmos.DrawRay(Vector3.zero, Vector3.back * 0.1f);
                    Gizmos.DrawWireSphere(new Vector3(0, 0, -0.1f - 0.015f), 0.03f);
                }
            }
        }
    }
}
