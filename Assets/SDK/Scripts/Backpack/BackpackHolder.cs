using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace ThunderRoad
{
    /// <summary>
    /// Interactable that allows the player to interacts with its inventory contents
    /// through releasing items in a trigger zone
    /// </summary>
    public class BackpackHolder : Interactable
    {
        // Flat list of the creature's inventory containers
        public Container creatureContainer;

        // Creature holding the backpack
        public Creature creature;

        public AudioSource uiSoundsAudioSource;

        /// <summary>
        /// Constant in seconds, used to avoid audio clutter
        /// </summary>
        public float soundPlayDelay = 0.25f;

        /// <summary>
        /// Used to open the inventory from the torso
        /// Defines a distance threshold to open the inventory with the hands
        /// </summary>
        public float distanceFromTorsoToOpen = .125f;

        /// <summary>
        /// Used to close the inventory when the hand leaves the torso
        /// Defines a distance threshold to close the inventory with the hands
        /// </summary>
        public float distanceFromTorsoToClose = .15f;

        /// <summary>
        /// Offset the head position to create a fake torso point, used for opening distances
        /// </summary>
        public Vector3 fakeTorsoOffsetFromHead = -Vector3.up * .3f;

        /// <summary>
        /// Distance to offset the storing zone when crouching.
        /// lerp between 0 and this using the player height offset
        /// </summary>
        public float capsuleOffsetCrouch = .11f;

        public delegate void InventoryLoad();
        public delegate void ItemDelegate(Item storedItem, InventoryContent.InventoryItem inventoryItem);
        public delegate void ItemsDelegate(Item[] items);

        public event InventoryLoad OnInventoryLoad;
        public event ItemDelegate OnItemStored;
        public event ItemDelegate OnItemStoredFail;
        public event ItemsDelegate OnItemsRetrieved;


    }
}