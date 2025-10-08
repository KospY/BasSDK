using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Pierced Spawner")]
    public class PiercedSpawner : ThunderBehaviour
    {
        public Item SpawnedItem { get; private set; }

        [Tooltip("Should the item spawn on start, or only when prompted?")]
        public bool spawnOnStart = true;
        [Tooltip("If the pierced spawner doesn't manage to pierce anything, should the pierce weapon despawn?")]
        public bool despawnIfNoPierce = true;
        public UnityEvent<Item> onPierceEvent = new();
        public UnityEvent onFailPierceEvent = new();
        [Tooltip("If set to false, the pierce will not spawn any effects.")]
        public bool spawnEffects = true;
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllWeapons()
        {
            List<ValueDropdownItem<string>> weapons = new List<ValueDropdownItem<string>>();
            foreach (ItemData data in Catalog.GetDataList<ItemData>())
            {
                if (data.type == ItemData.Type.Weapon) weapons.Add(new ValueDropdownItem<string>(data.id, data.id));
            }
            return weapons;
        }

        [ValueDropdown(nameof(GetAllWeapons))]
#endif
        [Tooltip("If the weapon hits this collider, it should stop piercing at this point and go no deeper!")]
        public string pierceWeaponID;
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllDamagers()
        {
            List<ValueDropdownItem<string>> damagers = new List<ValueDropdownItem<string>>();
            if (pierceWeaponData == null) pierceWeaponData = Catalog.GetData<ItemData>(pierceWeaponID);
            foreach (var damager in pierceWeaponData.damagers)
            {
                damagers.Add(new ValueDropdownItem<string>(damager.transformName, damager.transformName));
            }
            return damagers;
        }

        [ValueDropdown(nameof(GetAllDamagers))]
#endif
        [Tooltip("Pick which damager should do the piercing. This lines up with the pierce transform's name.")]
        public string pierceDamagerName;
        [Tooltip("Sets what the 'impact velocity' of the collision should be, if this hits a ragdoll. Any value greater than 0 makes the pierce deal damage to ragdolls it pierces.")]
        public float ragdollDamageSpeed = 0f;

        [Header("Pierce end")]
        [Tooltip("If the weapon hits this collider, it should stop piercing at this point and go no deeper!")]
        public Collider endPierceCollider;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Collider hitCollider;
        [Header("Raycasting")]
        public LayerMask raycastLayerMask = (LayerMask)(218250337);
        public float raycastDistance;
        [Header("Depth")]
        [Range(0f, 10f)]
        public float targetPierceDepth;
        public bool overridePierceDepth;
        [Range(0f, 10f)]
        public float externalPierceDepthAllowed;
        [Header("Child pierceables")]
        public bool alignChildItems;
        public List<ItemSpawner> childsToPierce;
        public ItemSpawner parentSpawner;

        protected bool foundPiercePoint = false;
        protected ItemData pierceWeaponData;
        protected Vector3 lastCalcPiercePointStart;
        protected Vector3 lastCalcPiercePointRotat;
        protected Vector3 colliderPiercePoint;
        protected float distanceToPiercePoint;

        public void Spawn(bool findPiecePoint = false)
        {
        }

        protected void Setup()
        {
            FindPiercePoint();
        }

        protected void FindPiercePoint()
        {
            lastCalcPiercePointStart = transform.position;
            lastCalcPiercePointRotat = transform.eulerAngles;
            colliderPiercePoint = transform.forward * raycastDistance;
        }


#if UNITY_EDITOR
        [Range(0f, 1f)]
        public float moveItemRender = 0f;


        public void OnValidate()
        {
            Setup();
        }

        public void OnDrawGizmos()
        {
            if (transform.position != lastCalcPiercePointStart || transform.eulerAngles != lastCalcPiercePointRotat) FindPiercePoint();
            if (parentSpawner != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, parentSpawner.transform.position);
            }
            Vector3 noPierceEnd = overridePierceDepth ? colliderPiercePoint + (-transform.forward * externalPierceDepthAllowed) : colliderPiercePoint;
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(transform.position, noPierceEnd);
            if (overridePierceDepth)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(noPierceEnd, colliderPiercePoint);
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawSolidDisc(noPierceEnd, transform.forward, 0.05f);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(colliderPiercePoint, colliderPiercePoint + (transform.forward * targetPierceDepth));
            float renderMovement = moveItemRender * (distanceToPiercePoint + targetPierceDepth);
            Gizmos.color = renderMovement > distanceToPiercePoint ? Color.green : (overridePierceDepth && renderMovement > distanceToPiercePoint - externalPierceDepthAllowed ? Color.blue : Color.white);
        }

#endif
    }
}
