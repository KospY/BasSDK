using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class AreaTable : CatalogData
    {
        [Serializable]
        public class AreaSettings
        {
            public IAreaBlueprintGenerator.IAreaBlueprintGeneratorIdContainer bpGeneratorIdContainer;

            public List<int> exitConnection;
            public List<int> entranceConnection;
            public int maxCreature = 0;

        }

        [Serializable] public class AreaSettingsTable : DropTable<AreaSettings> { }

        public AreaSettingsTable areaSettingTable;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
        }

    }
}