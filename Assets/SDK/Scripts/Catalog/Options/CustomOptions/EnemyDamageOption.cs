using System.Globalization;

namespace ThunderRoad
{
	public class EnemyDamageOption : Option, IGameModeOption
	{
		public EnemyDamageOption()
		{
			name = LevelOption.EnemyDamageMultiplier.Get();
			displayName = "Enemy Damage Multiplier";
			description = "Multiplier for Enemy damage done to player";
			minValue = 0;
			maxValue = 200;
			defaultValue = 100;
            currentValue = defaultValue; //ensure that the default isnt 0, since that will do no damage
		}
 // ProjectCore        
	}
}
