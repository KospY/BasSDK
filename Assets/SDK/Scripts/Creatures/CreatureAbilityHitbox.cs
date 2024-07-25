using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class CreatureAbilityHitbox : MonoBehaviour
    {
        public bool forcer = true;
        public bool damager = true;
#if ODIN_INSPECTOR
        [TableList]
#endif
        public List<InflictedStatus> applyStatuses = new List<InflictedStatus>();

        [System.Serializable]
        public class InflictedStatus
        {
#if ODIN_INSPECTOR
            private List<ValueDropdownItem<string>> GetAllStatuses => Catalog.GetDropdownAllID<StatusData>();

            [ValueDropdown(nameof(GetAllStatuses))]
#endif
            public string data;
            public float duration = 3f;
        }

        [NonSerialized]
        public Collider hitCollider;
        public bool active => hitCollider.enabled;

        protected Creature creature;
        protected CreatureAbility ability;

        protected static Dictionary<Creature, List<PhysicBody>> affectedBodies = new Dictionary<Creature, List<PhysicBody>>();
        protected static Dictionary<Creature, List<Creature>> damagedCreatures = new Dictionary<Creature, List<Creature>>();

        // Start is called before the first frame update
        void Start()
        {
            hitCollider = GetComponent<Collider>();
            hitCollider.enabled = false;
            creature = GetComponentInParent<Creature>();
        }

        private void CheckInitCreatureAffectedList<T>(Dictionary<Creature, T> dict) where T : new()
        {
            if (dict?.ContainsKey(creature) != true)
            {
                if (dict == null) dict = new();
                dict.Add(creature, new());
            }
        }

        public void EnableHitBox(CreatureAbility ability)
        {
            this.ability = ability;
            //Debug.Log($"enabled {name} hit box, ability is {ability.name}");  
            hitCollider.enabled = true;
            CheckInitCreatureAffectedList(affectedBodies);
            CheckInitCreatureAffectedList(damagedCreatures);
        }

        public void DisableHitBox()
        {
            //Debug.Log($"disabled {name} hit box");
            ability = null;
            hitCollider.enabled = false;
            affectedBodies.Clear();
            damagedCreatures.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
        }

        private void OnTriggerExit(Collider other)
        {
        }
    }
}