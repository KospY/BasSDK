using System;
using UnityEngine;

namespace ThunderRoad
{
	[Serializable]
	public class FreeClimbOption : OptionBoolean, IGameModeOption
	{
		
		public FreeClimbOption()
		{
			name = "FreeClimb";
			displayName = "Free Climb";
			description = "Climb anywhere you want";
		}
 // ProjectCore
	}
}
