using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR

namespace ThunderRoad
{
	/// <summary>
	/// This is a collection of options for a gamemode to define its difficulty
	/// </summary>
	[Serializable]
	public class DifficultyPreset
	{
		public string name;
#if UNITY_EDITOR
#if ODIN_INSPECTOR
		[ValueDropdown(nameof(GetAllTextGroupID))]
#endif
#endif //ODIN_INSPECTOR
		public string localizationGroupId;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
		[ValueDropdown(nameof(GetAllTextId))]
#endif
#endif //ODIN_INSPECTOR
		public string localizationId;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
		[ValueDropdown(nameof(GetAllTextId))]
#endif
#endif //ODIN_INSPECTOR
		public string localizationDescriptionId;

		[NonSerialized]
		public GameModeData gameModeData;

        //this needs to hold difficulty options, along with the value they should be set to
#if ODIN_INSPECTOR
        [DictionaryDrawerSettings(KeyLabel = "Option ID", ValueLabel = "Option Value")]
		[ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true, ShowPaging = false), LabelText("Drop Levels")]
#endif
        public OptionPresets optionPresets = new OptionPresets();

		[Button]
		public void Refresh()
		{
			if (gameModeData == null) return;
			foreach (OptionBase option in gameModeData.difficultyOptions)
			{
				//add the option to the list if it doesn't exist
				if (!optionPresets.ContainsKey(option.name))
				{
					//check the type of the option and add the default value
					OptionValue defaultValue = null;
					if (option is Option optionInt)
					{
						defaultValue = optionInt.DefaultValue();
					}
					else if (option is OptionBoolean optionBoolean)
					{
						defaultValue = optionBoolean.DefaultValue();
					}
					else if (option is OptionEnumInt optionEnum)
					{
						defaultValue = optionEnum.DefaultValue();
					}
					optionPresets.Add(option.name, defaultValue);
				}

			}
		}

#if UNITY_EDITOR
#if ODIN_INSPECTOR
		[Button]
		public void RemoveThisLevel()
		{
			gameModeData.difficultyPresets.Remove(this);
		}
		public List<ValueDropdownItem<string>> GetAllTextGroupID()
		{
			return Catalog.GetTextData().GetDropdownAllTextGroups();
		}

		public List<ValueDropdownItem<string>> GetAllTextId()
		{
			return Catalog.GetTextData().GetDropdownAllTexts(localizationGroupId);
		}
#endif
#endif
	}
}
