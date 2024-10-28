using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class ItemBasket : ThunderBehaviour
    {
        public float spawnRadius;
        public float startSpawnHeight;
        public float spawnHeightIncrease;
        public float maxSpawnHeight;
        public List<string> allowedItems = new List<string>();

        public int GetMaxItems() => Mathf.FloorToInt((maxSpawnHeight - startSpawnHeight) / spawnHeightIncrease) + 1;

        public Vector3 GetRandomPositionAtHeight(int heightIndex)
        {
            return transform.position + new Vector3(0f, startSpawnHeight + (heightIndex * spawnHeightIncrease), 0f) + (new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * spawnRadius);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0f, startSpawnHeight, 0f), Vector3.up, spawnRadius);
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0f, maxSpawnHeight, 0f), Vector3.up, spawnRadius);
            if (spawnHeightIncrease > 0.01f)
            {
                for (float f = 0; f < maxSpawnHeight - startSpawnHeight; f += spawnHeightIncrease)
                {
                    UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0f, startSpawnHeight + f, 0f), Vector3.up, spawnRadius);
                }
            }
            Gizmos.color = Color.white;
            for (int i = 0; i < 4; i++)
            {
                Vector3 offset = new Vector3(i % 2 == 0 ? 0.0f : (i < 2 ? -spawnRadius : spawnRadius), 0f, i % 2 == 1 ? 0.0f : (i < 2 ? -spawnRadius : spawnRadius));
                Gizmos.DrawLine(transform.position + new Vector3(0f, startSpawnHeight, 0f) + offset, transform.position + new Vector3(0f, maxSpawnHeight, 0f) + offset);
            }
        }
#endif
    }
}
