using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class FloatingHolder : ThunderBehaviour
    {
        public Item item;
        public Collider collider;
        
        public bool isOpen; //or enabled, like can the player grab from it
        public List<Item> items = new List<Item>();

        protected void Awake()
        {
            collider = GetComponent<Collider>();
            item = GetComponentInParent<Item>();
        }

    }
}
