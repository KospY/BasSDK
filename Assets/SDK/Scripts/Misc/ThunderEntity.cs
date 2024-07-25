using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad
{

    public abstract class AbstractEntityVariable {}
    public class EntityVariable<T> : AbstractEntityVariable
    {
        private Dictionary<string, T> dictionary;

        public T Set(string name, T value)
        {
            dictionary ??= new Dictionary<string, T>();
            dictionary[name] = value;
            return value;
        }

        public T Set(string name, Func<T, T> func)
        {
            dictionary ??= new Dictionary<string, T>();
            var newValue = TryGetValue(name, out var value) ? func(value) : func(default);
            dictionary[name] = newValue;
            return newValue;
        }

        public T Get(string name)
        {
            dictionary ??= new Dictionary<string, T>();
            return TryGetValue(name, out var value) ? value : default;
        }
        public bool TryGetValue(string name, out T value)
        {
            dictionary ??= new Dictionary<string, T>();
            return dictionary.TryGetValue(name, out value);
        }

        public bool Clear(string name)
        {
            return dictionary?.Remove(name) ?? false;
        }
    }

    public abstract class ThunderEntity : ThunderBehaviour
    {
        public static List<ThunderEntity> allEntities = new();
        public List<EntityModule> entityModules;
        public Dictionary<Type, AbstractEntityVariable> variables;
 // ProjectCore
        public virtual void Despawn()
        {
        }
 // ProjectCore
    }

    public static class Filter
    {

        public static bool LiveCreatures(ThunderEntity entity)
            => entity is Golem { isKilled: false } or Creature { isKilled: false };

    }
}
