using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/ContainerSaver.html")]
    public class ContainerSaver : MonoBehaviour
    {
        public static ContainerSaver instance;

        [Tooltip("Do you want to save the player inventory?")]
        public bool savePlayerInventory;
        [Tooltip("Do you want to save the container when the level unloads?")]
        public bool saveOnLevelUnload;
        [Tooltip("When enabled, the container will only save if the player is alive.")]
        public bool saveIfPlayerAliveOnly = true;
        [Tooltip("Saves the container when the game is closed.")]
        public bool saveOnAppQuit;
        [Tooltip("When enabled, unowned items will become owned by player when grabbed.")]
        public bool transferUnOwnedItemToPlayerOnGrab;
        [Tooltip("When enabled, items inside spawners are transferred to player.")]
        public bool transferIncludeItemFromSpawner;

        [Tooltip("List of saved containers.")]
        public List<ContainerSave> saveContainers;

        [Header("Events")]
        public UnityEvent onPreSave;

        [Serializable]
        public class ContainerSave
        {
            [Tooltip("States the Player Container which the container saver saves to.\n\nCrystal Hunt default container is \"PlayerStash\"")]
            public string playerContainerID;
            [Tooltip("(Optional) Can select a container placed in scene.")]
            public Container container;
            [Tooltip("Who owns the container?")]
            public OwnerFilter ownerFilter = OwnerFilter.Player;
            [Tooltip("Depicts the save behaviour of the container.\n\nRetrieve to Container: Items will go to the Container (like the stash).\nSave Current Position: Will save the item position in the scene (like Level Persistence).")]
            public SaveBehavior saveBehavior = SaveBehavior.RetrieveToContainer;
            [Tooltip("Filter the types of items that are affected by the container saver.")]
            public List<ItemData.Type> retrieveFilterTypes;
            [Tooltip("Specific excluded item types from container saver")]
            public List<ItemData.Type> excludedTypes;
            [Tooltip("Define whether an item must be in one of the holders in the holder list (None except), or anywhere except in one of those holders (Any except)")]
            public FilterLogic holderFilterMode = FilterLogic.AnyExcept;
            [Tooltip("Holders used for the above filter")]
            public List<Holder> holders;
            [Tooltip("When enabled, items grabbed by player will automatically become owned by the player.")]
            public bool transferToPlayerOnGrab;
            [Tooltip("When true, only allowed items will be stored to the container.")]
            public bool retrieveAllowedStorageItemOnly = true;
        }

        public enum OwnerFilter
        {
            Player,
            LinkedContainer,
        }

        public enum SaveBehavior
        {
            RetrieveToContainer,
            SaveCurrentPosition,
        }

        [Button]
        public void TransferCurrentGrabbedOrHolsteredNpcItemToPlayer()
        {
        }

        [Button]
        public void Save()
        {
        }

    }
}
