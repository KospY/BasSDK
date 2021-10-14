using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UIItemSpawner : MonoBehaviour
    {
        public Transform spawnPoint;
        public bool showExistingOnly = true;

        [Header("References")]
        public GameObject iconPrefab;
        public GridLayoutGroup categoriesLayout;
        public GridLayoutGroup itemsLayout;
        public Container container;

        public GameObject categoriesPage;
        public GameObject itemsPage;

        public GameObject itemStatsPage;
        public GameObject itemPreviewPage;
        public Button spawnButton;

        protected Queue<Item> lastItemsSpawned = new Queue<Item>();

        protected void OnDrawGizmos()
        {
            if (spawnPoint) Gizmos.DrawWireSphere(spawnPoint.position, 0.1f);
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
        }

    }
}
