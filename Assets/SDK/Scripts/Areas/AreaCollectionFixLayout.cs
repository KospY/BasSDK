using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class AreaCollectionFixLayout : AreaCollectionData
    {
        #region InternalClass
        public class AreaCollectionFixLayoutBlueprint : IAreaBlueprintGenerator.SpawnableBlueprint
        {
            public int areaCount;
            public IAreaBlueprintGenerator[] areaBpGeneratorArray;
            public IAreaBlueprintGenerator.SpawnableBlueprint[] spawnableBp;
            public int entranceAreaIndex;
            public float sumCreatureNumberWeight;

            public AreaCollectionFixLayoutBlueprint(AreaRotationHelper.Rotation rotation, int areaCount) : base(rotation)
            {
                this.areaCount = areaCount;
                areaBpGeneratorArray = new AreaData[areaCount];
                spawnableBp = new IAreaBlueprintGenerator.SpawnableBlueprint[areaCount];
                sumCreatureNumberWeight = 0;
            }

            public override SpawnableArea GetRoot()
            {
                return spawnableBp[entranceAreaIndex].GetRoot();
            }

            public override bool Intersects(Bounds bounds, float margin)
            {
                for (int i = 0; i < areaCount; i++)
                {
                    if (spawnableBp[i].Intersects(bounds, margin))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        [Serializable]
        public class InternalLink
        {
            public int indexArea1;
            public int indexConnectionArea1;

            public int indexArea2;
            public int indexConnectionArea2;

            public bool crossAreaAlert;
        }

        [Serializable] public class AreaIdContainer : DataIdContainer<AreaData> { }
        [Serializable]
        public class AreaLayout
        {
            public IAreaBlueprintGenerator.IAreaBlueprintGeneratorIdContainer bpGeneratorId;
            public AreaRotationHelper.Rotation rotation;
            public float creatureNumberWeight = 1.0f;
            public bool isShareNPCAlert = true;
        }

        [Serializable]
        public class ExternalConnection
        {
            public int indexArea;
            public int indexConnection;

            public ExternalConnection(int indexArea, int indexConnection)
            {
                this.indexArea = indexArea;
                this.indexConnection = indexConnection;
            }
        }
        #endregion InternalClass

        #region Data
        public List<AreaRotationHelper.Rotation> allowedRotation;
        public AreaLayout[] layout;
        public InternalLink[] internalLinks;
        public List<ExternalConnection> externalConnections;

        public float distanceBetweenConnectionMax = 0.01f;
        #endregion Data

        #region NonSerializedFields
        private List<AreaData.AreaConnection> _externalConnectionList = null;
        #endregion NonSerializedFields

        #region Methods

        public override HashSet<string> GetSpawnableAreasIds()
        {
            var areaIds = new HashSet<string>();

            return areaIds;
        }

        public override List<AreaData.AreaConnection> GetConnections()
        {
return default;
        }

        public override AreaData.AreaConnection GetConnection(int index)
        {
return default;
        }

        public override bool IsSpawnable()
        {

            return true;
        }

        public override List<AreaRotationHelper.Rotation> GetAllowedRotation()
        {
            return allowedRotation;
        }

        public override IAreaBlueprintGenerator.SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition)
        {
return default;
        }

        public override void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp, int numberCreature, bool isShareNPCAlert)
        {
        }
#endregion Methods

        #region Tools
 //UNITY_EDITOR
#endregion Tools
    }
}