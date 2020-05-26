using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemDefinition : MonoBehaviour
    {
#if ProjectCore
        [ValueDropdown("GetAllItemID")]
#endif
        public string itemId;
        public Transform holderPoint;
        public Transform parryPoint;
        public HandleDefinition mainHandleRight;
        public HandleDefinition mainHandleLeft;
        public Transform flyDirRef;
        public Preview preview;
        public bool useCustomCenterOfMass;
        public Vector3 customCenterOfMass;
        public bool customInertiaTensor;
        public CapsuleCollider customInertiaTensorCollider;
        public List<CustomReference> customReferences;

        public bool initialized { get; protected set; }

        [NonSerialized]
        public List<Renderer> renderers;
        [NonSerialized]
        public List<Paintable> paintables;
        [NonSerialized]
        public List<ColliderGroup> colliderGroups;
        [NonSerialized]
        public List<EffectHinge> effectHinges;
        [NonSerialized]
        public List<WhooshPoint> whooshPoints;

        [NonSerialized, ShowInInspector]
        public List<SavedValue> savedValues;

        [Serializable]
        public class SavedValue
        {
            [ValueDropdown("GetSavedValuesID")]
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

            public List<ValueDropdownItem<string>> GetSavedValuesID()
            {
                List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
                foreach (SavedValueID savedValueID in Enum.GetValues(typeof(SavedValueID)))
                {
                    dropdownList.Add(new ValueDropdownItem<string>(savedValueID.ToString(), savedValueID.ToString()));
                }
                return dropdownList;
            }
        }

#if ProjectCore
        [NonSerialized]
        public List<CollisionHandler> collisionHandlers;
        [NonSerialized, ShowInInspector]
        public Vector3 customInertiaTensorPos;
        [NonSerialized, ShowInInspector]
        public Quaternion customInertiaTensorRot;

        public void SetSavedValue(string id, string value)
        {
            if (savedValues == null) savedValues = new List<SavedValue>();
            bool foundId = false;
            for (int i = savedValues.Count - 1; i >= 0; i--)
            {
                if (savedValues[i].id == id)
                {
                    if (value == null || value == "")
                    {
                        savedValues.RemoveAt(i);
                    }
                    else
                    {
                        savedValues[i].value = value;
                    }
                    foundId = true;
                }
            }
            if (!foundId && value != null && value != "")
            {
                savedValues.Add(new SavedValue(id, value));
            }
        }

        public bool TryGetSavedValue(string id, out string value)
        {
            if (savedValues != null)
            {
                for (int i = savedValues.Count - 1; i >= 0; i--)
                {
                    if (savedValues[i].id == id)
                    {
                        value = savedValues[i].value;
                        return true;
                    }
                }
            }
            value = null;
            return false;
        }

        public delegate void InitializedDelegate(Item item);
        public event InitializedDelegate Initialized;

        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Item);
        }
#endif

        public Transform GetCustomReference(string name)
        {
            CustomReference customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                return customReference.transform;
            }
            else
            {
                Debug.LogError("[" + itemId + "] Cannot find item definition custom reference " + name);
                return null;
            }
        }

        protected virtual void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
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
                foreach (HandleDefinition handleDefinition in this.GetComponentsInChildren<HandleDefinition>())
                {
                    if (handleDefinition.IsAllowed(Side.Right))
                    {
                        mainHandleRight = handleDefinition;
                        break;
                    }
                }
            }
            if (!mainHandleLeft)
            {
                foreach (HandleDefinition handleDefinition in this.GetComponentsInChildren<HandleDefinition>())
                {
                    if (handleDefinition.IsAllowed(Side.Left))
                    {
                        mainHandleLeft = handleDefinition;
                        break;
                    }
                }
            }
            if (!mainHandleRight)
            {
                mainHandleRight = this.GetComponentInChildren<HandleDefinition>();
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

#if ProjectCore
        protected virtual void Awake()
        {
            renderers = new List<Renderer>();
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                if (!(renderer is SkinnedMeshRenderer) && !(renderer is MeshRenderer)) continue;
                renderers.Add(renderer);
            }
            paintables = new List<Paintable>(this.GetComponentsInChildren<Paintable>());
            colliderGroups = new List<ColliderGroup>(this.GetComponentsInChildren<ColliderGroup>());
            whooshPoints = new List<WhooshPoint>(this.GetComponentsInChildren<WhooshPoint>());
            effectHinges = new List<EffectHinge>(this.GetComponentsInChildren<EffectHinge>());
            collisionHandlers = new List<CollisionHandler>(this.GetComponentsInChildren<CollisionHandler>());
            if (collisionHandlers.Count == 0) collisionHandlers.Add(this.gameObject.AddComponent<CollisionHandler>());
            if (customInertiaTensorCollider) CalculateCustomInertiaTensor();
        }

        protected virtual void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            if (!initialized && itemId != null && itemId != "" && itemId != "None")
            {
                Init(Catalog.GetData<ItemPhysic>(itemId));
            }
        }

        public virtual Item Init(ItemPhysic itemData)
        {
            foreach (Item existingInteractiveObject in this.gameObject.GetComponents<Item>())
            {
                Destroy(existingInteractiveObject);
            }
            itemId = itemData.id;
            initialized = true;
            Item item = itemData.CreateComponent(this.gameObject);
            if (Initialized != null) Initialized.Invoke(item);
            return item;
        }

        [Button]
        public void SetCustomInertiaTensor()
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.inertiaTensor = customInertiaTensorPos;
            rb.inertiaTensorRotation = customInertiaTensorRot;
        }

        [Button]
        public virtual void ResetInertiaTensor()
        {
            this.GetComponent<Rigidbody>().ResetInertiaTensor();
        }

        [Button]
        public void CalculateCustomInertiaTensor()
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            if (!customInertiaTensorCollider)
            {
                Debug.LogWarning("Cannot calculate custom inertia tensor because no custom collider has been set on " + itemId);
                rb.ResetInertiaTensor();
                return;
            }
            List<Collider> orgColliders = new List<Collider>();
            foreach (Collider collider in rb.GetComponentsInChildren<Collider>())
            {
                if (collider.isTrigger || customInertiaTensorCollider == collider) continue;
                collider.enabled = false;
                orgColliders.Add(collider);
            }
            customInertiaTensorCollider.enabled = true;
            customInertiaTensorCollider.isTrigger = false;
            rb.ResetInertiaTensor();

            customInertiaTensorPos = rb.inertiaTensor;
            customInertiaTensorRot = rb.inertiaTensorRotation;

            customInertiaTensorCollider.isTrigger = true;
            customInertiaTensorCollider.enabled = false;
            foreach (Collider collider in orgColliders)
            {
                collider.enabled = true;
            }
        }
#endif

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
            Gizmos.matrix = holderPoint.transform.localToWorldMatrix;
            DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorName.Purple), 0.1f, 10);
            DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorName.Green), 0.05f);
        }
    }
}
