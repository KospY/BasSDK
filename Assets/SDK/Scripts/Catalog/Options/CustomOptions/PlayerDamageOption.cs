using System;
using System.Globalization;
using UnityEngine;

namespace ThunderRoad
{
	public class PlayerDamageOption : Option, IGameModeOption
	{
		public PlayerDamageOption()
		{
			name = LevelOption.PlayerDamageMultiplier.Get();
			displayName = "Player Damage Multiplier";
			description = "Multiplier for player damage done to enemies";
			minValue = 0;
			maxValue = 200;
			defaultValue = 100;
            currentValue = defaultValue; //ensure that the default isnt 0, since that will do no damage
		}
 // ProjectCore        
    }
}
