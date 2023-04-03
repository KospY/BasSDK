using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class ItemData : CatalogData
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Split", marginRight: 5, LabelWidth = 200, Width = 700)]
        [VerticalGroup("Split/Left")]
        [BoxGroup("Split/Left/General")]
#endif
        public string localizationId;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public string displayName;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General"), TextArea(4, 14)] 
#endif
        public string description;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public string author = "Unknown";
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General"), CustomContextMenu("Set all purchaseable prices same", "SetAllPricesToMatch")] 
#endif
        public float price;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public bool purchasable;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public int tier;
        [NonSerialized]
        public string tierString;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public string weight;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public string size;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public int levelRequired;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General"), ValueDropdown("GetCategories", IsUniqueList = true)] 
#endif
        public string category;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General"), ValueDropdown("GetAllEffectID")] 
#endif
        public string iconEffectId;
        [NonSerialized]
        public EffectData iconEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/General")] 
#endif
        public CenterType preferredItemCenter = CenterType.Mass;

        // PREFAB
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab")] 
#endif
        public string prefabAddress;
        [NonSerialized]
        public IResourceLocation prefabLocation;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab"), ShowInInspector] 
#endif
        public string iconAddress;
        [NonSerialized]
        public IResourceLocation iconLocation;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab"), ShowInInspector] 
#endif
        public string closeUpIconAddress;
        [NonSerialized]
        public IResourceLocation closeUpIconLocation;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [HorizontalGroup("Split/Left/Prefab/horiz"), LabelText("Prefab"), NonSerialized, ShowInInspector, DisableInPlayMode, OnValueChanged("EditorUpdatePrefabAddress"), PreviewField] 
#endif
        public GameObject editorPrefab;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab"), NonSerialized, ShowInInspector, HorizontalGroup("Split/Left/Prefab/horiz"), OnValueChanged("EditorUpdateIconAddress"), PreviewField] 
#endif
        public Texture icon;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab"), NonSerialized, ShowInInspector, HorizontalGroup("Split/Left/Prefab/horiz"), OnValueChanged("EditorUpdateCloseUpIconAddress"), PreviewField] 
#endif
        public Texture closeUpIcon;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab")] 
#endif
        public int pooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab")] 
#endif
        public int androidPooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab")] 
#endif
        public Type type = Type.Misc;

        public enum Type
        {
            Misc,
            Weapon,
            Quiver,
            Potion,
            Prop,
            Body,
            Shield,
            Wardrobe,
            Spell,
            Crystal,
            Valuable,
            Tool,
            Food,
        }

        static int spawningAsyncCount;

#if UNITY_EDITOR
        protected void EditorUpdatePrefabAddress()
        {
            if (editorPrefab) prefabAddress = Catalog.GetAddressFromPrefab(editorPrefab);
        }
        protected void EditorUpdateIconAddress()
        {
            if (icon) iconAddress = Catalog.GetAddressFromPrefab(icon);
        }
        protected void EditorUpdateCloseUpIconAddress()
        {
            if (closeUpIcon) closeUpIconAddress = Catalog.GetAddressFromPrefab(closeUpIcon);
        }
        protected void SetAllPricesToMatch()
        {
            float matchPrice = price;
            foreach (ItemData itemData in Catalog.GetDataList<ItemData>())
                if (itemData.purchasable) itemData.price = matchPrice;
        }
#endif

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Prefab"), NonSerialized, ShowInInspector, ReadOnly] 
#endif
        public List<Item.IconMarker> iconDamagerMarkers = new List<Item.IconMarker>();

        // INVENTORY

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Inventory")] 
#endif
        public bool canBeStoredInPlayerInventory;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Inventory")] 
#endif
        public bool limitMaxStorableQuantity;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Inventory")] 
#endif
        public int maxStorableQuantity;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Inventory")] 
#endif
        public bool isStackable;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Inventory")] 
#endif
        public int maxStackQuantity;
#if ODIN_INSPECTOR
        [FoldoutGroup("Split/Left/Inventory/Audio"), Tooltip("Sound played when the item is hovered with the cursor")] 
#endif
        public GameData.AudioContainerAddressAndVolume inventoryHoverSounds;
#if ODIN_INSPECTOR
        [FoldoutGroup("Split/Left/Inventory/Audio"), Tooltip("Sound played when the item gets out of the inventory")] 
#endif
        public GameData.AudioContainerAddressAndVolume inventorySelectSounds;
#if ODIN_INSPECTORODIN_INSPECTOR
        [FoldoutGroup("Split/Left/Inventory/Audio"), Tooltip("Sound played when the item is stored inside the inventory")] 
#endif
        public GameData.AudioContainerAddressAndVolume inventoryStoreSounds;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder"), ValueDropdown("GetAllHolderSlots")] 
#endif
        public string slot;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder"), HorizontalGroup("Split/Left/Holder/Snaphoriz")] 
#endif
        public string snapAudioContainerAddress;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder")] 
#endif
        public float snapAudioVolume_dB;
        [NonSerialized]
        public IResourceLocation snapAudioContainerLocation;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [HorizontalGroup("Split/Left/Holder/Snaphoriz"), LabelWidth(50), HideLabel, NonSerialized, ShowInInspector, DisableInPlayMode, OnValueChanged("EditorUpdateSnapAudioContainerAddress")] 
#endif
        public AudioContainer editorSnapAudioContainer;

        protected void EditorUpdateSnapAudioContainerAddress()
        {
            if (editorSnapAudioContainer) snapAudioContainerAddress = Catalog.GetAddressFromPrefab(editorSnapAudioContainer);
        }
#endif

#if ODIN_INSPECTORR
        [BoxGroup("Split/Left/Holder"), HorizontalGroup("Split/Left/Holder/Unsnaphoriz")] 
#endif
        public string unsnapAudioContainerAddress;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder")] 
#endif
        public float unsnapAudioVolume_dB;
        [NonSerialized]
        public IResourceLocation unsnapAudioContainerLocation;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [HorizontalGroup("Split/Left/Holder/Unsnaphoriz"), LabelWidth(50), HideLabel, NonSerialized, ShowInInspector, DisableInPlayMode, OnValueChanged("EditorUpdateUnsnapAudioContainerAddress")] 
#endif
        public AudioContainer editorUnsnapAudioContainer;

        protected void EditorUpdateUnsnapAudioContainerAddress()
        {
            if (editorUnsnapAudioContainer) unsnapAudioContainerAddress = Catalog.GetAddressFromPrefab(editorUnsnapAudioContainer);
        }
#endif

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllHolderSlots()
        {
            return Catalog.GetDropdownHolderSlots();
        } 
#endif

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Rigidbody")] 
#endif
        public bool overrideMassAndDrag;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Rigidbody"), ShowIf("overrideMassAndDrag")] 
#endif
        public float mass = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Rigidbody"), ShowIf("overrideMassAndDrag")] 
#endif
        public float drag = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Rigidbody"), ShowIf("overrideMassAndDrag")] 
#endif
        public float angularDrag = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bonus")] 
#endif
        public float manaRegenMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bonus")] 
#endif
        public float spellChargeSpeedPlayerMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bonus")] 
#endif
        public float spellChargeSpeedNPCMultiplier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Collisions")] 
#endif
        public int collisionMaxOverride;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Collisions")] 
#endif
        public bool collisionEnterOnly;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Collisions")] 
#endif
        public bool collisionNoMinVelocityCheck;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Collisions")] 
#endif
        public LayerName forceLayer = LayerName.None;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Collisions")]
#endif
        public bool diffForceLayerWhenHeld = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Collisions")]
#endif
        public LayerName forceLayerHeld = LayerName.None;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Water")] 
#endif
        public AnimationCurve waterHandSpringMultiplierCurve = AnimationCurve.EaseInOut(0, 0.3f, 1, 0.15f);
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Water")] 
#endif
        public AnimationCurve waterDragMultiplierCurve = AnimationCurve.EaseInOut(0, 1, 1, 10);
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Water")] 
#endif
        public float waterSampleMinRadius = 0.2f;

#if ODIN_INSPECTORODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public bool flyFromThrow = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public float flyRotationSpeed = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public float flyThrowAngle = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public float telekinesisSafeDistance = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public bool telekinesisSpinEnabled = true;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public float telekinesisThrowRatio = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public bool telekinesisAutoGrabAnyHandle;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public bool grippable = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        public bool grabAndGripClimb = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Object")] 
#endif
        [Tooltip("Change mesh and collider layer to MovingItem when grabbed or gripped")]
        public bool playerGrabAndGripChangeLayer = true;

        public enum DamageTransfer
        {
            Self,
            Handler,
            None,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder")]  
#endif
        public List<CustomSnap> customSnaps = new List<CustomSnap>();

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder/Imbuing")] 
#endif
        public bool drainImbueOnSnap = true;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Holder/Imbuing")] 
#endif
        [Tooltip("X → Time (seconds). Y → Imbue value (normalized [0; 1])")]
        public AnimationCurve imbueEnergyOverTimeOnSnap
            = AnimationCurve.EaseInOut(0, 1, 3, 0);

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Modules"), ShowInInspector] 
#endif
        public List<ItemModule> modules;

        [Serializable]
        public class CustomSnap
        {
            [JsonMergeKey]
            public string holderName;
            public Vector3 localPosition;
            public Vector3 localRotation;
        }

#if ODIN_INSPECTOR
        [VerticalGroup("Split/Right")]
        [BoxGroup("Split/Right/ColliderGroups")] 
#endif
        public List<ColliderGroup> colliderGroups = new List<ColliderGroup>();

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Damagers")] 
#endif
        public List<Damager> damagers = new List<Damager>();

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Interactables")] 
#endif
        public List<Interactable> Interactables = new List<Interactable>();

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/EffectHinge")] 
#endif
        public List<EffectHinge> effectHinges = new List<EffectHinge>();

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Whooshs")] 
#endif
        public List<Whoosh> whooshs = new List<Whoosh>();

        [Serializable]
        public class ColliderGroup
        {
            [JsonMergeKey]
            public string transformName = "Blades";
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllColliderGroupID")] 
#endif
            public string colliderGroupId;
            [NonSerialized]
            public ColliderGroupData colliderGroupData;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllColliderGroupID()
            {
                return Catalog.GetDropdownAllID(Category.ColliderGroup);
            } 
#endif

            public void OnCatalogRefresh()
            {
                if (colliderGroupId != null && colliderGroupId != "") colliderGroupData = Catalog.GetData<ColliderGroupData>(colliderGroupId);
            }
        }

        [Serializable]
        public class Interactable
        {
            [JsonMergeKey]
            public string transformName = "Handle";
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllInteractableID")] 
#endif
            public string interactableId;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllInteractableID()
            {
                return Catalog.GetDropdownAllID(Category.Interactable);
            } 
#endif
        }

        [Serializable]
        public class EffectHinge
        {
            [JsonMergeKey]
            public string transformName = "EffectHinge";
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllEffectID")] 
#endif
            public string effectId;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Category.Effect);
            } 
#endif
            public float minTorque = 5;
            public float maxTorque = 12;
        }

        [Serializable]
        public class Whoosh
        {
            [JsonMergeKey]
            public string transformName = "Whoosh";
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllEffectID")] 
#endif
            public string effectId;

            public WhooshPoint.Trigger trigger = WhooshPoint.Trigger.Always;
            public bool stopOnSnap = true;
            public float minVelocity = 5;
            public float maxVelocity = 12;
            public float dampening = 0.1f;
#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Category.Effect);
            } 
#endif
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetCategories()
        {
            return Catalog.gameData.GetCategories();
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        } 
#endif

        [Serializable]
        public class Damager
        {
            [JsonMergeKey]
            public string transformName = "Damager";
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllDamagerID")] 
#endif
            public string damagerID;
            [NonSerialized]
            public DamagerData damagerData;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllDamagerID()
            {
                return Catalog.GetDropdownAllID(Category.Damager);
            } 
#endif

            public void OnCatalogRefresh()
            {
                if (!string.IsNullOrEmpty(damagerID)) damagerData = Catalog.GetData<DamagerData>(damagerID);
            }
        }

        [NonSerialized]
        public ItemModuleAI moduleAI;

        public T GetModule<T>() where T : ItemModule
        {
            if (modules != null)
            {
                foreach (ItemModule itemModule in modules)
                {
                    if (itemModule is T) return itemModule as T;
                }
            }
            return null;
        }

        public bool HasModule<T>() where T : ItemModule
        {
            if (modules != null)
            {
                foreach (ItemModule itemModule in modules)
                {
                    if (itemModule is T) return true;
                }
            }
            return false;
        }

        public override int GetCurrentVersion()
        {
            return 4;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHODS
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    public enum CenterType
    {
        Mass,
        Root,
        Renderer,
        MainHandle,
        Holder
    }
}
