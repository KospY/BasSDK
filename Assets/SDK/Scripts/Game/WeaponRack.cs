using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#if ProjectCore
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class WeaponRack : MonoBehaviour
    {
        public string rackId = "Rack1";
        public bool playerRack;

#if ProjectCore
        public List<ObjectHolder> holders;

        private void Start()
        {
            holders = new List<ObjectHolder>(this.GetComponentsInChildren<ObjectHolder>());
            if (playerRack) LoadFromPlayerData();
            foreach (ObjectHolder holder in holders)
            {
                holder.Snapped += OnSnap;
                holder.UnSnapped += OnUnSnap;
            }
        }

        protected void OnSnap(Item item)
        {
            item.definition.SetSavedValue(SavedValueID.Rack.ToString(), rackId);
        }

        protected void OnUnSnap(Item item)
        {
            item.definition.SetSavedValue(SavedValueID.Rack.ToString(), null);
        }

        [Button]
        public void LoadFromPlayerData()
        {
            foreach (ObjectHolder objectHolder in holders)
            {
                if (objectHolder.holdObjects.Count > 0)
                {
                    Item temp = objectHolder.holdObjects[0];
                    objectHolder.UnSnapOne();
                    temp.Despawn();
                }
                foreach (ContainerData.Content content in GameManager.playerData.rackInventory)
                {
                    if (content.TryGetCustomValue(SavedValueID.Rack.ToString(), out string rackValue) && content.TryGetCustomValue(SavedValueID.Holder.ToString(), out string holderValue))
                    {
                        if (rackValue == rackId && holderValue == objectHolder.name)
                        {
                            Item item = content.Spawn();
                            objectHolder.Snap(item, true);
                            break;
                        }
                    }
                }
            }
        }

        [Button]
        public void SaveToPlayerData()
        {
            // Delete existing rack from player rack inventory
            for (int i = GameManager.playerData.rackInventory.Count - 1; i >= 0; i--)
            {
                if (GameManager.playerData.rackInventory[i].TryGetCustomValue(SavedValueID.Rack.ToString(), out string rackValue))
                {
                    if (rackId == rackValue)
                    {
                        GameManager.playerData.rackInventory.Remove(GameManager.playerData.rackInventory[i]);
                    }
                }
            }
            // Add to player rack inventory
            foreach (ObjectHolder objectHolder in holders)
            {
                if (objectHolder.holdObjects.Count == 1)
                {
                    GameManager.playerData.rackInventory.Add(new ContainerData.Content(objectHolder.holdObjects[0].data, 1, objectHolder.holdObjects[0].definition.savedValues != null ? new List<ItemDefinition.SavedValue>(objectHolder.holdObjects[0].definition.savedValues) : null));
                }
            }
        }
#endif
    }
}
