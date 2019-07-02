using BS;

namespace BasPluginExample
{
    // This create an item module that can be referenced in the item JSON
    public class ItemModuleShock : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemShock>();
        }
    }
}
