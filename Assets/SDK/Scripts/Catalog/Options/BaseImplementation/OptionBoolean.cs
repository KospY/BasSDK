using System;

namespace ThunderRoad
{
	[Serializable]
	public class OptionBoolean : OptionBase<OptionBooleanValue>
	{
		public override bool IsHidden() => false; // should be visible by default
		public override bool IsLevelOption() => true; // should be set as level option by default
		
		public bool defaultValue;
		protected bool currentValue;
		public OptionBoolean()
		{ }
		public override OptionValue DefaultValue() => new OptionBooleanValue { value = defaultValue };
		
		public override OptionValue CurrentValue() => new OptionBooleanValue { value = currentValue };
		
		public override void SetValue(OptionValue preset)
		{
			if(preset is OptionBooleanValue booleanPreset)
			{
				currentValue = booleanPreset.value;
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
