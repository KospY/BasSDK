using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class IgnoreCollisionArea : ThunderBehaviour
    {
        public Item item;
        public Collider collider;
        
        public List<Item> items = new List<Item>();

        protected void Awake()
        {
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
            item = GetComponentInParent<Item>();
        }
        
        public void OnTriggerEnter(Collider other)
        {
            //check if the collider has a item in parent
            
            Item otherItem = other.GetComponentInParent<Item>();
            if(otherItem == this.item) return;
            if (otherItem == null) return;
            OnItemAdded(otherItem);
        }
        
        public void OnTriggerExit(Collider other)
        {
            //check if the collider has a item in parent
            Item otherItem = other.GetComponentInParent<Item>();
            if(otherItem == this.item) return;
            if(otherItem == null) return;
            OnItemRemoved(otherItem);
        }

        public virtual void OnItemRemoved(Item otherItem)
        {
        }

        public virtual void OnItemAdded(Item otherItem)
        {
        }
        
    }
}
