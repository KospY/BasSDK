namespace ThunderRoad
{
	public class EnableSandboxBooksOption : OptionBoolean
	{
		/// <summary>
		/// This is primarily used for dungeon arenas to dynamically enable the books at runtime
		/// </summary>
		public EnableSandboxBooksOption()
		{
			name = "SandboxItemAndWaveBook"; //dont change the name, a component in dungeons is looking for this
			displayName = "Enable Sandbox Books";
			description = "Enable Sandbox Wave and Item books";
			defaultValue = true;
		}
		public override bool IsHidden() => true;
		public override bool IsLevelOption() => true;
	}
}
