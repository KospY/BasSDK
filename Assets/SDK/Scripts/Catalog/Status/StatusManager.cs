using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThunderRoad
{
    public class StatusManager : ThunderBehaviour
    {
        public Dictionary<ThunderEntity, Dictionary<int, IStatus>> statuses = new();
        public static StatusManager Instance => instance;
        private static StatusManager instance;

        private Dictionary<StatusData, Type> dataLookup = new();

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update | ManagedLoops.FixedUpdate;

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        protected internal override void ManagedUpdate()
        {
            base.ManagedUpdate();
        }

        protected internal override void ManagedFixedUpdate()
        {
            foreach (var list in statuses.Values)
            {
                foreach (var status in list.Values)
                {
                    status.FixedUpdate();
                }
            }
        }

        /// <summary>
        /// Register status data to a particular status type
        /// </summary>
        public void Register(StatusData data)
        {
            dataLookup[data] = data.type;
        }

        public bool Has(ThunderEntity entity, StatusData data) => TryGetStatus(entity, data, out _);
        public bool Has(ThunderEntity entity, string id) => TryGetStatus(entity, id, out _);
        public bool TryGetStatus(ThunderEntity entity, string id, out IStatus status)
            => TryGetStatus(entity, Catalog.GetData<StatusData>(id), out status);

        public bool TryGetStatus(ThunderEntity entity, StatusData data, out IStatus status)
        {
            status = default;
            return statuses.TryGetValue(entity, out var list) && list.TryGetValue(data.hashId, out status);
        }

        public void Inflict(
            ThunderEntity entity,
            StatusData data,
            object handler,
            float duration = float.PositiveInfinity,
            object parameter = default,
            bool playEffect = true)
        {
        }

        public bool Remove(ThunderEntity entity, StatusData data, object handler)
        {
            // If we don't have that status, or if it can't remove that handler, return false.
            if (!TryGetStatus(entity, data, out var status) || !status.RemoveHandler(handler)) return false;

            // If no handlers left, remove the status.
            if (!status.Refresh()) return true;
            status.Despawn();
            statuses[entity].Remove(data.hashId);
            return true;
        }

        public void Clear(ThunderEntity entity, string id)
        {
            Clear(entity, Catalog.GetData<StatusData>(id));
        }

        public void Clear(ThunderEntity entity, StatusData data)
        {
            if (!TryGetStatus(entity, data, out var status)) return;
            status.Despawn();
            statuses[entity].Remove(data.hashId);
        }

        public void ClearByHandler(ThunderEntity entity, object handler)
        {
            if (!statuses.TryGetValue(entity, out var list)) return;
            var cloneList = new Dictionary<int, IStatus>(list);
            foreach ((int id, var status) in cloneList)
            {
                status.RemoveHandler(handler);
                // If no handlers left, remove the status.
                if (!status.Refresh()) continue;
                status.Despawn();
                statuses[entity].Remove(id);
            }
        }

        public void ClearByType<T>(ThunderEntity entity) where T : Status
        {
            if (!statuses.TryGetValue(entity, out var list)) return;
            var cloneList = new Dictionary<int, IStatus>(list);
            foreach ((int id, var status) in cloneList)
            {
                if (status is not T) continue;
                status.ClearHandlers();
                // If no handlers left, remove the status.
                if (!status.Refresh()) continue;
                status.Despawn();
                statuses[entity].Remove(id);
            }
        }

        public void ClearAll(ThunderEntity entity)
        {
            if (statuses.TryGetValue(entity, out var dictionary))
            {
                var values = dictionary.Values.ToList();
                foreach (var status in values)
                {
                    status.Despawn();
                }
                dictionary.Clear();
            }

            statuses.Remove(entity);
        }

        public List<IStatus> GetStatuses(ThunderEntity entity)
        {
            return statuses.TryGetValue(entity, out var list) ? list.Values.ToList() : new List<IStatus>();
        }
    }
}
