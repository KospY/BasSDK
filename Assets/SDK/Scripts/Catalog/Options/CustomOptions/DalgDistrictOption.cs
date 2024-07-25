namespace ThunderRoad
{
    public class DalgDistrictOption : OptionString
    {
        public DalgDistrictOption()
        {
            //we actually use the dungen length option for this
            name = LevelOption.DungeonLength.Get();
            displayName = "District";
            description = "Area of the dungeon";
            defaultIntValue = 0;
            currentIntValue = defaultIntValue;
        }
 // ProjectCore
    }
}
