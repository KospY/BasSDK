using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
	public class ItemModuleStats : ItemModule
	{
		
		public List<IStats> stats = new List<IStats>();

		public bool TryGetStat(string id, out IStats stat)
		{
			stat = GetStat(id);
			return stat != null;
		}
		
		public IStats GetStat(string id)
		{
return null;
		}
#if ODIN_INSPECTOR
#if UNITY_EDITOR
		public enum ItemStatCollection
		{
			Weapon,
			Armor,
			Food,
			Bow,
			Quiver,
			Loot,
			Tool
		}
		[NonSerialized]
		[OnValueChanged(nameof(AddPresets))]
		public ItemStatCollection itemStatCollection;
		
		public void AddPresets()
		{
			stats.Clear();
			switch (itemStatCollection)
			{
				case ItemStatCollection.Weapon:
					stats.Add(new ItemStatInt("FleshDamage", 0));
					stats.Add(new ItemStatInt("LeatherDamage", 0));
					stats.Add(new ItemStatInt("PlateDamage", 0));
					stats.Add(new ItemStatInt("Penetration", 0));
					stats.Add(new ItemStatInt("Dismemberment", 0));
					stats.Add(new ItemStatInt("Stagger", 0));
					stats.Add(new ItemStatInt("Imbue", 0));
					stats.Add(new ItemStatInt("Handling", 0));
					break;
				case ItemStatCollection.Armor:
					stats.Add(new ItemStatInt("Defense", 0));
					stats.Add(new ItemStatInt("Mobility", 0));
					stats.Add(new ItemStatInt("SpellCasting", 0));
					stats.Add(new ItemStatInt("FocusRegen", 0));
					break;
				case ItemStatCollection.Food:
					stats.Add(new ItemStatInt("Healing", 0));
					break;
				case ItemStatCollection.Bow:
					stats.Add(new ItemStatInt("DrawPower", 0));
					break;
				case ItemStatCollection.Quiver:
					stats.Add(new ItemStatInt("HoldAmount", 0));
					break;
				case ItemStatCollection.Loot:
					stats.Add(new ItemStatFloat("Value", 0));
					break;
				case ItemStatCollection.Tool:
					break;
				
			}
		}
#endif
#endif
		
	}
}
