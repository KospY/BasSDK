using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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
        public override List<AreaData.AreaConnection> GetConnections()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                int connectionCount = externalConnections.Count;
                List<AreaData.AreaConnection> externalConnectionList = new List<AreaData.AreaConnection>(connectionCount);

                for (int i = 0; i < connectionCount; i++)
                {
                    IAreaBlueprintGenerator tempBpGenerator = layout[externalConnections[i].indexArea].bpGeneratorId.BlueprintGenerator;
                    if (tempBpGenerator == null)
                    {
                        Debug.LogError("Area config can not find generetor for id : " + layout[externalConnections[i].indexArea].bpGeneratorId.dataId);
                        return null;
                    }

                    AreaData.AreaConnection tempAreaConnection = tempBpGenerator.GetConnection(externalConnections[i].indexConnection);
                    externalConnectionList.Add(tempAreaConnection);
                }

                return externalConnectionList;
            }
#endif //UNITY_EDITOR

            if (_externalConnectionList == null)
            {
                int connectionCount = externalConnections.Count;
                _externalConnectionList = new List<AreaData.AreaConnection>(connectionCount);

                for (int i = 0; i < connectionCount; i++)
                {
                    IAreaBlueprintGenerator tempBpGenerator = layout[externalConnections[i].indexArea].bpGeneratorId.BlueprintGenerator;
                    if (tempBpGenerator == null)
                    {
                        Debug.LogError("Area config can not find generetor for id : " + layout[externalConnections[i].indexArea].bpGeneratorId.dataId);
                        return null;
                    }

                    AreaData.AreaConnection tempAreaConnection = tempBpGenerator.GetConnection(externalConnections[i].indexConnection);
                    _externalConnectionList.Add(tempAreaConnection);
                }
            }

            return _externalConnectionList;
        }

        public override AreaData.AreaConnection GetConnection(int index)
        {
            List<AreaData.AreaConnection> externalConnectionList = GetConnections();
            if (index < 0 || index >= externalConnectionList.Count) return null;
            return externalConnectionList[index];
        }

        public override bool IsSpawnable()
        {
            for (int i = 0; i < layout.Length; i++)
            {
                IAreaBlueprintGenerator tempBpGenerator = layout[i].bpGeneratorId.BlueprintGenerator;
                if (tempBpGenerator == null) return false;
                if (!tempBpGenerator.IsSpawnable())
                {
                    return false;
                }
            }

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
            AreaCollectionFixLayoutBlueprint bluePrint = new AreaCollectionFixLayoutBlueprint(rotation, layout.Length);

            for (int i = 0; i < bluePrint.areaCount; i++)
            {
                bluePrint.areaBpGeneratorArray[i] = layout[i].bpGeneratorId.BlueprintGenerator;
            }

            int entranceAreaConnectionIndex = entranceIndex;
            if (entranceIndex >= 0)
            {
                if (entranceIndex > externalConnections.Count)
                {
                    Debug.LogError("Can not find entrance area for Composed area : " + this.id
                        + " With entrance index : " + entranceIndex);
                    return null;
                }

                bluePrint.entranceAreaIndex = externalConnections[entranceIndex].indexArea;
                entranceAreaConnectionIndex = externalConnections[entranceIndex].indexConnection;
            }

            AreaLayout configFirst = layout[bluePrint.entranceAreaIndex];
            AreaRotationHelper.Rotation rotationFirst = AreaRotationHelper.RotateRotation(configFirst.rotation, rotation);
            bluePrint.spawnableBp[bluePrint.entranceAreaIndex] = bluePrint.areaBpGeneratorArray[bluePrint.entranceAreaIndex].GetSpawnableBluePrint(rotationFirst, entranceAreaConnectionIndex, entrancePosition);
            bluePrint.sumCreatureNumberWeight += configFirst.creatureNumberWeight;

            List<InternalLink> tempLinkList = new List<InternalLink>();
            tempLinkList.AddRange(internalLinks);

            while (tempLinkList.Count > 0)
            {
                int count = tempLinkList.Count;
                for (int i = 0; i < tempLinkList.Count; i++)
                {
                    InternalLink tempLink = tempLinkList[i];
                    SpawnableArea spawnableArea1 = bluePrint.spawnableBp[tempLink.indexArea1]?.GetRoot();
                    SpawnableArea spawnableArea2 = bluePrint.spawnableBp[tempLink.indexArea2]?.GetRoot();

                    string area1Id = layout[tempLink.indexArea1].bpGeneratorId.dataId;
                    string area2Id = layout[tempLink.indexArea2].bpGeneratorId.dataId;

                    if (spawnableArea1 != null && spawnableArea2 != null)
                    {
                        // Check if link is ok
                        Vector3 connectionPositionArea1 = spawnableArea1.GetConnectionPosition(area1Id, tempLink.indexConnectionArea1);
                        Vector3 connectionPositionArea2 = spawnableArea2.GetConnectionPosition(area2Id, tempLink.indexConnectionArea2);

                        if (Vector3.Distance(connectionPositionArea1, connectionPositionArea2) > distanceBetweenConnectionMax)
                        {
                            Debug.LogError("ComposedArea id : " + this.id
                                + " connection between area : " + tempLink.indexArea1
                                + " and area : " + tempLink.indexArea2
                                + " is wrong, position doesn't match");
                            return null;
                        }

                        SpawnableArea.ConnectAreas(spawnableArea1, area1Id, tempLink.indexConnectionArea1, spawnableArea2, area2Id, tempLink.indexConnectionArea2, tempLink.crossAreaAlert);
                    }
                    else if (spawnableArea1 != null)
                    {
                        // Create spawnableArea2
                        Vector3 existingConnectionPosition = spawnableArea1.GetConnectionPosition(area1Id, tempLink.indexConnectionArea1);
                        AreaLayout tempConfig = layout[tempLink.indexArea2];
                        AreaRotationHelper.Rotation tempRotation = AreaRotationHelper.RotateRotation(tempConfig.rotation, rotation);
                        bluePrint.spawnableBp[tempLink.indexArea2] = bluePrint.areaBpGeneratorArray[tempLink.indexArea2].GetSpawnableBluePrint(tempRotation, tempLink.indexConnectionArea2, existingConnectionPosition);
                        spawnableArea2 = bluePrint.spawnableBp[tempLink.indexArea2].GetRoot();
                        bluePrint.sumCreatureNumberWeight += tempConfig.creatureNumberWeight;

                        SpawnableArea.ConnectAreas(spawnableArea1, area1Id, tempLink.indexConnectionArea1, spawnableArea2, area2Id, tempLink.indexConnectionArea2, tempLink.crossAreaAlert);
                    }
                    else if (spawnableArea2 != null)
                    {
                        // Create spawnableArea1
                        Vector3 existingConnectionPosition = spawnableArea2.GetConnectionPosition(area2Id, tempLink.indexConnectionArea2);
                        AreaLayout tempConfig = layout[tempLink.indexArea1];
                        AreaRotationHelper.Rotation tempRotation = AreaRotationHelper.RotateRotation(tempConfig.rotation, rotation);
                        bluePrint.spawnableBp[tempLink.indexArea1] = bluePrint.areaBpGeneratorArray[tempLink.indexArea1].GetSpawnableBluePrint(tempRotation, tempLink.indexConnectionArea1, existingConnectionPosition);
                        spawnableArea1 = bluePrint.spawnableBp[tempLink.indexArea1].GetRoot();
                        bluePrint.sumCreatureNumberWeight += tempConfig.creatureNumberWeight;

                        SpawnableArea.ConnectAreas(spawnableArea1, area1Id, tempLink.indexConnectionArea1, spawnableArea2, area2Id, tempLink.indexConnectionArea2, tempLink.crossAreaAlert);
                    }
                    else
                    {
                        continue;
                    }

                    AreaRotationHelper.Face faceArea1 = spawnableArea1.GetConnectionFace(area1Id, tempLink.indexConnectionArea1);
                    AreaRotationHelper.Face faceArea2 = spawnableArea2.GetConnectionFace(area2Id, tempLink.indexConnectionArea2);
                    if (faceArea1 != AreaRotationHelper.GetOppositeFace(faceArea2))
                    {
                        Debug.LogError("ComposedArea id : " + this.id
                                   + " connection between area : " + tempLink.indexArea1
                                   + " and area : " + tempLink.indexArea2
                                   + " is wrong, connection face are not opposite :"
                                   + faceArea1 + " , " + faceArea2);
                        return null;
                    }

                    tempLinkList.RemoveAt(i);
                    i--;
                }

                if (count == tempLinkList.Count)
                {
                    // Infint loop, it means graph area is not fully connected and then there is area link with no area spawned
                    Debug.LogError("Composed Area : " + id + " Fail to generate SpawnableArea. All area are not fully connected");
                    return null;
                }
            }

            // Genererate sub area managed table
            List<SpawnableArea.ExternalConnectionData> subAreaTableExternalConnection = new List<SpawnableArea.ExternalConnectionData>();
            for (int i = 0; i < externalConnections.Count; i++)
            {
                ExternalConnection externalConnection = externalConnections[i];
                subAreaTableExternalConnection.Add(new SpawnableArea.ExternalConnectionData(bluePrint.spawnableBp[externalConnection.indexArea].GetRoot(),
                                                                                            externalConnection.indexConnection,
                                                                                            bluePrint.areaBpGeneratorArray[externalConnection.indexArea].GetId()));
            }

            bluePrint.GetRoot().SetSubAreaTableExternalConnection(this.id, subAreaTableExternalConnection);
            return bluePrint;
        }

        public override void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp, int numberCreature, bool isShareNPCAlert)
        {
            if (spawnableBp is AreaCollectionFixLayoutBlueprint blueprint)
            {
                for (int i = 0; i < blueprint.areaCount; i++)
                {
                    AreaLayout areaConfiguration = layout[i];
                    int tempNumberCreature = Mathf.CeilToInt(numberCreature * (areaConfiguration.creatureNumberWeight / blueprint.sumCreatureNumberWeight));

                    blueprint.areaBpGeneratorArray[i].SetCreatureDataToSpawnableArea(blueprint.spawnableBp[i], tempNumberCreature, layout[i].isShareNPCAlert && isShareNPCAlert);
                }
            }
        }
        #endregion Methods

        #region Tools
#if UNITY_EDITOR

        [Button]
        public void AutoSetExternalConnection()
        {
            externalConnections = new List<ExternalConnection>();
            for (int indexArea = 0; indexArea < layout.Length; indexArea++)
            {
                AreaLayout tempAreaConfiguration = layout[indexArea];
                IAreaBlueprintGenerator localBpgenerator = tempAreaConfiguration.bpGeneratorId.BlueprintGenerator;
                List<AreaData.AreaConnection> tempAreaConnection = localBpgenerator.GetConnections();
                int localAreaConnectionCount = tempAreaConnection.Count;
                for (int indexConnectionLocalArea = 0; indexConnectionLocalArea < localAreaConnectionCount; indexConnectionLocalArea++)
                {
                    bool isInternalConnection = false;
                    for (int indexLink = 0; indexLink < internalLinks.Length; indexLink++)
                    {
                        InternalLink tempLink = internalLinks[indexLink];
                        if (tempLink.indexArea1 == indexArea
                            && tempLink.indexConnectionArea1 == indexConnectionLocalArea)
                        {
                            isInternalConnection = true;
                            break;
                        }

                        if (tempLink.indexArea2 == indexArea
                            && tempLink.indexConnectionArea2 == indexConnectionLocalArea)
                        {
                            isInternalConnection = true;
                            break;
                        }
                    }

                    if (!isInternalConnection)
                    {
                        externalConnections.Add(new ExternalConnection(indexArea, indexConnectionLocalArea));
                    }
                }
            }
        }
#endif //UNITY_EDITOR
        #endregion Tools
    }
}