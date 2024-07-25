using System;
using UnityEngine;

namespace ThunderRoad
{
	[Serializable]
	public class TelekinesisOption : OptionBoolean, IGameModeOption
	{
       
		public TelekinesisOption()
		{
			name = "Telekinesis";
			displayName = "Telekinesis";
			description = "Grab objects at a distance";
		}
 // ProjectCore  
	}
  
}
