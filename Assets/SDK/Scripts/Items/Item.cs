using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad.Skill;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/Item.html")]
    [AddComponentMenu("ThunderRoad/Items/Item")]
    public class Item : ThunderEntity
    {
        public static List<Item> all = new List<Item>();
        public static List<Item> allActive = new List<Item>();
        public static List<Item> allThrowed = new List<Item>();
        public static HashSet<Item> allMoving = new HashSet<Item>();
        public static List<Item> allTk = new List<Item>();
        public static List<Item> allWorldAttached = new List<Item>();
        public static List<ItemContent> potentialLostItems = new List<ItemContent>(); //items that were destoyed but were not despawned to be returned to player stash if needed

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }

        [ValueDropdown(nameof(GetAllItemID), AppendNextDrawer = true)]
#endif
        [Tooltip("The Item ID of the item specified in the Catalog")]
        public string itemId;
        [Tooltip("Specifies the Holder Point of the item. This specifies the position and rotation of the item when held in a holder, such as on player hips and back. The Z axis/blue arrow specifies towards the floor.")]
#if ODIN_INSPECTOR        
        [InlineButton(nameof(SetHolderPointToCenterOfMass), "To COM")]
#endif        
        public Transform holderPoint;
        [Tooltip("Specifies the spawn point of the item. This specifies the position and rotation of the item when spawned, it's mostly used for itemSpawner and spawning the item via an item book")]
        public Transform spawnPoint;
        [Tooltip("Can add additional holderpoints for different interactables.\n \nFor Items on the Item Rack, the anchor must be named HolderRackTopAnchor, or alternatively for the bow rack, HolderRackTopAnchorBow, and HolderRackSideAnchor for Shield rack")]
        public List<HolderPoint> additionalHolderPoints = new List<HolderPoint>();
        [Tooltip("Shows the point that AI will try to block with when they are holding the item.")]
        public Transform parryPoint;
        [Tooltip("Specifies what handle is grabbed by default for the Right Hand")]
        public Handle mainHandleRight;
        [Tooltip("Specifies what handle is grabbed by default for the Left Hand")]
        public Handle mainHandleLeft;
        [Tooltip("Used to point in direction when thrown.\nZ-Axis/Blue Arrow points forwards.")]
        public Transform flyDirRef;
        [Tooltip("States the Preview for the item.")]
        public Preview preview;
        [Tooltip("Tick if the item is attached to the world and not spawned via item spawner or item book.")]
        public bool worldAttached;
        [Tooltip("Tick if the item should keep its parent when it loads into the scene.")]
        public bool keepParent;
        [Tooltip("Radius to depict how close this item needs to be to a creature before the creatures' collision is enabled.")]
        public float creaturePhysicToggleRadius = 2;
        [Tooltip("Allows user to adjust the center of mass on an object.\nIf unticked, this is automatically adjusted. When ticked, adds a custom gizmo to adjust.\n \nUse this if weight on the item is acting strange.")]
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

        [Tooltip("Determines the owner of the item. This is generally set at runtime to work with the shop and buying items.")]
        public Owner owner { get { return _owner; } }
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private Owner _owner;


        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public bool isUsed = true;

        public enum Owner
        {
            None,
            Player,
            Shopkeeper,
        }

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
        [HideInInspector]
        public LightVolumeReceiver lightVolumeReceiver;
        public AudioSource audioSource;
        [NonSerialized]
        public List<CollisionHandler> collisionHandlers = new List<CollisionHandler>();
        [NonSerialized]
        public List<Handle> handles = new List<Handle>();
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public PhysicBody physicBody;
        [NonSerialized]
        public Breakable breakable;
        [NonSerialized]
        public List<ParryTarget> parryTargets = new List<ParryTarget>();
        [NonSerialized]
        public Holder holder;
        [NonSerialized]
        ClothingGenderSwitcher clothingGenderSwitcher;

        [NonSerialized]
        public bool allowGrip = true;
        [NonSerialized]
        public bool hasSlash;
        [NonSerialized]
        public bool hasMetal;
        [NonSerialized]
        public List<ColliderGroup> metalColliderGroups;

        [NonSerialized]
        public FloatHandler sliceAngleMultiplier;
        public FloatHandler damageMultiplier;
        public IntAddHandler pushLevelMultiplier;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public Container linkedContainer;
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

        public void SetHolderPointToCenterOfMass()
        {
            holderPoint.transform.position = this.transform.TransformPoint(physicBody ? physicBody.centerOfMass : this.GetComponent<Rigidbody>().centerOfMass);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }


        [Button]
        public void AddNonStorableModifierInvokable(UnityEngine.Object handler)
        {
        }

        [Button]
        public void RemoveNonStorableModifierInvokable(UnityEngine.Object handler)
        {
        }

        [Button]
        public void ClearNonStorableModifiers()
        {
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying)
                return;
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;

            if (!this.InPrefabScene())
                return;
#endif            
            if (!gameObject.activeInHierarchy)
                return;
            SetupDefaultComponents();
        }


        [Button]
        public void SetupDefaultComponents()
        {
            if (physicBody == null && GetComponent<Rigidbody>() == null && GetComponent<ArticulationBody>() == null)
            {
                Debug.LogWarning($"[DefaultComponents] Adding Rigidbody to {this.itemId}");
                gameObject.AddComponent<Rigidbody>();
            }

            if (!holderPoint)
            {
                holderPoint = transform.Find("HolderPoint");
            }
            if (!holderPoint)
            {
                Debug.LogWarning($"[DefaultComponents] Adding HolderPoint to {this.itemId}");
                holderPoint = new GameObject("HolderPoint").transform;
                holderPoint.SetParent(transform, false);
            }

            if (!parryPoint)
                parryPoint = transform.Find("ParryPoint");
            if (!parryPoint)
            {
                Debug.LogWarning($"[DefaultComponents] Adding ParryPoint to {this.itemId}");
                parryPoint = new GameObject("ParryPoint").transform;
                parryPoint.SetParent(transform, false);
            }

            if (!spawnPoint)
                spawnPoint = transform.Find("SpawnPoint");
            if (!spawnPoint)
            {
                Debug.LogWarning($"[DefaultComponents] Adding SpawnPoint to {this.itemId}");
                spawnPoint = new GameObject("SpawnPoint").transform;
                spawnPoint.SetParent(transform, false);
            }

            preview = GetComponentInChildren<Preview>();
            if (!preview && transform.Find("Preview"))
                preview = transform.Find("Preview").gameObject.AddComponent<Preview>();
            if (!preview)
            {
                Debug.LogWarning($"[DefaultComponents] Adding Preview to {this.itemId}");
                preview = new GameObject("Preview").AddComponent<Preview>();
                preview.transform.SetParent(transform, false);
            }

            Transform whoosh = transform.Find("Whoosh");
            if (whoosh && !whoosh.GetComponent<WhooshPoint>())
            {
                Debug.LogWarning($"[DefaultComponents] Adding WhooshPoint to {this.itemId}");
                whoosh.gameObject.AddComponent<WhooshPoint>();
            }

            if (!mainHandleRight)
            {
                Debug.LogWarning($"[DefaultComponents] Adding Handle Right to {this.itemId}");
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
                Debug.LogWarning($"[DefaultComponents] Adding Handle Right to {this.itemId}");
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
            if (!this.TryGetComponent<LightVolumeReceiver>(out lightVolumeReceiver))
                lightVolumeReceiver = this.gameObject.AddComponent<LightVolumeReceiver>();
            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
#if UNITY_EDITOR
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
            PhysicBody physicBodyInParent = gameObject.GetPhysicBodyInParent();
            if (physicBodyInParent != null)
                Gizmos.DrawWireSphere(transform.TransformPoint(physicBodyInParent.centerOfMass), 0.01f);


            if (!additionalHolderPoints.IsNullOrEmpty())
            {
                int count = additionalHolderPoints.Count;
                for (int i = 0; i < count; i++)
                {
                    HolderPoint holderPoint = additionalHolderPoints[i];
                    Gizmos.matrix = holderPoint.anchor.localToWorldMatrix;
                    DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorName.Purple), 0.1f, 10);
                    DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorName.Green), 0.05f);
                }
            }

        }
#endif


        [Button]
        public void SwapWith(string itemID)
        {
        }

        [Button]
        public void ForceUngrabAll()
        {
        }


        public void SetValue(float value)
        {
        }

        public void SetOwner(Owner owner)
        {
        }

        public void ClearValueOverride()
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


        public void OnSpawn(List<ContentCustomData> contentCustomDataList, Owner owner)
        {
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
        public override void Despawn()
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


        public void ReturnToPool()
        {
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

        public ItemData.CustomSnap GetCustomSnap(string holderName)
        {
            return null;
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

        public void InvokeOnImbuesChange(SpellData spellData, float amount, float change, EventTime time)
        {
        }
    }


#if UNITY_EDITOR

    public class ItemPostprocessor : UnityEditor.AssetPostprocessor
    {
        private void OnPostprocessPrefab(GameObject gameObject)
        {


            if (!gameObject.TryGetComponent<Item>(out Item item))
                return;

            Debug.Log("AP: ItemPostprocessor: OnPostprocessPrefab: " + gameObject.name);

            if (!item.spawnPoint)
                item.spawnPoint = item.transform.Find("SpawnPoint");
            if (!item.spawnPoint)
            {
                item.spawnPoint = new GameObject("SpawnPoint").transform;
                item.spawnPoint.SetParent(item.transform, false);
            }
        }
    }
#endif
}
