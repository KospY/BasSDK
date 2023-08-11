﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using EasyButtons;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class CreatureTool : ThunderBehaviour
    {
        public static Creature currentEventCreature;
        public static RagdollPart currentEventPart;

        public bool clearAfterAction = false;
        public string defaultNameFilter = "Human";
        public CreatureEventLinker.LifeState defaultAliveState = CreatureEventLinker.LifeState.Either;
        public List<SelectionSetting> selectors = new List<SelectionSetting>();
        public List<RagdollPartAction> ragdollPartActions = new List<RagdollPartAction>();

        [NonSerialized]
        public List<Creature> selectedCreatures = new List<Creature>();

        protected RagdollPart targetPart;

        [Serializable]
        public class SelectionSetting
        {
            public string nameFilter = "Human";
            public SelectionCondition condition = SelectionCondition.Random;
            public CreatureEventLinker.LifeState aliveState = CreatureEventLinker.LifeState.Either;
            public bool includePlayer;
            public int repeat = 1;
        }

        [Serializable]
        public class RagdollPartAction
        {
            public RagdollPart.Type type;
            public RagdollPart.Section section;
            public UnityEvent action;
        }

        public enum SelectionCondition
        {
            Random,
            NewestSpawn,
            OldestSpawn,
            LastHit,
            LastAttack,
            Closest,
            Furthest,
        }

        #region Selection
        public void ClearSelections()
        {
        }

        public void PruneRandomly(int leaveBehind)
        {
        }

        public void SelectPlayer()
        {
        }

        public void RemovePlayer()
        {
        }

        public void SetPlayerEventTarget()
        {
        }

        public void SelectItemLastHandler(Item item)
        {
        }

        public void RemoveItemLastHandler(Item item)
        {
        }

        public void SelectAllCurrentlyInZone(Zone zone)
        {
        }

        public void RemoveAllCurrentlyInZone(Zone zone)
        {
        }

        public void ZoneSetEventTarget(UnityEngine.Object creatureObj)
        {
        }

        public void ZoneAddToSelection(UnityEngine.Object creatureObj) => ZoneAddRemove(creatureObj, true);

        public void ZoneRemoveFromSelection(UnityEngine.Object creatureObj) => ZoneAddRemove(creatureObj, false);

        protected void ZoneAddRemove(UnityEngine.Object creatureObj, bool add)
        {
        }

        public void AddUsingSelectorIndex(int index)
        {
        }

        public void RemoveUsingSelectorIndex(int index)
        {
        }
        #endregion


        #region Selected creature tools
        public void CopyComponentToSelection(Component component)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void RemoveComponentFromSelection(Component component)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void CopyComponentToAllSelectionParts(Component component)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void RemoveComponentFromAllSelectionParts(Component component)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void RunRagdollPartActionAtIndex(int index)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void HealSelection(float healing)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void ResurrectSelection(float healing)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void ResurrectSelectionMaxHealth()
        {
            if (clearAfterAction) ClearSelections();
        }

        public void OneSelectedRightHandGrab(Handle handle)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void OneSelectedLeftHandGrab(Handle handle)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void ReleaseRightHandsSelection() => ReleaseHandsSelection(false, true);

        public void ReleaseLeftHandsSelection() => ReleaseHandsSelection(true, false);

        public void ReleaseBothHandsSelection() => ReleaseHandsSelection(true, true);

        protected void ReleaseHandsSelection(bool left, bool right)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void FreezeSelection() => RagdollStateSelection(Ragdoll.State.Frozen);

        public void DestabilizeSelection() => RagdollStateSelection(Ragdoll.State.Destabilized);

        public void RagdollSelection() => RagdollStateSelection(Ragdoll.State.Inert);

        protected void RagdollStateSelection(Ragdoll.State state)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void TeleportSelection(Transform target)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void ManaDeltaSelection(float manaChange)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void DamageSelection(float damage)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void KillSelection()
        {
            if (clearAfterAction) ClearSelections();
        }

        public void DecapitateSelection(bool dontKill)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void FullSliceSelection(bool dontKill)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void ShredSelection(bool dontKill)
        {
            if (clearAfterAction) ClearSelections();
        }

        public void DespawnSelection()
        {
            if (clearAfterAction) ClearSelections();
        }
        #endregion

        #region EventTarget creature tools
        public void CopyComponentToEventTarget(Component component)
        {
        }

        public void RemoveComponentFromEventTarget(Component component)
        {
        }

        public void CopyComponentToAllEventTargetParts(Component component)
        {
        }

        public void RemoveComponentFromAllEventTargetParts(Component component)
        {
        }

        public void RunRagdollPartActionAtIndexOnEventTarget(int index)
        {
        }

        public void HealEventTarget(float healing)
        {
        }

        public void ResurrectEventTarget(float healing)
        {
        }

        public void ResurrectEventTargetMaxHealth()
        {
        }

        public void EventTargetRightHandGrab(Handle handle)
        {
        }

        public void EventTargetLeftHandGrab(Handle handle)
        {
        }

        public void ReleaseRightHandsEventTarget() => ReleaseHandsEventTarget(false, true);

        public void ReleaseLeftHandsEventTarget() => ReleaseHandsEventTarget(true, false);

        public void ReleaseBothHandsEventTarget() => ReleaseHandsEventTarget(true, true);

        protected void ReleaseHandsEventTarget(bool left, bool right)
        {
        }

        public void FreezeEventTarget() => RagdollStateEventTarget(Ragdoll.State.Frozen);

        public void DestabilizeEventTarget() => RagdollStateEventTarget(Ragdoll.State.Destabilized);

        public void RagdollEventTarget() => RagdollStateEventTarget(Ragdoll.State.Inert);

        protected void RagdollStateEventTarget(Ragdoll.State state)
        {
        }

        public void TeleportEventTarget(Transform target)
        {
        }

        public void ManaDeltaEventTarget(float manaChange)
        {
        }

        public void DamageEventTarget(float damage)
        {
        }

        public void KillEventTarget()
        {
        }

        public void DecapitateEventTarget(bool dontKill)
        {
        }

        public void FullSliceEventTarget(bool dontKill)
        {
        }

        public void ShredEventTarget(bool dontKill)
        {
        }

        public void DespawnEventTarget()
        {
        }
        #endregion

        #region Part tools (By index)
        public void CopyComponentToPart(Component component)
        {
        }

        public void RemoveComponentFromPart(Component component)
        {
        }

        public void DisablePartAnimation(bool recursive)
        {
        }

        public void ResetPartAnimation(bool recursive)
        {
        }

        public void SlicePart(bool dontKill)
        {
        }
        #endregion

        #region Part tools (By linker)
        public void CopyComponentToEventTargetPart(Component component)
        {
        }

        public void RemoveComponentFromEventTargetPart(Component component)
        {
        }

        public void DisableEventTargetPartAnimation(bool recursive)
        {
        }

        public void ResetEventTargetPartAnimation(bool recursive)
        {
        }

        public void SliceEventTargetPart(bool dontKill)
        {
        }
        #endregion
    }
}
