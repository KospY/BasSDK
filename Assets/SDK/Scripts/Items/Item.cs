using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Item")]
    [AddComponentMenu("ThunderRoad/Items/Item")]
    [RequireComponent(typeof(Rigidbody))]
    public class Item : ThunderBehaviour
    {
        public static List<Item> all = new List<Item>();
        public static List<Item> allActive = new List<Item>();
        public static List<Item> allThrowed = new List<Item>();
        public static HashSet<Item> allMoving = new HashSet<Item>();
        public static List<Item> allTk = new List<Item>();
        public static List<Item> allHanging = new List<Item>();

        public string itemId;
        public Transform holderPoint;
        public List<HolderPoint> additionalHolderPoints = new List<HolderPoint>();
        public Transform parryPoint;
        public Handle mainHandleRight;
        public Handle mainHandleLeft;
        public Transform flyDirRef;
        public Preview preview;
        public bool disallowRoomDespawn;
        public bool hanging;
        public float creaturePhysicToggleRadius = 2;
        public bool useCustomCenterOfMass;
        public Vector3 customCenterOfMass;
        public bool customInertiaTensor;
        public CapsuleCollider customInertiaTensorCollider;
        public List<CustomReference> customReferences = new List<CustomReference>();
        public bool forceThrown;

        public int forceMeshLayer = -1;

        [NonSerialized]
        public List<Renderer> renderers = new List<Renderer>();
        [NonSerialized]
        public List<RevealDecal> revealDecals = new List<RevealDecal>();
        [NonSerialized]
        public List<ColliderGroup> colliderGroups = new List<ColliderGroup>();
        [NonSerialized]
        public (Collider, bool)[] allColliders;
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
        public Rigidbody rb;
        [NonSerialized]
        public List<ParryTarget> parryTargets = new List<ParryTarget>();
        [NonSerialized]
        public Holder holder;
        [NonSerialized]
        ClothingGenderSwitcher clothingGenderSwitcher;


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


        public Bounds GetLocalBounds()
        {
            var b = new Bounds(Vector3.zero, Vector3.zero);
            RecurseEncapsulate(transform, ref b);
            return b;

            void RecurseEncapsulate(Transform child, ref Bounds bounds)
            {
                if (child.TryGetComponent(out MeshFilter mesh) && mesh?.sharedMesh != null)
                {
                    var lsBounds = mesh.sharedMesh.bounds;
                    var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents);
                    var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents);
                    bounds.Encapsulate(transform.InverseTransformPoint(wsMin));
                    bounds.Encapsulate(transform.InverseTransformPoint(wsMax));
                }
                else if (child.TryGetComponent(out SkinnedMeshRenderer smr))
                {
                    var lsBounds = smr.localBounds;
                    var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents);
                    var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents);
                    bounds.Encapsulate(transform.InverseTransformPoint(wsMin));
                    bounds.Encapsulate(transform.InverseTransformPoint(wsMax));
                }
                else
                {
                    bounds.Encapsulate(transform.InverseTransformPoint(child.transform.position));
                }

                foreach (Transform grandChild in child.transform)
                {
                    RecurseEncapsulate(grandChild, ref bounds);
                }
            }
        }

        public Vector3 GetLocalCenter()
        {
            return Vector3.zero;
        }

        public void Haptic(float intensity)
        {
        }

        public bool TryGetCustomReference<T>(string name, out T custom) where T : Component
        {
            custom = GetCustomReference<T>(name, false);
            return custom != null;
        }

        public T GetCustomReference<T>(string name, bool printError = true) where T : Component
        {
            CustomReference customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                if (customReference.transform is T) return (T)customReference.transform;
                if (typeof(T) == typeof(Transform)) return customReference.transform.transform as T;
                return customReference.transform.GetComponent<T>();
            }

            if (printError) Debug.LogError("[" + itemId + "] Cannot find item custom reference " + name);
            return null;
        }

        public Transform GetCustomReference(string name, bool printError = true) => GetCustomReference<Transform>(name, printError);

        protected virtual void OnValidate()
        {
            IconManager.SetIcon(gameObject, null);

            if (!gameObject.activeInHierarchy) return;

            //Transform holderPoint = null;

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
                foreach (Handle handle in GetComponentsInChildren<Handle>())
                {
                    if (handle.IsAllowed(Side.Right))
                    {
                        mainHandleRight = handle;
                        break;
                    }
                }
            }
            if (!mainHandleLeft)
            {
                foreach (Handle handle in GetComponentsInChildren<Handle>())
                {
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
            if (useCustomCenterOfMass)
            {
                GetComponent<Rigidbody>().centerOfMass = customCenterOfMass;
            }
            else
            {
                GetComponent<Rigidbody>().ResetCenterOfMass();
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
            Gizmos.DrawWireSphere(transform.TransformPoint(GetComponent<Rigidbody>().centerOfMass), 0.01f);

            foreach (HolderPoint holderPoint in additionalHolderPoints)
            {
                Gizmos.matrix = holderPoint.anchor.localToWorldMatrix;
                DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorName.Purple), 0.1f, 10);
                DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorName.Green), 0.05f);
            }

        }

        protected virtual void Awake()
        {
            all.Add(this);

            clothingGenderSwitcher = GetComponentInChildren<ClothingGenderSwitcher>();


            renderers = new List<Renderer>();
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                if (!renderer.enabled || (!(renderer is SkinnedMeshRenderer) && !(renderer is MeshRenderer))) continue;
                renderers.Add(renderer);
            }

            revealDecals = new List<RevealDecal>(this.GetComponentsInChildren<RevealDecal>());
            colliderGroups = new List<ColliderGroup>(this.GetComponentsInChildren<ColliderGroup>());
            whooshPoints = new List<WhooshPoint>(this.GetComponentsInChildren<WhooshPoint>());
            effectHinges = new List<HingeEffect>(this.GetComponentsInChildren<HingeEffect>());
            handles = new List<Handle>(this.GetComponentsInChildren<Handle>());
            if (mainHandleRight) mainHandleRight = mainHandleRight.GetComponent<Handle>();
            if (mainHandleLeft) mainHandleLeft = mainHandleLeft.GetComponent<Handle>();
            parryTargets = new List<ParryTarget>(GetComponentsInChildren<ParryTarget>());

            // Rigidbody
            rb = GetComponent<Rigidbody>();
            if (useCustomCenterOfMass) rb.centerOfMass = customCenterOfMass;

            collisionHandlers = new List<CollisionHandler>(GetComponentsInChildren<CollisionHandler>());
            if (collisionHandlers.Count == 0)
            {
                collisionHandlers.Add(gameObject.AddComponent<CollisionHandler>());
                foreach (ColliderGroup colliderGroup in colliderGroups)
                {
                    colliderGroup.collisionHandler = colliderGroup.GetComponentInParent<CollisionHandler>();
                }
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

        public HolderPoint GetDefaultHolderPoint()
        {
            return new HolderPoint(holderPoint, "Default");
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
    }
}