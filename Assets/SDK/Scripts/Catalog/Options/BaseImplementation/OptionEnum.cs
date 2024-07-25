using System;

namespace ThunderRoad
{
	public abstract class OptionEnumInt : OptionBase<OptionIntValue>
	{
		protected int defaultIntValue;
		protected int currentIntValue;
		public override OptionValue DefaultValue() => new OptionIntValue { value = defaultIntValue };
		public override OptionValue CurrentValue() => new OptionIntValue { value = currentIntValue };
		public override void SetValue(OptionValue preset)
		{
			if(preset is OptionIntValue intPreset)
			{
				currentIntValue = intPreset.value;
			}
		}
		public abstract Type GetEnumType();

		public virtual int GetEnumIndex(int enumInt)
        {
			var values = System.Enum.GetValues(GetEnumType());
            for (int i = 0; i < values.Length; i++)
            {
				int tempEnumInt = (int)values.GetValue(i);
				if(tempEnumInt == enumInt)
                {
					return i;
                }				
            }

			return -1;
		}

		public virtual int GetDefaultEnumIndex()
        {
			return GetEnumIndex(defaultIntValue);
        }public virtual int GetCurrentEnumIndex()
        {
			return GetEnumIndex(currentIntValue);
        }

		public virtual int GetEnumValueCount()
        {
			return System.Enum.GetValues(GetEnumType()).Length;
		}

		public virtual void SetEnumValueFromIndex(int index)
		{
			var values = System.Enum.GetValues(GetEnumType());
			int tempValue = (int)values.GetValue(index);
			SetValue(new OptionIntValue(tempValue));
		}
	}
	
	[Serializable]
	public abstract class OptionEnum<T> : OptionEnumInt where T : struct, Enum
	{
		public override bool IsHidden() => false; // should be visible by default
		public override bool IsLevelOption() => true; // should be set as level option by default
		
		public T defaultValue;
		private T currentValue;

		public override OptionValue DefaultValue()
		{
			return new OptionEnumValue<T>(defaultValue);
		}

		public override OptionValue CurrentValue()
		{
			return new OptionEnumValue<T>(currentValue);
		}

		public override Type GetEnumType()
		{
			return typeof(T);
		}

		public override void SetValue(OptionValue preset)
		{
			if (preset is OptionIntValue intPreset)
			{
				if (preset is OptionEnumValue<T> enumPreset)
				{
					currentValue = enumPreset.enumValue;
					currentIntValue = intPreset.value;
				}
				else
				{
					//check the int value is in range for the enum
					if (Enum.IsDefined(typeof(T), intPreset.value))
					{
						currentValue = (T)Enum.ToObject(typeof(T), intPreset.value);
						currentIntValue = intPreset.value;
					}
				}
			}			
		}
		public override string GetCurrentValueLabel()
		{
			return currentValue.ToString();
		}
		public override string GetDefaultValueLabel()
		{
			return defaultValue.ToString();
		}
	}
}
