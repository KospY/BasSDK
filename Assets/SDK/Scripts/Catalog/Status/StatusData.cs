using System;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad
{
    public enum StackType
    {
        Refresh,
        Stack,
        None,
        Infinite
    }
    public class StatusData : CatalogData
    {
        [ShowInInspector]
        [BoxGroup("Status")]
        public Type type;

        [BoxGroup("Status")]
        public CreatureType bypassImmunity;

        [BoxGroup("Status")]
        public StackType stackType;
        
        public string StackTypeDescription => stackType switch
        {
            StackType.Refresh => "Refreshes the duration every time you apply a status.",
            StackType.Stack => "Status duration will accumulate with every application.",
            StackType.None => "Does not stack. Will always last for the duration of the first stack applied.",
            StackType.Infinite => "Never automatically expires.",
            _ => throw new ArgumentOutOfRangeException($"StackType out of range: {stackType}")
        };
        
#if ODIN_INSPECTOR
        [BoxGroup("Conditions")]
#endif
        public bool forceAllowOnCreatures = false;
        
#if ODIN_INSPECTOR
        [BoxGroup("Conditions")]
#endif
        public bool allowOnItems = true;
        
        
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string creatureEffectId;

        [NonSerialized]
        public EffectData creatureEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string itemEffectId;

        [NonSerialized]
        protected EffectData itemEffectData;
        
        public delegate void StatusEvent(Status status);
        public event StatusEvent OnFirstApplyEvent;
        public event StatusEvent OnFullRemoveEvent;

        public void InvokeOnFirstApplyEvent(Status status)
        {
            OnFirstApplyEvent?.Invoke(status);
        }

        public void InvokeOnFullRemoveEvent(Status status)
        {
            OnFullRemoveEvent?.Invoke(status);
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<Type>> GetStatusTypes()
        {
            var dropdownList = new List<ValueDropdownItem<Type>> { new("None", null) };
            
            foreach (var statusType in AppDomain.CurrentDomain
                         .GetAssemblies()
                         .SelectMany(assembly => assembly.GetTypes())
                         .Where(type => typeof(IStatus).IsAssignableFrom(type)))
            {
                dropdownList.Add(new ValueDropdownItem<Type>(statusType.ToString(), statusType));
            }

            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetAllEffectID() => Catalog.GetDropdownAllID(Category.Effect);
        public List<ValueDropdownItem<string>> GetAllAnimationID() => Catalog.GetDropdownAllID(Category.Animation);
#endif
        
    }
}
