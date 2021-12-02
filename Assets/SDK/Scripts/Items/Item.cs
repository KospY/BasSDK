using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Profiling;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Items/Item")]
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        public static List<Item> all = new List<Item>();
        public static List<Item> allActive = new List<Item>();
        public static List<Item> allThrowed = new List<Item>();
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
        public List<CustomReference> customReferences;

        [NonSerialized]
        public List<Renderer> renderers;
        [NonSerialized]
        public List<RevealDecal> revealDecals;
        [NonSerialized]
        public List<ColliderGroup> colliderGroups;
        [NonSerialized]
        public List<HingeEffect> effectHinges;
        [NonSerialized]
        public List<WhooshPoint> whooshPoints;
        [NonSerialized]
        public LightVolumeReceiver lightVolumeReceiver;
        [NonSerialized]
        public List<CollisionHandler> collisionHandlers = new List<CollisionHandler>();
        [NonSerialized]
        public List<Handle> handles;
        [NonSerialized]
        public Rigidbody rb;
        [NonSerialized]
        public List<ParryTarget> parryTargets;
        [NonSerialized]
        public Holder holder;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public List<SavedValue> savedValues;

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

        [Serializable]
        public class SavedValue
        {
            public string id;
            public string value;
            public SavedValue(string id, string value)
            {
                this.id = id;
                this.value = value;
            }

            public SavedValue Clone()
            {
                return MemberwiseClone() as SavedValue;
            }
        }


        public Transform GetCustomReference(string name)
        {
            CustomReference customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                return customReference.transform;
            }
            else
            {
                Debug.LogError("[" + itemId + "] Cannot find item custom reference " + name);
                return null;
            }
        }

        protected virtual void OnValidate()
        {
            IconManager.SetIcon(this.gameObject, null);

            if (!this.gameObject.activeInHierarchy) return;

            Transform holderPoint = null;

            holderPoint = this.transform.Find("HolderPoint");

            if (!holderPoint)
            {
                holderPoint = new GameObject("HolderPoint").transform;
                holderPoint.SetParent(this.transform, false);
            }
            parryPoint = this.transform.Find("ParryPoint");
            if (!parryPoint)
            {
                parryPoint = new GameObject("ParryPoint").transform;
                parryPoint.SetParent(this.transform, false);
            }
            preview = this.GetComponentInChildren<Preview>();
            if (!preview && this.transform.Find("Preview")) preview = this.transform.Find("Preview").gameObject.AddComponent<Preview>();
            if (!preview)
            {
                preview = new GameObject("Preview").AddComponent<Preview>();
                preview.transform.SetParent(this.transform, false);
            }
            Transform whoosh = this.transform.Find("Whoosh");
            if (whoosh && !whoosh.GetComponent<WhooshPoint>())
            {
                whoosh.gameObject.AddComponent<WhooshPoint>();
            }

            if (!mainHandleRight)
            {
                foreach (Handle handle in this.GetComponentsInChildren<Handle>())
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
                foreach (Handle handle in this.GetComponentsInChildren<Handle>())
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
                mainHandleRight = this.GetComponentInChildren<Handle>();
            }
            if (useCustomCenterOfMass)
            {
                this.GetComponent<Rigidbody>().centerOfMass = customCenterOfMass;
            }
            else
            {
                this.GetComponent<Rigidbody>().ResetCenterOfMass();
            }
            if (customInertiaTensor)
            {
                if (customInertiaTensorCollider == null)
                {
                    customInertiaTensorCollider = new GameObject("InertiaTensorCollider").AddComponent<CapsuleCollider>();
                    customInertiaTensorCollider.transform.SetParent(this.transform, false);
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
            Gizmos.DrawWireSphere(this.transform.TransformPoint(this.GetComponent<Rigidbody>().centerOfMass), 0.01f);
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
            renderers = new List<Renderer>();
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                if (!renderer.enabled || (!(renderer is SkinnedMeshRenderer) && !(renderer is MeshRenderer))) continue;
                renderers.Add(renderer);
            }

#if PrivateSDK
            lightVolumeReceiver = this.GetComponent<LightVolumeReceiver>();
            if (!lightVolumeReceiver) lightVolumeReceiver = this.gameObject.AddComponent<LightVolumeReceiver>();
            lightVolumeReceiver.initRenderersOnStart = false;
            lightVolumeReceiver.SetRenderers(renderers);
#endif

            revealDecals = new List<RevealDecal>(this.GetComponentsInChildren<RevealDecal>());
            colliderGroups = new List<ColliderGroup>(this.GetComponentsInChildren<ColliderGroup>());
            whooshPoints = new List<WhooshPoint>(this.GetComponentsInChildren<WhooshPoint>());
            effectHinges = new List<HingeEffect>(this.GetComponentsInChildren<HingeEffect>());
            handles = new List<Handle>(this.GetComponentsInChildren<Handle>());
            if (mainHandleRight) mainHandleRight = mainHandleRight.GetComponent<Handle>();
            if (mainHandleLeft) mainHandleLeft = mainHandleLeft.GetComponent<Handle>();
            parryTargets = new List<ParryTarget>(this.GetComponentsInChildren<ParryTarget>());

            // Rigidbody
            rb = this.GetComponent<Rigidbody>();
            if (useCustomCenterOfMass) rb.centerOfMass = customCenterOfMass;

            collisionHandlers = new List<CollisionHandler>(this.GetComponentsInChildren<CollisionHandler>());
            if (collisionHandlers.Count == 0)
            {
                collisionHandlers.Add(this.gameObject.AddComponent<CollisionHandler>());
                foreach (ColliderGroup colliderGroup in colliderGroups)
                {
                    colliderGroup.collisionHandler = colliderGroup.GetComponentInParent<CollisionHandler>();
                }
            }

        }

#if PrivateSDK

        protected virtual void OnEnable()
        {
            allActive.Add(this);
            if (Level.current && Level.current.dungeon)
            {
                cullingDetectionEnabled = true;
            }
        }

        protected virtual void OnDisable()
        {
            allActive.Remove(this);
            cullingDetectionEnabled = false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ROOM CULLING
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [NonSerialized, ShowInInspector, ReadOnly]
        public Room currentRoom;
        [NonSerialized, ShowInInspector, ReadOnly]
        public bool isCulled;

        protected bool cullingDetectionEnabled;
        protected float cullingDetectionCycleSpeed = 1;
        protected float cullingDetectionCycleTime;

        private void LateUpdate()
        {
            if (!cullingDetectionEnabled) return;
            if (!Level.current.dungeon.initialized) return;
            if ((Time.time - cullingDetectionCycleTime) < cullingDetectionCycleSpeed) return;
            cullingDetectionCycleTime = Time.time;

            //if (rb.IsSleeping()) return;

            if (currentRoom == null)
            {
                Room roomFound = Level.current.dungeon.SearchRoomFromPosition(this.transform.position);
                if (currentRoom != roomFound)
                {
                    currentRoom = roomFound;
                    currentRoom.RegisterItem(this);
                    SetCull(currentRoom.isCulled);
                }
            }
            else if (!currentRoom.tile.Bounds.Contains(this.transform.position))
            {
                Room roomFound = Level.current.dungeon.SearchRoomFromPosition(this.transform.position, currentRoom);
                if (currentRoom != roomFound)
                {
                    currentRoom.UnRegisterItem(this);
                    currentRoom = roomFound;
                    currentRoom.RegisterItem(this);
                    SetCull(currentRoom.isCulled);
                }
            }
        }

        public void SetCull(bool cull)
        {
            if (isCulled == cull) return;

            this.gameObject.SetActive(!cull);
            isCulled = cull;
        }

#endif


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
            else if (!string.IsNullOrEmpty(holderPoint))
            {
                Debug.LogWarning("HolderPoint " + holderPoint + " not found on item " + name + " : returning default HolderPoint.");
            }

            return GetDefaultHolderPoint();
        }
    }
}
