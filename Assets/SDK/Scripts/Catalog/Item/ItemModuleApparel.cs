namespace ThunderRoad
{
	public enum ApparelModuleType
    {
        Head,
        Torso,
        HandLeft,
        HandRight,
        Legs,
        Feet,
        Global
    }
    public class ItemModuleApparel : ItemModule
    {
        public virtual void OnEquip(Creature creature, ApparelModuleType equippedOn, ItemModuleWardrobe.CreatureWardrobe wardrobeData) {}
        public virtual void OnUnequip(Creature creature, ApparelModuleType equippedOn, ItemModuleWardrobe.CreatureWardrobe wardrobeData) {}
    }
}
