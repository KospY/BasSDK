using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class EntityData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Entity Modules"), ShowInInspector, ValueDropdown(nameof(GetAllEntityModuleIDs))]
#endif
        public List<string> entityModules;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEntityModuleIDs()
        {
            return Catalog.GetDropdownAllID(Category.EntityModule);
        }
#endif

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            entityModules ??= new List<string>();
        }

        public void Load(ThunderEntity entity)
        {
            for (var i = 0; i < entityModules.Count; i++)
            {
                var module = Catalog.GetData<EntityModule>(entityModules[i]).Clone() as EntityModule;
                if (module is not EntityModule) continue;
                module.Load(entity);
                entity.entityModules.Add(module);
            }
        }
    }
}