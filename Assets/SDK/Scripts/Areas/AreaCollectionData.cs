namespace ThunderRoad
{
    using System.Collections.Generic;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#else
    using TriInspector;
#endif

    public abstract class AreaCollectionData : CatalogData, IAreaBlueprintGenerator
    {
        public string GetId()
        {
            return id;
        }

        public abstract HashSet<string> GetSpawnableAreasIds();
        public abstract List<AreaData.AreaConnection> GetConnections();
        public abstract AreaData.AreaConnection GetConnection(int index);

        public abstract bool IsSpawnable();

        public abstract List<AreaRotationHelper.Rotation> GetAllowedRotation();

        public virtual SpawnableArea GetSpawnableArea(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition,
                                                        int numberCreature,
                                                        bool isShareNPCAlert
                                                        )
        {
            IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp = GetSpawnableBluePrint(rotation, entranceIndex, entrancePosition);
            SetCreatureDataToSpawnableArea(spawnableBp, numberCreature, isShareNPCAlert);
            return spawnableBp.GetRoot();
        }


        public abstract IAreaBlueprintGenerator.SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation,
                                                                                int entranceIndex,
                                                                                Vector3 entrancePosition);

        public abstract void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp,
                                                    int numberCreature,
                                                    bool isShareNPCAlert);

#if UNITY_EDITOR
        public void PreviewAreaAndDisableCondition()
        {
            PreviewAreaWithItemAndDisableCondition(false);
        }

        public void PreviewAreaWithItemAndDisableCondition()
        {
            PreviewAreaWithItemAndDisableCondition(true);
        }

        public virtual void PreviewAreaWithItemAndDisableCondition(bool spawnItem = false)
        {
        }
        
        public virtual void PreviewArea(bool spawnItem = false)
        {
        }

#endif //UNITY_EDITOR
    }
}