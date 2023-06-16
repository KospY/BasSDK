using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ItemSpawner")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Item Spawner")]
    public class ItemSpawner : MonoBehaviour, ICheckAsset
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllItemOrLootTableID")]
#endif
        [Tooltip("The ID of the item/table specified in the Catelog")]
        [FormerlySerializedAs("itemId")]
        public string referenceId;

        public string itemId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }

        [Tooltip("When \"Item\" is selected, only Items in the catelog appear in the Reference Id.\nWhen \"LootTable\" is selected, LootTables appear in the catelog.")]
        public ReferenceType referenceType = ReferenceType.Item;

        public Priority priority = Priority.Default;

        public enum Priority
        {
            Mandatory,
            Default,
            IgnoreOnAndroid,
        }

        public enum ReferenceType
        {
            Item,
            LootTable,
        }

        [Tooltip("(Optional) If this spawner is a child of a parent, place parent in to this field. The parent will spawn first to prevent this item from spawning inside the parent.")]
        public ItemSpawner parentSpawner;

        [Tooltip("When ticked, will use items stored inside pool.")]
        public bool pooled;
        [Tooltip("Spawns items apon level load.")]
        public bool spawnOnStart = true;
        [Tooltip("When ticked, items spawned by this spawner will be despawned on wave start or after some time.")]
        public bool allowDespawn;
        [Tooltip("How many items spawn when spawner is initiated")]
        public int spawnCount = 1;
        [Tooltip("Radius of which items can spawn in, in random locations inside the radius.")]
        public float randomRadius = 0;
        [Tooltip("When enabled, items will be randomly rotated when spawned.")]
        public bool randomRotate = false;

        [Tooltip("(Optional) When Item is spawned, they will spawn inside referenced Holder.")]
        public Transform holderObject = null;
        [Tooltip("(Optional) When Item is spawned, they will spawn on referenced Rope or RopeSimple component.")]
        public RopeSimple ropeTemplate = null;

        public UnityEvent<Item> onSpawnEvent = new UnityEvent<Item>();

        [NonSerialized]
        public ItemSpawnerLimiter itemSpawnerLimiter;

        public enum VelocityReference
        {
            WorldSpace = 0,
            SpawnerSpace = 1,
            ItemSpace = 2,
            InheritWorldSpace = 3,
        }

        [Header("Spawn in Motion")]
        [Tooltip("Sets the reference transform for the item's linear velocity on spawn." +
            "\nWorld Space applies force in the world directions (X/Y/Z direction is world-based)." +
            "\nSpawner Space applies force in the item spawner's transform directions (X/Y/Z direction is based on spawner rotation)." +
            "\nItem Space applies force based on the item's rotation. (X/Y/Z direction is local to the item)" +
            "\nInherit World Space will make the spawned item match the motion of the item spawner, if it's on a moving physics body.")]
        public VelocityReference linearVelocityMode = VelocityReference.WorldSpace;
        [Tooltip("Measured in meters per second")]
        public Vector3 linearVelocity = new Vector3();
        [Tooltip("Sets the reference transform for the item's angular velocity on spawn." +
            "\nWorld Space applies torque in the world directions (X/Y/Z direction is world-based)." +
            "\nSpawner Space applies torque in the item spawner's transform directions (X/Y/Z direction is based on spawner rotation)." +
            "\nItem Space applies force torque on the item's rotation. (X/Y/Z direction is local to the item)" +
            "\nInherit World Space will make the spawned item match the motion of the item spawner, if it's on a moving physics body.")]
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
        [Tooltip("Set this to true if this item should act as though the player was the last handler. This means the item will not collide with the player when spawned.")]
        public bool setPlayerLastHandler = false;
        [Tooltip("Use this field to prevent the spawned item(s) from colliding with specific parts of the player body.")]
        public RagdollPart.Type ignoredPlayerParts = (RagdollPart.Type)(0);

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemOrLootTableID()
        {
            if (referenceType == ReferenceType.Item)
            {
                return Catalog.GetDropdownAllID(Category.Item);
            }
            else
            {
                return Catalog.GetDropdownAllID(Category.LootTable);
            }
        }
#endif

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (randomRadius > 0)
            {
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, randomRadius);
                Vector3 size = Vector3.one * 0.02f;
                for (int i = 0; i < spawnCount; i++)
                {
                    Random.InitState(gameObject.GetInstanceID() + i);
                    UnityEditor.Handles.DrawWireCube(transform.position + Vector3.ProjectOnPlane(Random.insideUnitSphere * randomRadius, transform.up), size);
                    Random.InitState((int)Time.realtimeSinceStartup);
                }
            }
            if (parentSpawner)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(this.transform.position, parentSpawner.transform.position);
            }
            if (priority == Priority.Mandatory) Gizmos.color = Color.green;
            else if (priority == Priority.Default) Gizmos.color = Color.blue;
            else if (priority == Priority.IgnoreOnAndroid) Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, Vector3.one / 2);
            Handles.Label(transform.position+Vector3.up*0.25f, this.referenceId);
        }
#endif



        public void SubscribeChildSpawner(ItemSpawner child)
        {
            
        }

        public void UnSubscribeChildSpawner(ItemSpawner child)
        {
            
        }

        public bool IsCurrentlySpawning()
        {
            return false;
 //ProjectCore
        }

        [Button]
        public void Spawn()
        {
            Spawn(true, true, true);
        }

        public int Spawn(bool checkParentSpawner = true, bool checkIgnoreOnAndroid = true, bool checkSpawnLimiter = true, System.Random randomGen = null, int maxItemCount = -1, bool allowDuplicates = true)
        {
            return 0;
        }

        /// <summary>
        /// Returns the item data linked to this ID int the catalog.
        /// If we use a loot table, use the random Pick() method.
        /// </summary>
        /// <returns>The item data in the catalog, null if not found</returns>
        private ItemData GetItemData(System.Random randomGen = null)
        {
            
            return null;
        }

        /// <summary>
        /// Places the item.
        /// Using a separate method allows for correctly placed gizmos
        /// </summary>
        /// <param name="item">item to place and align.</param>
        private void AlignAndPlaceItem(Item item, System.Random randomGen = null)
        {
            item.transform.MoveAlign(item.spawnPoint, this.transform);
            Vector3 insideUnitSphere;
            if (randomGen != null)
            {
                insideUnitSphere = new Vector3((float)randomGen.NextDouble(), (float)randomGen.NextDouble(), (float)randomGen.NextDouble());
                insideUnitSphere = insideUnitSphere.normalized * (float)randomGen.NextDouble();
            }
            else
            {
                insideUnitSphere = Random.insideUnitSphere;
            }

            item.transform.position += Vector3.ProjectOnPlane(insideUnitSphere * randomRadius, transform.up);
            if (randomRotate)
            {
                float randomValue = randomGen != null ? (float)randomGen.NextDouble() : Random.value;
                item.transform.rotation *= Quaternion.AngleAxis(randomValue * 360, transform.up);
            }

            
        }

        /// <summary>
        /// Resets "unused" items transformations.
        /// Useful for resetting props, as if they were spawning again
        /// </summary>
        /// <param name="forceDisallowDespawn">If true, ignore disallow despawn and spawn times equal to zero. Used by item spawners</param>
        public void ResetSpawnedItems(bool forceDisallowDespawn = true)
        {
        }

        /// <summary>
        /// Remove item from spawned items list (Used to keep track of what has been spawned).
        /// Automatically called on item despawn
        /// </summary>
        /// <param name="item">Item to remove from the list</param>
        public void UnloadFromSpawner(Item item)
        {
            
        }

#if UNITY_EDITOR
        public List<Issue> GetIssues(bool includeLongCheck, bool experimental)
        {
            List<Issue> issues = new List<Issue>();

            if (string.IsNullOrEmpty(itemId))
            {
                issues.Add(new Issue(this, "Item spawner have no Item or LootTable ID", false));
                return issues;
            }

            Catalog.EditorLoadAllJson();

            if (referenceType == ReferenceType.Item)
            {
                var item = Catalog.GetData<ItemData>(itemId, false);
                if (item == null)
                {
                    issues.Add(new Issue(this, "Item spawner item reference don't exist", false));
                }
            }
            else
            {
                var table = Catalog.GetData<LootTable>(itemId, false);
                if (table == null)
                {
                    issues.Add(new Issue(this, "Item spawner lot table reference don't exist", false));
                }
            }

            return issues;
        }
#endif
    }
}
