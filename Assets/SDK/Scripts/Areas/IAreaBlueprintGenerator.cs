namespace ThunderRoad
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAreaBlueprintGenerator

    {
        #region InternalClass

        [System.Serializable]
        public class IAreaBlueprintGeneratorIdContainer : DataIdContainerChoice
        {
            public IAreaBlueprintGeneratorIdContainer(string id, ThunderRoad.Category category) : base(id, category)
            {
            }

            [JsonIgnore]
            public IAreaBlueprintGenerator BlueprintGenerator
            {
                get
                {
                    if (Data is IAreaBlueprintGenerator bpGenerator)
                    {
                        return bpGenerator;
                    }

                    return null;
                }
            }

            public override ThunderRoad.Category[] GetCategoryAllowed()
            {
                return new ThunderRoad.Category[] { Category.Area, Category.AreaCollection };
            }
        }

        public class SpawnableBlueprint
        {
            protected AreaRotationHelper.Rotation _rotation;
            public AreaRotationHelper.Rotation Rotation { get { return _rotation; } }
            public SpawnableBlueprint(AreaRotationHelper.Rotation rotation)
            {
                _rotation = rotation;
            }

            public virtual SpawnableArea GetRoot()
            {
                return null;
            }

            public virtual bool Intersects(Bounds bounds, float margin)
            {
                return false;
            }
        }
        #endregion InternalClass
        public string GetId();
        public List<AreaData.AreaConnection> GetConnections();
        public AreaData.AreaConnection GetConnection(int index);

        public List<AreaRotationHelper.Rotation> GetAllowedRotation();
        public bool IsSpawnable();

        public SpawnableArea GetSpawnableArea(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition,
                                                        int numberCreature,
                                                        bool isShareNPCAlert
                                                        );

        public SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition);

        public void SetCreatureDataToSpawnableArea(SpawnableBlueprint spawnableBp,
                                                    int numberCreature,
                                                    bool isShareNPCAlert);

#if UNITY_EDITOR
        public void PreviewAreaWithItemAndDisableCondition();
        public void PreviewArea(bool spawnItem = false);

#endif // UNITY_EDITOR

    }
}