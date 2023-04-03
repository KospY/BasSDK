using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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

            [Button]
            public void SetAllExitAndEntrance()
            {
                IAreaBlueprintGenerator bpGenerator = bpGeneratorIdContainer.BlueprintGenerator;
                if (bpGenerator == null) return;
                List<AreaData.AreaConnection> connection = bpGenerator.GetConnections();

                if(connection == null)
                {
                    return;
                }
                exitConnection = new List<int>();
                entranceConnection = new List<int>();

                int count = connection.Count;
                for (int i = 0; i < count; i++)
                {
                    exitConnection.Add(i);
                    entranceConnection.Add(i);
                }
            }
        }

        [Serializable] public class AreaSettingsTable : DropTable<AreaSettings> { }

        public AreaSettingsTable areaSettingTable;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (areaSettingTable != null) areaSettingTable.CalculateWeight();
        }        
    }
}