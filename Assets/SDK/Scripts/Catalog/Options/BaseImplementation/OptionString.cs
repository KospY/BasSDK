using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public abstract class OptionStringBase : OptionBase<OptionIntValue>
    {
        protected int defaultIntValue;
        protected int currentIntValue;

        public abstract int StringCount();

        public abstract string GetString(int index);

        
        public virtual OptionStringValue DefaultStringValue()
        {
            string defaultValue = GetString(defaultIntValue);
            return new OptionStringValue(defaultValue);
        }

        public virtual OptionStringValue CurrentStringValue()
        {
            string currentValue = GetString(currentIntValue);
            return new OptionStringValue(currentValue);
        }
        
        public override string GetCurrentValueLabel()
        {
	        return GetString(currentIntValue);
        }
        
        public override string GetDefaultValueLabel()
        {
	        return GetString(defaultIntValue);
        }

        public override OptionValue DefaultValue()
        {
            return new OptionIntValue(defaultIntValue);
        }

        public override OptionValue CurrentValue()
        {
            return new OptionIntValue(currentIntValue);
        }

        public override void SetValue(OptionValue preset)
        {
            if (preset is OptionIntValue intPreset)
            {
                currentIntValue = intPreset.value;
                //clamp to min/max
                currentIntValue = Math.Clamp(currentIntValue, 0, StringCount());
            }
        }
    }

    [Serializable]
    public class OptionStringLabel : OptionString
    {
	    public List<string> labelList = new List<string>();
	    
	    public override string GetCurrentValueLabel()
	    {
		    if (currentIntValue >= 0 && currentIntValue < StringCount())
		    {
			    return labelList[currentIntValue];
		    }
		    return $"INVALID[{currentIntValue}]";
	    }
        
	    public override string GetDefaultValueLabel()
	    {
		    if (defaultIntValue >= 0 && defaultIntValue < StringCount())
		    {
			    return labelList[defaultIntValue];
		    }
		    return $"INVALID[{defaultIntValue}]";
	    }
	    
#if UNITY_EDITOR	    

	    [Button]
	    public override void AddAllValueFromCatalogType()
	    {
		    valueList ??= new List<string>();
		    valueList.AddRange(Catalog.GetAllID(catalogType));
		    foreach (var value in valueList)
		    {
			    if (Catalog.TryGetData<CatalogData>(value, out CatalogData catalogData))
			    {
				    labelList.Add(catalogData.id);
			    }
		    }
	    }
#endif
	    
    }
    
    [Serializable]
    public class OptionString : OptionStringBase
    {
	    public override bool IsHidden() => false; // should be visible by default
	    public override bool IsLevelOption() => true; // should be set as level option by default
	    
	    public List<string> valueList = new List<string>();
	    
	    
#if UNITY_EDITOR	    
        [JsonIgnore]
        public Category catalogType;
        [Button]
        public virtual void AddAllValueFromCatalogType()
        {
            valueList ??= new List<string>();
            valueList.AddRange(Catalog.GetAllID(catalogType));
        }
#endif
        public override string GetString(int index)
        {
            if (index >= 0 && index < StringCount())
            {
                return valueList[index];
            }
            return $"INVALID[{index}]";
        }
        
        public override int StringCount() => valueList != null ? valueList.Count : 0;

        public override void SetValue(OptionValue preset)
        {
            valueList ??= new List<string>();
            base.SetValue(preset);
        }
    }
}
