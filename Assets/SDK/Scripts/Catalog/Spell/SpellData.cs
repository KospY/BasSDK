using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class SpellData : SkillData, IContainerLoadable<SpellData>
    {
        [NonSerialized]
        public Dictionary<int, Dictionary<object, float>> modifiers;
        [NonSerialized]
        public Dictionary<int, float> multipliers;

        public virtual void DrawGizmos() { }

        public virtual void DrawGizmosSelected() { }

        public override ContainerContent InstanceContent()
        {
            return new SpellContent(this);
        }

        public override void Init()
        {
            base.Init();
            SetupModifiers();
        }

        public override CatalogData Clone()
        {
            var clone = base.Clone();
            SetupModifiers();
            return clone;
        }

        public string SkillPassiveLabel(string name, float value)
            => $"{name} Mult. per Skill (max {value * 3 * 100}%)";

        public void SetupModifiers()
        {
            modifiers = new Dictionary<int, Dictionary<object, float>>();
            multipliers = new Dictionary<int, float>();
        }


        /// <summary>
        /// Add a default modifier to a spell.
        /// </summary>
        /// <param name="handler">Owner of this modifier</param>
        /// <param name="modifier">One of the three basic modifier types</param>
        /// <param name="value">Modifier value</param>
        public void AddModifier(object handler, Modifier modifier, float value)
        {
            if (!modifiers.ContainsKey((int)modifier))
                modifiers[(int)modifier] = new Dictionary<object, float>();
            modifiers[(int)modifier][handler] = value;
            RefreshMultiplier((int)modifier);
        }
        
        /// <summary>
        /// Add a custom modifier to a spell.
        /// </summary>
        /// <param name="handler">Owner of this modifier</param>
        /// <param name="modifier">Hash of the modifier name</param>
        /// <param name="value">Modifier value</param>
        public void AddModifier(object handler, int modifier, float value)
        {
            if (!modifiers.ContainsKey(modifier))
                modifiers[modifier] = new Dictionary<object, float>();
            modifiers[modifier][handler] = value;
            RefreshMultiplier(modifier);
        }

        public void RefreshMultiplier(int modifier)
        {
            if (!modifiers.ContainsKey(modifier))
            {
                multipliers[modifier] = 1;
                return;
            }
            float output = 1;
            foreach (float mult in modifiers[modifier].Values)
            {
                output *= mult;
            }
            multipliers[modifier] = output;
        }

        public void RemoveModifier(object handler, Modifier modifier)
        {
            if (modifiers.TryGetValue((int)modifier, out var values) && values.Remove(handler))
            {
                RefreshMultiplier((int)modifier);
            }
        }

        public void RemoveModifier(object handler, int modifier)
        {
            if (modifiers.TryGetValue(modifier, out var values) && values.Remove(handler))
            {
                RefreshMultiplier(modifier);
            }
        }

        public void RemoveModifiers(object handler)
        {
            foreach (var kvp in modifiers)
            {
                if (!kvp.Value.Remove(handler)) continue;
                RefreshMultiplier(kvp.Key);
            }
        }

        // Don't do an Animator.StringToHash here for performance. The caller should cache their own modifier hashes.
        public float GetModifier(int modifierHash)
        {
            return multipliers.TryGetValue(modifierHash, out float value) ? value : 1;
        }

        public float GetModifier(Modifier modifier)
        {
            return multipliers.TryGetValue((int)modifier, out float value) ? value : 1;
        }

        public void ClearModifiers()
        {
            modifiers.Clear();
            multipliers.Clear();
        }

    }
}
