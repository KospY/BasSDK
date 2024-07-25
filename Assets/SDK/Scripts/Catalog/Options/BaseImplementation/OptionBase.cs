using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
	public abstract class OptionBase
	{
		public string name; //id of the option

#if ODIN_INSPECTOR
		[ValueDropdown(nameof(GetAllTextId))]
#endif
		public string displayName;
#if ODIN_INSPECTOR
		[ValueDropdown(nameof(GetAllTextId))]
#endif
		public string description;

        public virtual bool IsHidden() => false;
		public virtual bool IsLevelOption() => false;
		public abstract OptionValue DefaultValue();
		public abstract OptionValue CurrentValue();
		public abstract void SetValue(OptionValue preset);
		public abstract string GetCurrentValueLabel();
		public abstract string GetDefaultValueLabel();


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextId()
		{
			return Catalog.GetTextData().GetDropdownAllTexts(TextData.DEFAULT_TEXT_GROUP);
		}
#endif
	}

	public abstract class OptionBase<T> : OptionBase, IEquatable<OptionBase<T>> where T : OptionValue, new()
    {
        public bool Equals(OptionBase<T> other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return name == other.name;
		}
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((OptionBase<T>)obj);
		}
		public override int GetHashCode()
		{
			return (name != null ? name.GetHashCode() : 0);
		}
		public static bool operator ==(OptionBase<T> left, OptionBase<T> right)
		{
			return Equals(left, right);
		}
		public static bool operator !=(OptionBase<T> left, OptionBase<T> right)
		{
			return !Equals(left, right);
        }
    }
}
