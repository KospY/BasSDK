using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if FULLGAME
using Sirenix.OdinInspector;
#endif

namespace BS
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemDefinition : MonoBehaviour
    {
#if FULLGAME
        [ValueDropdown("GetAllItemID")]
#endif
        public string itemId;
        public Transform holderPoint;
        public Transform parryPoint;
        public List<Renderer> renderers = new List<Renderer>();
        public HandleDefinition mainHandleRight;
        public HandleDefinition mainHandleLeft;
        public Transform flyDirRef;
        public bool customCenterOfMass;
        public Vector3 centerOfMass;
        public List<ColliderGroup> colliderGroups;
        public List<Transform> whooshPoints;
        public float previewSize = 1;
        public Transform preview;
        public List<CustomReference> customReferences;
        public bool initialized { get; protected set; }
#if FULLGAME
        public delegate void InitializedDelegate(Item interactiveObject);
        public event InitializedDelegate Initialized;
#endif
        [Serializable]
        public class ColliderGroup
        {
            public string name;
            public bool imbueMagic;
            public bool checkIndependently;
            public PhysicMaterial ColliderMaterial;
            public List<Collider> colliders;
            public ColliderGroup(string name)
            {
                this.name = name;
                this.colliders = new List<Collider>();
            }
        }

        [Serializable]
        public class CustomReference
        {
            public string name;
            public Transform transform;
        }

#if FULLGAME
        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.current.GetDropdownAllID(Catalog.Category.Item);
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
            foreach (ColliderGroup colG in colliderGroups)
            {
                foreach (Collider col in colG.colliders)
                {
                    if (colG.ColliderMaterial)
                    {
                        col.material = colG.ColliderMaterial;
                    }
                }
            }
            if (customCenterOfMass)
            {
                this.GetComponent<Rigidbody>().centerOfMass = centerOfMass / 10;
            }
            else
            {
                centerOfMass = GetComponent<Rigidbody>().centerOfMass * 10;
            }
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
            if (!preview)
            {
                if (this.transform.Find("Preview")) { 
                preview = this.transform.Find("Preview");
                } else { 
                preview = new GameObject("Preview").transform;
                preview.SetParent(this.transform, false);
                }
            }
            else if (!preview.GetComponent<Preview>() && preview != this.transform)
                {
                    preview.gameObject.AddComponent(typeof(Preview));
                }

            if (renderers == null || renderers.Count == 0) renderers = new List<Renderer>(this.GetComponentsInChildren<Renderer>());

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

            if (!mainHandleRight) mainHandleRight = this.GetComponentInChildren<HandleDefinition>();
            if (colliderGroups == null || colliderGroups.Count == 0)
            {
                colliderGroups = new List<ColliderGroup>();
                ColliderGroup colliderGroup = new ColliderGroup("Default");
                colliderGroup.colliders = new List<Collider>(this.GetComponentsInChildren<Collider>().Where(c => !c.isTrigger));
                colliderGroups.Add(colliderGroup);
            }
            if (whooshPoints == null)
            {
                whooshPoints = new List<Transform>();
                Transform whooshPoint = this.transform.Find("Whoosh");
                if (!whooshPoint)
                {
                    whooshPoint = new GameObject("Whoosh").transform;
                    whooshPoint.SetParent(this.transform, false);
                }
                whooshPoints.Add(whooshPoint);
            }
        }

#if FULLGAME
        protected virtual void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            if (!initialized && itemId != null && itemId != "" && itemId != "None")
            {
                Init(Catalog.current.GetData<ItemData>(itemId));
            }
        }

        public virtual Item Init(ItemData item)
        { 
            foreach (Item existingInteractiveObject in this.gameObject.GetComponents<Item>())
            {
                Destroy(existingInteractiveObject);
            }
            itemId = item.id;
            initialized = true;
            Item interactiveObject = item.CreateComponent(this.gameObject);
            if (Initialized != null) Initialized.Invoke(interactiveObject);
            return interactiveObject;
        }
#endif

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.TransformPoint(this.GetComponent<Rigidbody>().centerOfMass), 0.01f);
            Gizmos.matrix = holderPoint.transform.localToWorldMatrix;
            Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Common.HueColourValue(HueColorNames.Purple), 0.1f, 10);
            Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Common.HueColourValue(HueColorNames.Green), 0.05f);
            Gizmos.matrix = preview.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(previewSize, previewSize, 0));
        }
    }
}
