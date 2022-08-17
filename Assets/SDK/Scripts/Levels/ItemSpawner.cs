using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ItemSpawner")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Item Spawner")]
    public class ItemSpawner : MonoBehaviour
    {
        public string itemId;
        public bool pooled;
        public bool spawnOnStart = true;
        public bool disallowDespawn;
        public int spawnCount = 1;
        public float randomRadius = 0;
        public bool randomRotate = false;

        public Transform holderObject = null;

        public enum VelocityReference
        {
            WorldSpace,
            SpawnerSpace,
            ItemSpace,
        }

        [Header("Spawn in motion")]
        [Tooltip("Sets the reference transform for the item's linear velocity on spawn.")]
        public VelocityReference linearVelocityMode = VelocityReference.WorldSpace;
        [Tooltip("Measured in meters per second")]
        public Vector3 linearVelocity = new Vector3();
        [Tooltip("Sets the reference transform for the item's angular velocity on spawn.")]
        public VelocityReference angularVelocityMode = VelocityReference.WorldSpace;
        [Tooltip("Measured in radians per second")]
        public Vector3 angularVelocity = new Vector3();
        [Tooltip("Set this to true to force the item to fly as though thrown.")]
        public bool forceThrow = false;
        [Tooltip("Use this to prevent the item from colliding with something else on spawn.")]
        public Collider ignoreCollisionCollider;
        [Tooltip("Use this to prevent the item from colliding with another item on spawn.")]
        public Item ignoreCollisionItem;
        [Tooltip("Use this to prevent the item from colliding with an input ragdoll on spawn.")]
        public Ragdoll ignoreCollisionRagdoll;
        [Tooltip("Set this to true if this item should act as though the player was the last handler.")]
        public bool setPlayerLastHandler = false;
        [Tooltip("Use this field to prevent the spawned item(s) from colliding with specific parts of the player body.")]
        public RagdollPart.Type ignoredPlayerParts = (RagdollPart.Type)(0);

        public void OnDrawGizmos()
        {
            if (randomRadius > 0)
            {
#if UNITY_EDITOR
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, randomRadius);
#endif
                Vector3 size = Vector3.one * 0.02f;
                for (int i = 0; i < spawnCount; i++)
                {
                    Random.InitState(gameObject.GetInstanceID() + i);
#if UNITY_EDITOR
                    UnityEditor.Handles.DrawWireCube(transform.position + Vector3.ProjectOnPlane(Random.insideUnitSphere * randomRadius, transform.up), size);
#endif
                    Random.InitState((int) Time.realtimeSinceStartup);
                }
            }
        }

                // The method signature should stay *outside* the ProjectCore condition so that modders can access and call this method using Unity events
        [Button]
        public void Spawn()
        {
        }
        
    }
}
