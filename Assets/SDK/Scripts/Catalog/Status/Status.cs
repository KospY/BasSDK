using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThunderRoad
{
    public interface IStatus
    {
        public void OnCatalogRefresh();
        public void Spawn(StatusData data, ThunderEntity entity);
        /// <summary>
        /// Called when the effect is first applied.
        /// </summary>
        public void Apply();
        /// <summary>
        /// Called when the effect is first applied.
        /// Not called on subsequent Apply() calls from ReapplyOnValueChange.
        /// </summary>
        public void FirstApply();

        public void PlayEffect();
        /// <summary>
        /// Called when the status parameter value has changed, if it has one.
        /// You may want to re-apply or change the effect on your entity.
        /// </summary>
        public void OnValueChange();
        /// <summary>
        /// Called once per entity per effect per FixedUpdate.
        /// </summary>
        public void FixedUpdate();
        /// <summary>
        /// Called once per entity per effect per frame.
        /// </summary>
        public void Update();
        /// <summary>
        /// Called when it's time to remove the effect. May be called on value change if ReapplyOnValueChange is true.
        /// </summary>
        public void Remove();
        /// <summary>
        /// Called when the effect is removed. Only called when the effect is fully removed, not just being reapplied.
        /// </summary>
        public void FullRemove();

        public bool CheckExpired();
        /// <summary>
        /// Called to check for any expiring handlers.
        /// </summary>
        public bool Refresh();
        /// <summary>
        /// Called to despawn the status and release its pooled objects
        /// </summary>
        public void Despawn();
        /// <summary>
        /// Does the status effect have any handlers?
        /// </summary>
        public bool HasHandlers();
        /// <summary>
        /// Remove a handler from this effect.
        /// Does not refresh the effect, use <c>ThunderEntity.Remove()</c> instead.
        /// </summary>
        public bool RemoveHandler(object handler);
        /// <summary>
        /// Remove all handlers from this effect.
        /// Does not remove the effect, use <c>ThunderEntity.Clear()</c> instead.
        /// </summary>
        public void ClearHandlers();

        public void Transfer(ThunderEntity other);

        public bool AddHandler(
            object handler,
            float duration = float.PositiveInfinity,
            object parameter = default,
            bool playEffect = true);
    }

    public abstract class Status : IStatus
    {
        public StatusData data;
        public ThunderEntity entity;
        public virtual void Spawn(StatusData data, ThunderEntity entity)
        {
        }

        public virtual void OnCatalogRefresh() { }

        // List of handlers on this effect, with optional expiry time.
        protected Dictionary<object, (float expiry, object parameter)> handlers;

        public object value;
        public float expiry;
        public float startTime;

        public virtual void FirstApply()
        {
            startTime = Time.time;
            data.InvokeOnFirstApplyEvent(this);
        }
        public virtual void Apply() { }

        public virtual void PlayEffect()
        {
        }


        private void OnStateChange(
            Ragdoll.State oldState,
            Ragdoll.State newState,
            Ragdoll.PhysicStateChange physicsChange,
            EventTime time)
        {
        }

        public virtual bool ReapplyOnValueChange => false;
        public virtual void OnValueChange()
        {
            value = GetValue();

            if (!ReapplyOnValueChange)
                return;
            Remove();
            Apply();
        }

        public virtual void FixedUpdate() { }
        public virtual void Update() { }

        public virtual void Remove()
        {
        }
        public virtual void FullRemove()
        {
        }
        public bool HasHandlers() => handlers.Count > 0;
        public virtual bool RemoveHandler(object handler) => handlers.Remove(handler);
        public void ClearHandlers() => handlers.Clear();

        /// <summary>
        /// Combine values from the list of handlers on this status instance.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetValue() => null;

        public virtual bool AddHandler(object handler, float duration = float.PositiveInfinity, object parameter = default, bool playEffect = true)
        {
            var isNew = false;
            bool noChange = handlers.TryGetValue(handler, out var current) && Equals(current.parameter, value);
            if (!HasHandlers())
                isNew = true;

            if (duration == 0)
                duration = Mathf.Infinity;

            // Deal with status duration stacking
            switch (GetStackType())
            {
                case StackType.Infinite:
                handlers[handler] = (expiry, parameter);
                break;
                case StackType.Refresh:
                float thisExpiry = Time.time + duration;
                // This status effect tracks separate expiry times per handler
                handlers[handler] = (thisExpiry, parameter);
                if (float.IsInfinity(expiry) || thisExpiry > expiry)
                    expiry = thisExpiry;
                break;
                case StackType.Stack:
                // This status effect has a single duration that is added to with each new stack
                if (expiry == 0)
                    expiry = Time.time;
                expiry += duration;
                // (it doesn't actually matter what we put as the duration of the handler)
                handlers[handler] = (expiry, parameter);
                break;
                case StackType.None:
                // This status effect _cannot_ be refreshed by new handlers,
                // and always lasts exactly for the first duration applied to it.
                if (expiry == 0)
                    handlers[handler] = (Time.time + duration, parameter);
                else
                    handlers[handler] = (expiry, parameter);
                break;
            }

            if (noChange)
                return false;
            // If it's a new status effect, we need to calculate the initial value and apply it.
            if (isNew)
            {
                value = GetValue();
                FirstApply();
                Apply();
                if (playEffect)
                    PlayEffect();
            }
            else
            {
                // Trigger the OnValueChange event to tell the status that its parameters may have changed.
                OnValueChange();
            }

            return true;
        }

        public float Duration => expiry - Time.time;

        public bool CheckExpired()
        {
            switch (GetStackType())
            {
                case StackType.Infinite:
                // Delegate expiry checking to "does this have handlers" check
                return !HasHandlers();
                case StackType.Refresh:
                // We need to check and remove each handler's duration
                var expired = false;
                foreach ((object handler, (float expiry, _)) in handlers.ToList())
                {
                    if (Time.time < expiry)
                        continue;
                    RemoveHandler(handler);
                    expired = true;
                }

                return expired;
                case StackType.Stack:
                case StackType.None:
                // We just care about the highest expiry date
                if (Time.time >= expiry)
                {
                    ClearHandlers();
                    return true;
                }
                break;
            }

            return false;
        }

        public bool Refresh()
        {
            if (HasHandlers())
            {
                // Still have some handlers but the number has changed, so we should recalculate the parameter
                OnValueChange();
            }
            else
            {
                // No handlers left. Bye bye status.
                return true;
            }

            return false;
        }

        public void Despawn()
        {
        }

        /// <summary>
        /// Replay every non-infinite status onto another entity
        /// </summary>
        /// <param name="other">The entity to inflict these statuses upon</param>
        public virtual void Transfer(ThunderEntity other)
        {
        }

        public StackType GetStackType() => data?.stackType ?? StackType.Refresh;
    }
}