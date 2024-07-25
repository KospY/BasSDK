using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Modules;
using static ThunderRoad.Modules.CrystalHuntLevelInstancesModule;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	
	[Serializable]
	public class CrystalHuntLootTable : LootTableBase, IContainerLoadable<CrystalHuntLootTable>
	{
		[Serializable]
		public class LootTypeLootTableDictionary : UnitySerializedDictionary<LootType,LootTableBase> { }
		[Serializable]
		public class LootTypeStringDictionary : UnitySerializedDictionary<LootType,string> { }

		//This is a loot table which is essentially a wrapper for a list of loot tables
		//They get split up based on the CrystalHunt level LootType
		
#if ODIN_INSPECTOR
		[ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true, ShowPaging = false), LabelText("Crystal Hunt Level Loot Types")]
#endif
		public LootTypeStringDictionary lootTypes = new LootTypeStringDictionary
		{
			{ LootType.Loot, null },
			{ LootType.Shard, null },
			{ LootType.Arena, null},
			{ LootType.Dalgarian, null },
		};
		
		private LootTypeLootTableDictionary lootTypeLootTables = new LootTypeLootTableDictionary();
		
		public override IEnumerator OnCatalogRefreshCoroutine()
		{
			foreach (var lootType in lootTypes)
			{
				if(string.IsNullOrEmpty(lootType.Value)) continue;
				if(Catalog.TryGetData<LootTableBase>(lootType.Value, out LootTableBase lootTable))
				{
					lootTypeLootTables[lootType.Key] = lootTable;
				}
				
			}
			return base.OnCatalogRefreshCoroutine();
		}

		public override int GetCurrentVersion()
		{
			return 1;
		}

		public override void RenameItem(string itemId, string newName)
		{
			foreach (var lootTable in lootTypeLootTables)
			{
				if(lootTable.Value == null) continue;
				lootTable.Value.RenameItem(itemId, newName);
			}
		}
		public override ItemData PickOne(int level = 0, int searchDepth = 0, Random randomGen = null)
		{
			//check the current levels level instance data to get the loot type
			if(TryGetCurrentLootType(out LootType lootType))
			{
				//got the loot type, now get the loot table
				if(lootTypeLootTables.TryGetValue(lootType, out LootTableBase lootTable))
				{
					//got the loot table, now pick one
					return lootTable.PickOne(level, searchDepth, randomGen);
				}
			}
			return base.PickOne(level, searchDepth, randomGen);
		}
		public override List<ItemData> Pick(int level = 0, int searchDepth = 0, Random randomGen = null)
		{
			//check the current levels level instance data to get the loot type
			if(TryGetCurrentLootType(out LootType lootType))
			{
				//got the loot type, now get the loot table
				if(lootTypeLootTables.TryGetValue(lootType, out LootTableBase lootTable))
				{
					//got the loot table, now pick one
					return lootTable.Pick(level, searchDepth, randomGen);
				}
			}
			return base.Pick(level, searchDepth, randomGen);
		}
		public override List<ItemData> GetAll(int level = -1, int pickCount = 0)
		{
			//check the current levels level instance data to get the loot type
			if(TryGetCurrentLootType(out LootType lootType))
			{
				//got the loot type, now get the loot table
				if(lootTypeLootTables.TryGetValue(lootType, out LootTableBase lootTable))
				{
					//got the loot table, now pick one
					return lootTable.GetAll(level, pickCount);
				}
			}
			return base.GetAll(level, pickCount);
		}

		private bool TryGetCurrentLootType(out LootType lootType)
		{
			lootType = LootType.Loot;
			return false;
		}
		public void OnLoadedFromContainer(Container container)
		{
			throw new NotImplementedException();
		}
		public ContainerContent InstanceContent()
		{
			throw new NotImplementedException();
		}
	}
}
