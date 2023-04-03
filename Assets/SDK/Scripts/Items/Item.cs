using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Item")]
    [AddComponentMenu("ThunderRoad/Items/Item")]
    public class Item : ThunderBehaviour
    {
        public static List<Item> all = new List<Item>();
        public static List<Item> allActive = new List<Item>();
        public static List<Item> allThrowed = new List<Item>();
        public static HashSet<Item> allMoving = new HashSet<Item>();
        public static List<Item> allTk = new List<Item>();
        public static List<Item> allWorldAttached = new List<Item>();

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllItemID")]
#endif
        [Tooltip("The Item ID of the item specified in the Catelog")]
        public string itemId;
        [Tooltip("Specifies the Holder Point of the item. This specifies the position and rotation of the item when held in a holder, such as on player hips, back, on an item rack etc. For hips/back, the Z axis/blue arrow specifies towards the floor.")]
        public Transform holderPoint;
        [Tooltip("Specifies the spawn point of the item. This specifies the position and rotation of the item when spawned, it's mostly used for itemSpawner")]
        public Transform spawnPoint;
        [Tooltip("Can add additional holderpoints for different interactables (such as home rack)")]
        public List<HolderPoint> additionalHolderPoints = new List<HolderPoint>();
        [Tooltip("Shows the point that AI will try to block with when they are holding the weapon.")]
        public Transform parryPoint;
        [Tooltip("Specifies what handle is grabbed by default for the Right Hand")]
        public Handle mainHandleRight;
        [Tooltip("Specifies what handle is grabbed by default for the Left Hand")]
        public Handle mainHandleLeft;
        [Tooltip("Used to point in direction when thrown.\nZ-Axis/Blue Arrow points forwards.")]
        public Transform flyDirRef;
        [Tooltip("States the Preview for the item.")]
        public Preview preview;
        [Tooltip("Tick if the item is attached to the world and not spawned via item spawner.")]
        public bool worldAttached;
        [Tooltip("Radius to depict how close this item needs to be to a creature before the creatures' collision is enabled.")]
        public float creaturePhysicToggleRadius = 2;
        [Tooltip("Allows user to adjust the center of mass on object.\nIf unticked, this is automatically adjusted. When ticked, adds a custom gizmo to adjust.\n \nUse this if weight on the item is acting strange.")]
        public bool useCustomCenterOfMass;
        [Tooltip("Position of Center of Mass (if ticked)")]
        public Vector3 customCenterOfMass;
        [Tooltip("Used for balance adjustment on a weapon.\n \nUse this if swinging weapons are strange. Adjust the Capsule collider to the width of the weapon.")]
        public bool customInertiaTensor;
        [Tooltip("Collider of the Custom Inertia Tensor")]
        public CapsuleCollider customInertiaTensorCollider;
        [Tooltip("Allows a custom reference to be able to reference specific gameobjects and scripts in External code.")]
        public List<CustomReference> customReferences = new List<CustomReference>();
        [Tooltip("When ticked, item is automatically set as \"Thrown\" when spawned.")]
        public bool forceThrown;

        [Tooltip("Forces layer of mesh when an item is spawned.\n\n(Items will have their layer automatically applied when spawned, unless this is set)")]
        public int forceMeshLayer = -1;

        [NonSerialized]
        public List<Renderer> renderers = new List<Renderer>();
        [NonSerialized]
        public List<FxController> fxControllers = new List<FxController>();
        [NonSerialized]
        public List<FxModule> fxModules = new List<FxModule>();

        [NonSerialized]
        public List<RevealDecal> revealDecals = new List<RevealDecal>();
        [NonSerialized]
        public List<ColliderGroup> colliderGroups = new List<ColliderGroup>();
        [NonSerialized]
        public List<Collider> disabledColliders = new List<Collider>();
        [NonSerialized]
        public List<HingeEffect> effectHinges = new List<HingeEffect>();
        [NonSerialized]
        public List<WhooshPoint> whooshPoints = new List<WhooshPoint>();
        [NonSerialized]
        public LightVolumeReceiver lightVolumeReceiver;
        [NonSerialized]
        public List<CollisionHandler> collisionHandlers = new List<CollisionHandler>();
        [NonSerialized]
        public List<Handle> handles = new List<Handle>();
        [NonSerialized]
        public PhysicBody physicBody;
        [NonSerialized]
        public List<ParryTarget> parryTargets = new List<ParryTarget>();
        [NonSerialized]
        public Holder holder;
        [NonSerialized]
        ClothingGenderSwitcher clothingGenderSwitcher;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public ContentState contentState;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public List<ContentCustomData> contentCustomData;


        [Serializable]
        public class IconMarker
        {
            public string damagerId;
            public Vector2 position;
            public float directionAngle;
            public Damager.Direction direction;
            public IconMarker(string damagerId, Vector2 position, Damager.Direction direction, float directionAngle)
            {
                this.damagerId = damagerId;
                this.position = position;
                this.direction = direction;
                this.directionAngle = directionAngle;
            }
        }


        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif
            if (this.InPrefabScene()) return;
            if (!gameObject.activeInHierarchy) return;

            if (physicBody == null && GetComponent<Rigidbody>() == null && GetComponent<ArticulationBody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }

            if (!holderPoint) holderPoint = transform.Find("HolderPoint");
            if (!holderPoint)
            {
                holderPoint = new GameObject("HolderPoint").transform;
                holderPoint.SetParent(transform, false);
            }

            if (!parryPoint) parryPoint = transform.Find("ParryPoint");
            if (!parryPoint)
            {
                parryPoint = new GameObject("ParryPoint").transform;
                parryPoint.SetParent(transform, false);
            }

            if (!spawnPoint) spawnPoint = transform.Find("SpawnPoint");
            if (!spawnPoint)
            {
                spawnPoint = new GameObject("SpawnPoint").transform;
                spawnPoint.SetParent(transform, false);
            }

            preview = GetComponentInChildren<Preview>();
            if (!preview && transform.Find("Preview")) preview = transform.Find("Preview").gameObject.AddComponent<Preview>();
            if (!preview)
            {
                preview = new GameObject("Preview").AddComponent<Preview>();
                preview.transform.SetParent(transform, false);
            }

            Transform whoosh = transform.Find("Whoosh");
            if (whoosh && !whoosh.GetComponent<WhooshPoint>())
            {
                whoosh.gameObject.AddComponent<WhooshPoint>();
            }

            if (!mainHandleRight)
            {
                Handle[] children = GetComponentsInChildren<Handle>();
                for (int i = 0; i < children.Length; i++)
                {
                    Handle handle = children[i];
                    if (handle.IsAllowed(Side.Right))
                    {
                        mainHandleRight = handle;
                        break;
                    }
                }
            }

            if (!mainHandleLeft)
            {
                Handle[] children = GetComponentsInChildren<Handle>();
                for (int i = 0; i < children.Length; i++)
                {
                    Handle handle = children[i];
                    if (handle.IsAllowed(Side.Left))
                    {
                        mainHandleLeft = handle;
                        break;
                    }
                }
            }
            if (!mainHandleRight)
            {
                mainHandleRight = GetComponentInChildren<Handle>();
            }

            physicBody = this.gameObject.GetPhysicBody();
            if (useCustomCenterOfMass)
            {
                physicBody.centerOfMass = customCenterOfMass;
            }
            else
            {
                physicBody.ResetCenterOfMass();
            }

            if (customInertiaTensor)
            {
                if (customInertiaTensorCollider == null)
                {
                    customInertiaTensorCollider = new GameObject("InertiaTensorCollider").AddComponent<CapsuleCollider>();
                    customInertiaTensorCollider.transform.SetParent(transform, false);
                    customInertiaTensorCollider.radius = 0.05f;
                    customInertiaTensorCollider.direction = 2;
                }
                customInertiaTensorCollider.enabled = false;
                customInertiaTensorCollider.isTrigger = true;
                customInertiaTensorCollider.gameObject.layer = 2;
            }
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Vector3 upwards, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction, upwards) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction, upwards) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(gameObject.GetPhysicBodyInParent().centerOfMass), 0.01f);

            int count = additionalHolderPoints.Count;
            for (int i = 0; i < count; i++)
            {
                HolderPoint holderPoint = additionalHolderPoints[i];
                Gizmos.matrix = holderPoint.anchor.localToWorldMatrix;
                DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorName.Purple), 0.1f, 10);
                DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorName.Green), 0.05f);
            }

        }


        public void SwapWith(string itemID)
        {
        }


        [ContextMenu("Convert to spawner")]
        public void CreateItemSpawnerFromItem()
        {
            if (GetComponentInChildren<RopeSimple>() != null)
            {
                // don't change for spawner if it's linked to a rope
                return;
            }

            GameObject itemGameObject = gameObject;
            GameObject newItemSpawnerRoot = Instantiate(new GameObject("itemSpawnerRoot"), holderPoint.position, holderPoint.rotation, itemGameObject.transform.parent);

            ItemSpawner newItemSpawner = newItemSpawnerRoot.AddComponent<ItemSpawner>();
            newItemSpawner.name = itemId + "_Spawner";
            newItemSpawner.itemId = itemId;
            newItemSpawner.spawnOnStart = false;

            try
            {
                DestroyImmediate(itemGameObject);
            }
            catch (Exception)
            {
                DestroyImmediate(newItemSpawnerRoot);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // DESPAWN
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Keep these method signatures outside so modders can call these events using Unity events
        [Button]
        public void Despawn(float delay)
        {
        }

        [ContextMenu("Despawn")]
        [Button]
        public virtual void Despawn()
        {
        }

        [Serializable]
        public class HolderPoint
        {
            public HolderPoint(Transform t, string s)
            {
                anchor = t;
                anchorName = s;
            }

            public Transform anchor;
            public string anchorName;
        }

        public HolderPoint GetHolderPoint(string holderPoint)
        {
            HolderPoint hp = additionalHolderPoints.Find(x => x.anchorName == holderPoint);

            if (hp != null)
                return hp;
            if (!string.IsNullOrEmpty(holderPoint))
            {
                Debug.LogWarning("HolderPoint " + holderPoint + " not found on item " + name + " : returning default HolderPoint.");
            }

            return GetDefaultHolderPoint();
        }

        public HolderPoint GetDefaultHolderPoint()
        {
            return new HolderPoint(holderPoint, "Default");
        }

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [Button]
        public void AlignTransforms(Transform transformA, Transform transformB)
        {
            Transform childTransform = transformA.GetComponentInParent<Item>() == this ? transformA : transformB;
            Transform otherTransform = childTransform == transformA ? transformB : transformA;
            childTransform.SetPositionAndRotation(otherTransform.position, otherTransform.rotation);
        }
#endif
#endif

        /// <summary>
        /// Assign the position and rotation of the item to the spawning cached values.
        /// Stops the physic body from moving.
        /// </summary>
        public void ResetToSpawningTransformation()
        {
            
        }

    }


#if UNITY_EDITOR
    public class ItemPostprocessor : UnityEditor.AssetPostprocessor
    {
        private void OnPostprocessPrefab(GameObject go)
        {
            if (!go.TryGetComponent<Item>(out Item item))
            {
                return;
            }

            if (!item.spawnPoint) item.spawnPoint = item.transform.Find("SpawnPoint");
            if (!item.spawnPoint)
            {
                item.spawnPoint = new GameObject("SpawnPoint").transform;
                item.spawnPoint.SetParent(item.transform, false);
            }
        }
    }
#endif
}
