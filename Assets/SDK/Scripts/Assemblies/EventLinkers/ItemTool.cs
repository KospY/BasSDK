using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class ItemTool : ThunderBehaviour
    {
        public static Item currentEventItem;

        public bool clearAfterAction = false;
        public List<SelectionSetting> selectors = new List<SelectionSetting>();
        public UnityEvent<Item> itemEvent;

        [NonSerialized]
        public List<Item> selectedItems = new List<Item>();

        [Serializable]
        public class SelectionSetting
        {
            public string idFilter = "";
            public SelectionCondition condition = SelectionCondition.Random;
            public VennState playerGrabState = VennState.Either;
            public VennState NPCGrabState = VennState.Either;
            public VennState breakable = VennState.Either;
            public int repeat = 1;
        }

        public enum VennState
        {
            Either,
            Yes,
            No,
        }

        public enum SelectionCondition
        {
            Random,
            NewestSpawn,
            OldestSpawn,
            Closest,
            Furthest,
            NewestInteracted,
            OldestInteracted,
        }

        public void ClearSelections()
        {
        }

        public void PruneRandomly(int leaveBehind)
        {
        }

        public void SelectAllCurrentlyInZone(Zone zone)
        {
        }

        public void RemoveAllCurrentlyInZone(Zone zone)
        {
        }

        public void ZoneSetEventTarget(UnityEngine.Object itemObj, bool add)
        {
        }

        public void ZoneAddToSelection(UnityEngine.Object itemObj) => ZoneAddRemove(itemObj, true);

        public void ZoneRemoveFromSelection(UnityEngine.Object itemObj) => ZoneAddRemove(itemObj, false);

        protected void ZoneAddRemove(UnityEngine.Object itemObj, bool add)
        {
        }

        public void AddUsingSelectorIndex(int index)
        {
        }

        public void RemoveUsingSelectorIndex(int index)
        {
        }


        #region Dynamic invocation item tools
        public void RunEventWithItem(Item item)
        {
        }

        public void BreakEventTargetBreakable(Item item)
        {
        }

        public void DespawnItemInstantly(Item item)
        {
        }

        public void EquipItemToPlayerRightHand(Item item)
        {
        }

        public void EquipItemToPlayerLeftHand(Item item)
        {
        }

        #endregion

        #region Selected item tools
        public void CopyComponentToSelection(Component component)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void RemoveComponentFromSelection(Component component)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void RunEventWithSelection()
        {
            if (clearAfterAction) ClearSelections();
        }

        public void ClearSelectionStatuses(string id)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void BreakSelectedBreakables()
        {
            if (clearAfterAction) ClearSelections();
        }

        public void DespawnSelected(float delay)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void EquipOneSelectedToPlayerRightHand()
        {
        }

        public void EquipOneSelectedToPlayerLeftHand()
        {
        }

        public void EquipOneSelectedToHand(RagdollHand hand)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void SwapSelectedWith(string itemID)
        {
            if (clearAfterAction) ClearSelections();
        }
        #endregion

        #region Event target item tools
        public void CopyComponentToEventTarget(Component component)
        {
        }

        public void RemoveComponentFromEventTarget(Component component)
        {
        }

        public void RunEventWithEventTarget()
        {
        }

        public void ClearEventTargetStatuses(string id)
        {
        }

        public void BreakEventTargetBreakable()
        {
        }

        public void DespawnEventTarget(float delay)
        {
        }

        public void EquipEventTargetToPlayerRightHand()
        {
        }

        public void EquipEventTargetToPlayerLeftHand()
        {
        }

        public void EquipEventTargetToHand(RagdollHand hand)
        {
        }

        public void SwapEventTargetWith(string itemID)
        {
        }
        #endregion
    }
}
