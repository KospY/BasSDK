using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Pierced Spawner")]
    public class PiercedSpawner : ThunderBehaviour
    {
        public bool spawnOnStart = true;
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

        [ValueDropdown("GetAllWeapons")]
#endif
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

        [ValueDropdown("GetAllDamagers")]
#endif
        public string pierceDamagerName;
        public Collider pierceTarget;
        [Range(0f, 10f)]
        public float raycastDistance;
        [Range(0f, 10f)]
        public float targetPierceDepth;
        public bool overridePierceDepth;
        [Range(0f, 10f)]
        public float externalPierceDepthAllowed;
        public bool alignChildItems;
        public List<ItemSpawner> childsToPierce;
        public ItemSpawner parentSpawner;

        protected bool foundPiercePoint = false;
        protected bool spawned = false;
        protected ItemData pierceWeaponData;
        protected Vector3 lastCalcPiercePointStart;
        protected Vector3 lastCalcPiercePointRotat;
        protected Vector3 colliderPiercePoint;
        protected float distanceToPiercePoint;

        public void Spawn()
        {
        }

        protected void Setup()
        {
            FindPiercePoint();
        }

        protected void FindPiercePoint()
        {
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
            float penetration = moveItemRender * (distanceToPiercePoint + targetPierceDepth);
            Gizmos.color = penetration > distanceToPiercePoint ? Color.green : (overridePierceDepth && penetration > distanceToPiercePoint - externalPierceDepthAllowed ? Color.blue : Color.white);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, Vector3.one / 2);
        }

#endif
    }
}
