namespace ThunderRoad
{
	public class EnemyConfigOption : OptionString
	{
		public EnemyConfigOption()
		{
			name = LevelOption.EnemyConfig.Get();
			displayName = "Enemy Config";
			description = "Choose the enemy config";
			defaultIntValue = 0;
			currentIntValue = defaultIntValue;
		}
 // ProjectCore    
	}

}
