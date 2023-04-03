using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class EffectBundle
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID"), HideLabel, HorizontalGroup("Effect", 0.5f)] 
#endif
        public string effectId;
        [NonSerialized]
        public EffectData effectData;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffetModuleTypes"), LabelText("Ignored module(s)"), HorizontalGroup("Effect")] 
#endif
        public List<string> ignoredEffects;
        [NonSerialized]
        public Type[] ignoredEffectTypes;

        public EffectBundle Clone()
        {
            EffectBundle clone = MemberwiseClone() as EffectBundle;
            clone.ignoredEffects = new List<string>(ignoredEffects);
            return clone;
        }

        public void OnCatalogRefresh()
        {
            if (effectId != null && effectId != "") effectData = Catalog.GetData<EffectData>(effectId);
            if (ignoredEffects != null)
            {
                List<Type> types = new List<Type>();
                foreach (string ignoreEffect in ignoredEffects)
                {
                    types.Add(Type.GetType(ignoreEffect));
                }
                ignoredEffectTypes = types.ToArray();
            }
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffetModuleTypes()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("None", null));
            foreach (Type type in typeof(EffectModule).Assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(EffectModule))) dropdownList.Add(new ValueDropdownItem<string>(type.ToString(), type.ToString()));
            }
            return dropdownList;
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        } 
#endif
    }
}
