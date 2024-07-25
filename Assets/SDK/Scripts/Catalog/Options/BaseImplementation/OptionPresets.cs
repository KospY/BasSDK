using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
namespace ThunderRoad
{
	[Serializable]
	public abstract class OptionValue
	{
		protected OptionValue()
		{ }
		public abstract object Value();
	}
	[Serializable]
	public class OptionBooleanValue : OptionValue
	{
		public OptionBooleanValue()
		{ }
		public OptionBooleanValue(bool value) : base()
		{
			this.value = value;
		}
		public bool value;
		public override object Value() => value;
	}
	[Serializable]
	public class OptionStringValue : OptionValue
	{
		public OptionStringValue()
		{ }
		public OptionStringValue(string value) : base()
		{
			this.value = value;
		}
		public string value;
		public override object Value() => value;
	}
	[Serializable]
	public class OptionIntValue : OptionValue
	{
		public OptionIntValue()
		{ }
		public OptionIntValue(int value) : base()
		{
			this.value = value;
		}
		public int value;
		public override object Value() => value;
	}
	[Serializable]
	public class OptionFloatValue : OptionValue
	{
		public OptionFloatValue()
		{ }
		public OptionFloatValue(float value) : base()
		{ this.value = value; }
		public float value;
		public override object Value() => value;
	}

	[Serializable]
	public class OptionEnumValue<T> : OptionIntValue where T : Enum
	{
		public OptionEnumValue()
		{ }
		public OptionEnumValue(T value) : base(Convert.ToInt32(value))
		{
			this.enumValue = value;
		}
		private T _enumValue;
#if ODIN_INSPECTOR		
		[ShowInInspector]
#endif		
		public T enumValue
		{
			get => _enumValue;
			set { _enumValue = value; this.value = Convert.ToInt32(_enumValue);}
		}

		public override object Value() => _enumValue;
	}
	
	
	[Serializable]
	public class OptionPresets : UnitySerializedDictionary<string, OptionValue>{}
}
