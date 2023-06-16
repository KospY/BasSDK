using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

#if UNITY_EDITOR
using System.IO;
#endif //UNITY_EDITOR

namespace ThunderRoad
{
    public class AreaCollectionDungeon : AreaCollectionData
    {
        #region Data
        [Serializable] public class AreaTableIdContainer : DataIdContainer<AreaTable> { }

        [Serializable]
        public class PathGroup
        {
            public int numberAreaMinToSpawn;
            public int numberAreaMaxToSpawn;
            public float creatureFillPercentage = 1.0f; // Value between 0-1
            public float lootFillPercentage = 1.0f; // Value between 0-1
            public bool isSharedNPCAlert = true;

            public List<AreaTableIdContainer> areaPoolIdContainer;
        }

        public List<PathGroup> path;

        public int retry_previous_area_allowed = 10000;
        public float bounds_margin = 0.1f;

        public List<BackupData> backupList;

        private int backupIndex = -1;
        #endregion Data

        #region InternalClass
        public class DungeonBlueprint : IAreaBlueprintGenerator.SpawnableBlueprint
        {
            private List<AreaPlacement> allPlacement = new List<AreaPlacement>();
            private HashSet<string> presentUniqueAreaIds = new HashSet<string>();

            public int AreaCount { get { return allPlacement.Count; } }

            public DungeonBlueprint(AreaRotationHelper.Rotation rotation) : base(rotation) { }

            public override SpawnableArea GetRoot()
            {
                if (allPlacement.Count > 0)
                {
                    return allPlacement[0].SpawnableBluePrint.GetRoot();
                }

                return null;
            }

            public void Add(AreaPlacement newArea)
            {
                int lastIndex = allPlacement.Count - 1;
                allPlacement.Add(newArea);
                List<int> indexDepth;
                List<SpawnableArea> newAreas = newArea.SpawnableBluePrint.GetRoot().GetBreadthTree(out indexDepth);
                int count = newAreas.Count;
                for (int i = 0; i < count; i++)
                {
                    if (newAreas[i].IsUnique)
                    {
                        presentUniqueAreaIds.Add(newAreas[i].AreaDataId);
                    }
                }


                // Connect Area
                if (lastIndex >= 0)
                {
                    AreaPlacement.ConnectAreas(allPlacement[lastIndex], newArea);
                }
            }

            public void RemoveLast()
            {
                int lastIndex = allPlacement.Count - 1;
                if (lastIndex < 0) return;

                AreaPlacement areaToRemove = allPlacement[lastIndex];

                if (lastIndex > 0)
                {
                    AreaPlacement previousArea = allPlacement[lastIndex - 1];
                    AreaPlacement.DisconnectAreas(previousArea, areaToRemove);
                }

                List<int> indexDepth;
                List<SpawnableArea> areaToRemoveList = areaToRemove.SpawnableBluePrint.GetRoot().GetBreadthTree(out indexDepth);
                int count = areaToRemoveList.Count;
                for (int i = 0; i < count; i++)
                {
                    presentUniqueAreaIds.Remove(areaToRemoveList[i].AreaDataId);
                }

                allPlacement.RemoveAt(lastIndex);
            }

            public AreaPlacement GetAreaPlacementAt(int index)
            {
                if (index < 0 || index >= allPlacement.Count)
                {
                    return null;
                }

                return allPlacement[index];
            }

            public bool CheckAreaUnique(SpawnableArea area)
            {
                List<int> indexDepth;
                List<SpawnableArea> areaToCheck = area.GetBreadthTree(out indexDepth);
                int count = areaToCheck.Count;
                for (int i = 0; i < count; i++)
                {
                    if (areaToCheck[i].IsUnique)
                    {
                        string id = areaToCheck[i].AreaDataId;
                        if (presentUniqueAreaIds.Contains(id))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public class AreaPlacement
        {
            #region Fields
            private IAreaBlueprintGenerator.SpawnableBlueprint _spawnableBluePrint = null;
            private int _entranceIndex = 0;
            private int _exitIndex = 0;
            private IAreaBlueprintGenerator _bpGenerator = null;
            private AreaTable.AreaSettings _areaSettings = null;
            private PathGroup _pathGroup = null;
            #endregion Fields

            #region Properties
            public IAreaBlueprintGenerator.SpawnableBlueprint SpawnableBluePrint { get { return _spawnableBluePrint; } }

            public string BpGeneratorId { get { return _bpGenerator?.GetId(); } }
            public IAreaBlueprintGenerator BpGenerator { get { return _bpGenerator; } }
            public AreaTable.AreaSettings AreaSettings { get { return _areaSettings; } }
            public PathGroup PathGroup { get { return _pathGroup; } }

            public int EntranceIndex { get { return _entranceIndex; } }
            public int ExitIndex { get { return _exitIndex; } }
            public AreaData.AreaConnection ExitConnection
            {
                get
                {
                    return _spawnableBluePrint.GetRoot().GetConnection(BpGeneratorId, _exitIndex);
                }
            }

            public AreaRotationHelper.Face ExitFace
            {
                get
                {
                    if (_exitIndex < 0)
                    {
                        return AreaRotationHelper.Face.Front;
                    }

                    return _spawnableBluePrint.GetRoot().GetConnectionFace(BpGeneratorId, _exitIndex);
                }
            }

            public Vector3 ExitPosition
            {
                get
                {
                    return _spawnableBluePrint.GetRoot().GetConnectionPosition(BpGeneratorId, _exitIndex);
                }
            }

            public Vector3 EntrancePosition
            {
                get
                {
                    return _spawnableBluePrint.GetRoot().GetConnectionPosition(BpGeneratorId, _entranceIndex);
                }
            }
            #endregion Properties

            #region Methods
            public AreaPlacement(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBluePrint,
                                  int entranceIndex,
                                  int exitIndex,
                                  IAreaBlueprintGenerator bpGenerator,
                                  AreaTable.AreaSettings areaSettings,
                                  PathGroup pathGroup)
            {
                _spawnableBluePrint = spawnableBluePrint;
                _entranceIndex = entranceIndex;
                _exitIndex = exitIndex;
                _bpGenerator = bpGenerator;
                _areaSettings = areaSettings;
                _pathGroup = pathGroup;
            }

            public bool IsSamePath(AreaPlacement other)
            {
                SpawnableArea spawnableArea = _spawnableBluePrint.GetRoot();
                SpawnableArea otherSpawnableArea = other._spawnableBluePrint.GetRoot();
                if (string.IsNullOrEmpty(spawnableArea.AreaDataId) && !string.IsNullOrEmpty(otherSpawnableArea.AreaDataId))
                {
                    return false;
                }

                return spawnableArea.AreaDataId.Equals(otherSpawnableArea.AreaDataId)
                        && _exitIndex == other._exitIndex
                        && _entranceIndex == other._entranceIndex
                        && spawnableArea.Rotation == otherSpawnableArea.Rotation;
            }

            public bool isGlobalParameterValid(SpawnableArea nextSpawnableArea, string areaId, int indexEntranceConnection)
            {
                return nextSpawnableArea.isGlobalParameterValid(areaId, indexEntranceConnection, _spawnableBluePrint.GetRoot(), _bpGenerator.GetId(), _exitIndex);
            }

            public static void ConnectAreas(AreaPlacement previousArea, AreaPlacement nextArea)
            {
                SpawnableArea.ConnectAreas(previousArea._spawnableBluePrint.GetRoot(), previousArea.BpGeneratorId, previousArea._exitIndex, nextArea._spawnableBluePrint.GetRoot(), nextArea.BpGeneratorId, nextArea._entranceIndex, false);
            }

            public static void DisconnectAreas(AreaPlacement previousArea, AreaPlacement nextArea)
            {
                SpawnableArea.DiconnectedAreas(previousArea._spawnableBluePrint.GetRoot(), previousArea.BpGeneratorId, previousArea._exitIndex, nextArea._spawnableBluePrint.GetRoot(), nextArea.BpGeneratorId, nextArea._entranceIndex);
            }
            #endregion Methods
        }

        [System.Serializable]
        public class BackupData
        {
            public IAreaBlueprintGenerator.IAreaBlueprintGeneratorIdContainer areaDataBackupIdContainer;
            public float maxCreature;
        }
        #endregion InternalClass

        #region Methods

        public override HashSet<string> GetSpawnableAreasIds()
        {
            var areaIds = new HashSet<string>();
            for (var i = 0; i < path.Count; i++)
            {
                var tables = path[i].areaPoolIdContainer;
                if (tables == null)
                {
                    Debug.LogError(id + " - Path empty at index : " + i);
                    continue;
                }

                for (var j = 0; j < tables.Count; j++)
                {
                    var table = tables[j];
                    if (table.Data == null)
                    {
                        Debug.LogError("Table data is null: " + table.dataId);
                        continue;
                    }

                    var spawnableAreaids = table.Data.GetSpawnableAreasIds();
                    foreach (var areaId in spawnableAreaids)
                    {
                        areaIds.Add(areaId);
                    }
                }
            }
            return areaIds;
        }

        public override List<AreaData.AreaConnection> GetConnections()
        {
            return null;
        }

        public override AreaData.AreaConnection GetConnection(int index)
        {
            return null;
        }

        public override bool IsSpawnable()
        {
            return true;
        }

        public override List<AreaRotationHelper.Rotation> GetAllowedRotation()
        {
            return new List<AreaRotationHelper.Rotation>() { AreaRotationHelper.Rotation.Back,
                                                            AreaRotationHelper.Rotation.Front,
                                                            AreaRotationHelper.Rotation.Left,
                                                            AreaRotationHelper.Rotation.Right};
        }

        public override IAreaBlueprintGenerator.SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation, int entranceIndex, Vector3 entrancePosition)
        {
            backupIndex = -1;
            DungeonBlueprint dungeonBp;
            System.Random rng = new System.Random(Level.seed);
            if (TryGetSpawnableBluePrint(out dungeonBp, rng))
            {
                return dungeonBp;
            }

            Debug.LogError("[DungeonManager] : Retry max, take a backup area");
            if (backupList == null || backupList.Count == 0)
            {
                Debug.LogError("[DungeonManager] : No data backup found");
                return dungeonBp;
            }

            int backupCount = backupList.Count;
            int rand = backupCount > 1 ? rng.Next(0, backupCount) : 0;
            IAreaBlueprintGenerator backupGenerator = backupList[rand].areaDataBackupIdContainer.BlueprintGenerator;
            if (backupGenerator == null)
            {
                Debug.LogError("[DungeonManager] : data backup index : " + rand + " is not correct (can not find an area blueprint generator)");
                return dungeonBp;
            }

            backupIndex = rand;
            return backupGenerator.GetSpawnableBluePrint(rotation, entranceIndex, entrancePosition);
        }

        public bool TryGetSpawnableBluePrint(out DungeonBlueprint blueprint, System.Random rng)
        {
            blueprint = new DungeonBlueprint(AreaRotationHelper.Rotation.Front);

            // Make the list of areas 
            List<PathGroup> areasToGenerateList = new List<PathGroup>();
            int dungeonPathGroupCount = path.Count;
            for (int indexPathGroup = 0; indexPathGroup < dungeonPathGroupCount; indexPathGroup++)
            {
                PathGroup pathGroup = path[indexPathGroup];
                int nbArea = rng.Next(pathGroup.numberAreaMinToSpawn, pathGroup.numberAreaMaxToSpawn + 1);
                for (int i = 0; i < nbArea; i++)
                {
                    areasToGenerateList.Add(pathGroup);
                }
            }

            int areaCount = areasToGenerateList.Count;
            Dictionary<string, List<AreaPlacement>>[] deadEndAreaPath = new Dictionary<string, List<AreaPlacement>>[areaCount];
            int retry = 0;
            for (int areasIndex = 0; areasIndex < areaCount; areasIndex++)
            {
                bool allowNoExit = areasIndex == areaCount - 1;
                PathGroup pathGroup = areasToGenerateList[areasIndex];
                AreaPlacement newArea = GetAreaPlacementFromPathGroup(rng, blueprint, pathGroup, areasIndex > 0 ? blueprint.GetAreaPlacementAt(areasIndex - 1) : null, allowNoExit, deadEndAreaPath[areasIndex]);

                if (newArea == null)
                {
                    if (areasIndex == 0)
                    {
                        Debug.LogError("[DungeonManager] : Can not find correct start room");
                        return false;
                    }

                    if (retry < retry_previous_area_allowed)
                    {
                        retry++;
                        if (deadEndAreaPath[areasIndex] != null)
                        {
                            deadEndAreaPath[areasIndex].Clear();
                        }

                        areasIndex--;
                        if (deadEndAreaPath[areasIndex] == null)
                        {
                            deadEndAreaPath[areasIndex] = new Dictionary<string, List<AreaPlacement>>();
                        }

                        AreaPlacement previousArea = blueprint.GetAreaPlacementAt(areasIndex);
                        string previousAreaId = previousArea.BpGeneratorId;
                        if (!deadEndAreaPath[areasIndex].ContainsKey(previousAreaId))
                        {
                            deadEndAreaPath[areasIndex].Add(previousAreaId, new List<AreaPlacement>());
                        }

                        deadEndAreaPath[areasIndex][previousAreaId].Add(previousArea);

                        blueprint.RemoveLast();
                        areasIndex--;

                        continue;
                    }

                    return false;
                }

                blueprint.Add(newArea);
            }

            Debug.Log("Dungeon generation success with " + retry + " area retry. Used seed:" + Level.seed);

            return true;
        }

        public override void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp, int numberCreature, bool isShareNPCAlert)
        {
            if (backupIndex < 0)
            {
                if (spawnableBp is DungeonBlueprint blueprint)
                {
                    int count = blueprint.AreaCount;
                    float maxCreature = 0;
                    for (int i = 0; i < count; i++)
                    {
                        AreaPlacement tempArea = blueprint.GetAreaPlacementAt(i);
                        float areaMaxCreature = tempArea.AreaSettings.maxCreature * tempArea.PathGroup.creatureFillPercentage;
                        maxCreature += areaMaxCreature;
                    }

                    if (numberCreature > maxCreature)
                    {
                        numberCreature = Mathf.FloorToInt(maxCreature);
                    }

                    for (int i = 0; i < count; i++)
                    {
                        AreaPlacement tempArea = blueprint.GetAreaPlacementAt(i);
                        float areaMaxCreature = tempArea.AreaSettings.maxCreature * tempArea.PathGroup.creatureFillPercentage;
                        int tempNumberCreature = Mathf.CeilToInt(numberCreature * (areaMaxCreature / maxCreature));

                        tempArea.BpGenerator.SetCreatureDataToSpawnableArea(tempArea.SpawnableBluePrint, tempNumberCreature, isShareNPCAlert);
                    }
                }

                return;
            }

            // Use Backup
            BackupData backup = backupList[backupIndex];
            if (numberCreature > backup.maxCreature)
            {
                numberCreature = Mathf.FloorToInt(backup.maxCreature);
            }

            IAreaBlueprintGenerator backupGenerator = backup.areaDataBackupIdContainer.BlueprintGenerator;
            backupGenerator.SetCreatureDataToSpawnableArea(spawnableBp, numberCreature, isShareNPCAlert);
        }

        private AreaPlacement GetAreaPlacementFromPathGroup(System.Random rng,
                                                        DungeonBlueprint dungeonBp,
                                                        PathGroup pathGroup,
                                                        AreaPlacement previousSpawnableArea,
                                                        bool allowNoExit,
                                                        Dictionary<string, List<AreaPlacement>> deadEndAreas)
        {
            // Select Room
            DropTable<AreaPlacement> areaPool = new DropTable<AreaPlacement>();
            int nbGroup = pathGroup.areaPoolIdContainer.Count;
            for (int indexAreaGroup = 0; indexAreaGroup < nbGroup; indexAreaGroup++)
            {
                AreaTable areaGroup = pathGroup.areaPoolIdContainer[indexAreaGroup].Data;
                if (areaGroup != null)
                {
                    int areaCount = areaGroup.areaSettingTable.drops.Count;
                    for (int indexArea = 0; indexArea < areaCount; indexArea++)
                    {
                        AreaTable.AreaSettings areaSetting = areaGroup.areaSettingTable.drops[indexArea].dropItem;
                        IAreaBlueprintGenerator bgGenerator = areaSetting.bpGeneratorIdContainer.BlueprintGenerator;
                        if (bgGenerator == null)
                        {
                            Debug.LogError("Cannot find IBlueprintGenerator for id : " + areaSetting.bpGeneratorIdContainer.dataId);
                            continue;
                        }

                        if (!bgGenerator.IsSpawnable())
                        {
                            continue;
                        }

                        if (previousSpawnableArea != null
                            && bgGenerator.GetId().Equals(previousSpawnableArea.BpGeneratorId))
                        {
                            // Don't use the same area twice in a row
                            continue;
                        }

                        AreaPlacement tempSpawnableArea = null;
                        List<AreaPlacement> deadEndAreaSettings = null;
                        if (deadEndAreas != null)
                        {
                            deadEndAreas.TryGetValue(bgGenerator.GetId(), out deadEndAreaSettings);
                        }

                        if (previousSpawnableArea != null)
                        {
                            tempSpawnableArea = GetDungeonPlacementArea(rng, dungeonBp, pathGroup, areaSetting, previousSpawnableArea, allowNoExit, deadEndAreaSettings);
                        }
                        else
                        {
                            tempSpawnableArea = GetDungeonPlacementStartArea(rng, pathGroup, areaSetting, deadEndAreaSettings);
                        }

                        if (tempSpawnableArea != null)
                        {
                            DropTable<AreaPlacement>.Drop drop = new DropTable<AreaPlacement>.Drop();
                            drop.dropItem = tempSpawnableArea;
                            drop.probabilityWeight = areaGroup.areaSettingTable.drops[indexArea].probabilityWeight;
                            areaPool.drops.Add(drop);
                        }
                    }
                }
            }

            AreaPlacement result = null;
            if (areaPool.TryPick(out result, rng))
            {
                return result;
            }

            return null;

        }

        /// <summary>
        /// Chose room settings (entrance, exit, rotation)
        /// </summary>
        /// <param name="area">The area we need to set the settings</param>
        /// <param name="previousArea">The previous area that need to be connected</param>
        /// <param name="allowNoExit">boolean allow area without exit (dungeon ends)</param>
        /// <param name="deadEndAreas">List of all area's settings that has been tested and end up in a dead end. Areas' settings in this list will not be consider as a valid choice</param>
        /// <returns></returns>
        private AreaPlacement GetDungeonPlacementArea(System.Random rng,
                                                        DungeonBlueprint dungeonBp,
                                                        PathGroup pathGroup,
                                                        AreaTable.AreaSettings areaSetting,
                                                        AreaPlacement previousArea, bool allowNoExit,
                                                        List<AreaPlacement> deadEndAreaSettings)
        {
            SpawnableArea root = dungeonBp.GetRoot();
            IAreaBlueprintGenerator bpGenerator = areaSetting.bpGeneratorIdContainer.BlueprintGenerator;

            AreaRotationHelper.Face entranceFace = AreaRotationHelper.GetOppositeFace(previousArea.ExitFace);
            AreaData.AreaConnection previousConnection = previousArea.ExitConnection;
            Vector3 previousAreaExitPosition = previousArea.ExitPosition;

            List<int> allEntranceConnectionIndex = areaSetting.entranceConnection;
            int entranceCount = allEntranceConnectionIndex.Count;
            List<int> allExitConnectionIndex = areaSetting.exitConnection;
            int exitCount = allExitConnectionIndex.Count;

            List<AreaRotationHelper.Rotation> allowedRotation = bpGenerator.GetAllowedRotation();
            if (allowedRotation == null) return null;

            int rotationCount = allowedRotation.Count;

            List<AreaPlacement> spawnableDataList = new List<AreaPlacement>();

            for (int indexEntrance = 0; indexEntrance < entranceCount; indexEntrance++)
            {
                int entranceConnectionIndex = allEntranceConnectionIndex[indexEntrance];
                AreaData.AreaConnection tempEntranceConnection = bpGenerator.GetConnection(entranceConnectionIndex);
                if (tempEntranceConnection == null) continue;
                if (tempEntranceConnection.IsConnectionValid(previousConnection))
                {
                    for (int indexRotation = 0; indexRotation < rotationCount; indexRotation++)
                    {
                        AreaRotationHelper.Rotation tempRotation = allowedRotation[indexRotation];
                        if (AreaRotationHelper.RotateFace(tempEntranceConnection.face, tempRotation) == entranceFace)
                        {
                            IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp = bpGenerator.GetSpawnableBluePrint(tempRotation,
                                                                                entranceConnectionIndex,
                                                                                previousAreaExitPosition);

                            if (!dungeonBp.CheckAreaUnique(spawnableBp.GetRoot()))
                            {
                                return null;
                            }

                            if (!previousArea.isGlobalParameterValid(spawnableBp.GetRoot(), bpGenerator.GetId(), entranceConnectionIndex))
                            {
                                continue;
                            }

                            if (exitCount == 0 && allowNoExit)
                            {
                                AreaPlacement spawnArea = new AreaPlacement(spawnableBp, entranceConnectionIndex, -1, bpGenerator, areaSetting, pathGroup);
                                if (!root.IntersectRecursif(spawnableBp, bounds_margin)
                                     && !IsDeadEnd(deadEndAreaSettings, spawnArea))
                                {
                                    spawnableDataList.Add(spawnArea);
                                }

                                continue;
                            }

                            for (int indexExit = 0; indexExit < exitCount; indexExit++)
                            {
                                int exitConnectionIndex = allExitConnectionIndex[indexExit];
                                AreaData.AreaConnection tempExitConnection = bpGenerator.GetConnection(exitConnectionIndex);
                                if (tempExitConnection == null) continue;
                                if (tempExitConnection == tempEntranceConnection)
                                {
                                    continue;
                                }

                                AreaPlacement spawnArea = new AreaPlacement(spawnableBp,
                                                                            entranceConnectionIndex,
                                                                            exitConnectionIndex,
                                                                            bpGenerator,
                                                                            areaSetting,
                                                                            pathGroup);



                                if (!root.IntersectRecursif(spawnableBp, bounds_margin)
                                    && !IsDeadEnd(deadEndAreaSettings, spawnArea))
                                {
                                    spawnableDataList.Add(spawnArea);
                                }
                            }
                        }
                    }
                }
            }

            if (spawnableDataList.Count == 0)
            {
                return null;
            }

            if (spawnableDataList.Count == 1)
            {
                return spawnableDataList[0];
            }

            AreaPlacement areaPlacement = spawnableDataList[rng.Next(0, spawnableDataList.Count)];
            return areaPlacement;
        }

        private bool IsDeadEnd(List<AreaPlacement> deadEndList, AreaPlacement area)
        {
            if (deadEndList == null)
            {
                return false;
            }

            int count = deadEndList.Count;
            for (int i = 0; i < count; i++)
            {
                if (area.IsSamePath(deadEndList[i]))
                //if (area.IsSimilarPath(deadEndList[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Chose area settings (entrance, exit, rotation) without considering entrance
        /// </summary>
        /// <param name="area">The area we need to chose the settings</param>
        /// <param name="deadEndAreaSettings">List of area's setting that end up as a dead end. Settings on this list will not be consider as a valid choice</param>
        /// <returns></returns>
        private AreaPlacement GetDungeonPlacementStartArea(System.Random rng,
                                                        PathGroup pathGroup,
                                                        AreaTable.AreaSettings areaSetting,
                                                        List<AreaPlacement> deadEndAreaSettings)
        {
            IAreaBlueprintGenerator bpGenerator = areaSetting.bpGeneratorIdContainer.BlueprintGenerator;
            if (bpGenerator == null)
            {
                return null;
            }

            List<int> allExitConnectionIndex = areaSetting.exitConnection;
            int exitCount = allExitConnectionIndex.Count;

            List<AreaRotationHelper.Rotation> allowedRotation = bpGenerator.GetAllowedRotation();
            if (allowedRotation == null) return null;

            int rotationCount = allowedRotation.Count;

            List<AreaPlacement> spawnableDataList = new List<AreaPlacement>();

            for (int indexRotation = 0; indexRotation < rotationCount; indexRotation++)
            {
                AreaRotationHelper.Rotation tempRotation = allowedRotation[indexRotation];
                for (int indexExit = 0; indexExit < exitCount; indexExit++)
                {
                    int exitConnectionIndex = allExitConnectionIndex[indexExit];
                    AreaData.AreaConnection tempExitConnection = bpGenerator.GetConnection(exitConnectionIndex);
                    if (tempExitConnection == null) continue;
                    if (AreaRotationHelper.RotateFace(tempExitConnection.face, tempRotation) != AreaRotationHelper.Face.Back)
                    {
                        IAreaBlueprintGenerator.SpawnableBlueprint spawnableBluePrint = bpGenerator.GetSpawnableBluePrint(tempRotation, -1, Vector3.zero);

                        AreaPlacement spawnArea = new AreaPlacement(spawnableBluePrint,
                                                                    -1,
                                                                    exitConnectionIndex,
                                                                    bpGenerator,
                                                                    areaSetting,
                                                                    pathGroup);

                        if (!IsDeadEnd(deadEndAreaSettings, spawnArea))
                        {
                            spawnableDataList.Add(spawnArea);
                        }
                    }
                }
            }

            if (spawnableDataList.Count == 0)
            {
                return null;
            }

            if (spawnableDataList.Count == 1)
            {
                return spawnableDataList[0];
            }

            return spawnableDataList[rng.Next(0, spawnableDataList.Count)];
        }

        #endregion Methods

        #region Tools
#if UNITY_EDITOR
        [JsonIgnore] public bool useSeed = false;
        [JsonIgnore] public int seed = 0;

        public override void PreviewArea(bool spawnItem = false)
        {
            if (useSeed)
            {
                Level.seed = seed;
            }
            else
            {
                Level.GenerateNewSeed();
            }
            base.PreviewArea(spawnItem);
        }

        // Backup Generation
        [Button]
        public void GenerateBackup()
        {
            Platform currentPlatform = Common.GetPlatform();
            Common.SetPlatform(Platform.Android);

            if (backupList == null)
            {
                backupList = new List<BackupData>();
            }

            if (backupList.Count == 0)
            {
                // Add at least 1 backup
                BackupData tempBackup = new BackupData();
                tempBackup.areaDataBackupIdContainer = new IAreaBlueprintGenerator.IAreaBlueprintGeneratorIdContainer("", Category.AreaCollection);
                backupList.Add(tempBackup);
            }

            int backupCount = backupList.Count;
            for (int indexBackup = 0; indexBackup < backupCount; indexBackup++)
            {
                BackupData backupData = backupList[indexBackup];
                AreaCollectionFixLayout backupFixLayout = new AreaCollectionFixLayout();
                backupFixLayout.id = "DungeonBackup_" + id + "_" + indexBackup;
                backupData.areaDataBackupIdContainer.dataId = backupFixLayout.id;
                backupData.areaDataBackupIdContainer.category = Category.AreaCollection;

                backupFixLayout.allowedRotation = new List<AreaRotationHelper.Rotation>();
                backupFixLayout.allowedRotation.Add(AreaRotationHelper.Rotation.Front);
                backupFixLayout.allowedRotation.Add(AreaRotationHelper.Rotation.Back);
                backupFixLayout.allowedRotation.Add(AreaRotationHelper.Rotation.Left);
                backupFixLayout.allowedRotation.Add(AreaRotationHelper.Rotation.Right);

                DungeonBlueprint dungeonBlueprint;

                Level.GenerateNewSeed();
                int tempSeed = Level.seed;
                System.Random rng = new System.Random(tempSeed);

                while (!TryGetSpawnableBluePrint(out dungeonBlueprint, rng)) { }

                int areaCount = dungeonBlueprint.AreaCount;
                float maxCreature = 0;
                for (int i = 0; i < areaCount; i++)
                {
                    AreaPlacement tempArea = dungeonBlueprint.GetAreaPlacementAt(i);
                    float areaMaxCreature = tempArea.AreaSettings.maxCreature * tempArea.PathGroup.creatureFillPercentage;
                    maxCreature += areaMaxCreature;
                }

                backupData.maxCreature = maxCreature;

                backupFixLayout.layout = new AreaCollectionFixLayout.AreaLayout[areaCount];
                backupFixLayout.internalLinks = new AreaCollectionFixLayout.InternalLink[areaCount - 1];
                backupFixLayout.externalConnections = new List<AreaCollectionFixLayout.ExternalConnection>();
                for (int indexArea = 0; indexArea < areaCount; indexArea++)
                {
                    AreaCollectionFixLayout.AreaLayout tempLayout = new AreaCollectionFixLayout.AreaLayout();
                    AreaPlacement areaPlacement = dungeonBlueprint.GetAreaPlacementAt(indexArea);
                    ThunderRoad.Category category = areaPlacement.SpawnableBluePrint is AreaData.AreaBlueprint ? Category.Area : Category.AreaCollection;
                    tempLayout.bpGeneratorId = new IAreaBlueprintGenerator.IAreaBlueprintGeneratorIdContainer(areaPlacement.BpGeneratorId, category);
                    tempLayout.rotation = areaPlacement.SpawnableBluePrint.Rotation;
                    tempLayout.creatureNumberWeight = areaPlacement.AreaSettings.maxCreature * areaPlacement.PathGroup.creatureFillPercentage;
                    tempLayout.isShareNPCAlert = true;
                    backupFixLayout.layout[indexArea] = tempLayout;

                    if (indexArea > 0)
                    {
                        AreaCollectionFixLayout.InternalLink tempLink = new AreaCollectionFixLayout.InternalLink();
                        AreaPlacement previousAreaPlacement = dungeonBlueprint.GetAreaPlacementAt(indexArea - 1);
                        tempLink.indexArea1 = indexArea - 1;
                        tempLink.indexConnectionArea1 = previousAreaPlacement.ExitIndex;
                        tempLink.indexArea2 = indexArea;
                        tempLink.indexConnectionArea2 = areaPlacement.EntranceIndex;
                        tempLink.crossAreaAlert = false;
                        backupFixLayout.internalLinks[indexArea - 1] = tempLink;
                    }
                }

                Catalog.SaveToJson(backupFixLayout);
            }

            Common.SetPlatform(currentPlatform);
        }

        // Stat Tool
        public string StatsResultFileFolderPath = "BuildStaging/DungeonsResults";
        public int numberTest;
        public List<Platform> plateformTypes;

        [Button]
        public void UpdateStats()
        {
            if (plateformTypes == null || plateformTypes.Count == 0)
            {
                Debug.LogError("No plateform Type selected");
                return;
            }

            Platform currentPlatform = Common.GetPlatform();

            int plateformCount = plateformTypes.Count;

            Dictionary<string, int>[] areaPickCount = new Dictionary<string, int>[plateformCount];
            Dictionary<string, int>[] areaDeadEndCount = new Dictionary<string, int>[plateformCount];
            Dictionary<string, bool>[] areaIsSpawnable = new Dictionary<string, bool>[plateformCount];
            int dungeonPathGroupCount = path.Count;

            for (int indexPlatefom = 0; indexPlatefom < plateformCount; indexPlatefom++)
            {
                areaPickCount[indexPlatefom] = new Dictionary<string, int>();
                areaDeadEndCount[indexPlatefom] = new Dictionary<string, int>();
                areaIsSpawnable[indexPlatefom] = new Dictionary<string, bool>();

                for (int indexPathGroup = 0; indexPathGroup < dungeonPathGroupCount; indexPathGroup++)
                {
                    PathGroup pathGroup = path[indexPathGroup];
                    for (int i = 0; i < pathGroup.areaPoolIdContainer.Count; i++)
                    {
                        AreaTable table = pathGroup.areaPoolIdContainer[i].Data;
                        foreach (var drops in table.areaSettingTable.drops)
                        {
                            string id = drops.dropItem.bpGeneratorIdContainer.dataId;
                            if (!areaPickCount[indexPlatefom].ContainsKey(id))
                            {
                                areaPickCount[indexPlatefom].Add(id, 0);

                                bool isSpawnable = drops.dropItem.bpGeneratorIdContainer.BlueprintGenerator.IsSpawnable();

                                areaIsSpawnable[indexPlatefom].Add(id, isSpawnable);
                            }

                            if (!areaDeadEndCount[indexPlatefom].ContainsKey(id))
                            {
                                areaDeadEndCount[indexPlatefom].Add(id, 0);
                            }
                        }
                    }
                }
            }

            List<int>[] failureSeed = new List<int>[plateformCount];
            float[] meanRetry = new float[plateformCount];

            for (int indexPlatefom = 0; indexPlatefom < plateformCount; indexPlatefom++)
            {
                Common.SetPlatform(plateformTypes[indexPlatefom]);
                failureSeed[indexPlatefom] = new List<int>();
                meanRetry[indexPlatefom] = 0.0f;
                for (int indexTest = 0; indexTest < numberTest; indexTest++)
                {
                    if (indexTest > 0)
                    {
                        Level.GenerateNewSeed();
                    }
                    int tempSeed = Level.seed;

                    System.Random rng = new System.Random(tempSeed);
                    bool success;
                    List<string> areaDeadEnd;
                    int retry;
                    DungeonBlueprint resultBp = GetSpawnableBluePrintStat(rng, out success, out areaDeadEnd, out retry);

                    if (!success)
                    {
                        failureSeed[indexPlatefom].Add(tempSeed);
                    }
                    else
                    {
                        meanRetry[indexPlatefom] += retry;
                        int tempAreaCount = resultBp.AreaCount;
                        for (int indexArea = 0; indexArea < tempAreaCount; indexArea++)
                        {
                            AreaPlacement areaPlacement = resultBp.GetAreaPlacementAt(indexArea);
                            areaPickCount[indexPlatefom][areaPlacement.BpGeneratorId]++;
                        }
                    }

                    foreach (string areaId in areaDeadEnd)
                    {
                        areaDeadEndCount[indexPlatefom][areaId]++;
                    }
                }
            }

            Common.SetPlatform(currentPlatform);

            // Write Data
            string filePath = StatsResultFileFolderPath + "/" + id + ".csv";
            StreamWriter writer = new StreamWriter(filePath, false);

            writer.WriteLine("Dungeon Id : " + id + "; Number Test : " + numberTest);
            writer.WriteLine("");
            int areaCount = areaPickCount[0].Count;
            string linePlateform = string.Empty;
            string lineHeader = string.Empty;
            string retryHeader = string.Empty;
            string[] lineArea = new string[areaCount];
            for (int indexPlateform = 0; indexPlateform < plateformCount; indexPlateform++)
            {
                linePlateform += plateformTypes[indexPlateform].ToString() + ";Number failure : ;" + failureSeed[indexPlateform].Count + ";;";
                float numberSuccess = numberTest - failureSeed[indexPlateform].Count;
                retryHeader += ";" + ((numberSuccess > 0) ? "Mean retry :; " + (meanRetry[indexPlateform] / numberSuccess) + ";;"
                                                            : ";;;");
                lineHeader += "Area Id;Pick Count;Dead End Count;;";

                int lineIndex = 0;
                foreach (KeyValuePair<string, int> pair in areaPickCount[indexPlateform])
                {
                    string areaId = pair.Key;
                    int pickCount = pair.Value;
                    int deadEndCount = areaDeadEndCount[indexPlateform][areaId];

                    lineArea[lineIndex] += areaId + ";" + pickCount + ";" + deadEndCount + ";;";
                    lineIndex++;
                }
            }

            writer.WriteLine(linePlateform);
            writer.WriteLine(retryHeader);
            writer.WriteLine("");
            writer.WriteLine(lineHeader);
            for (int i = 0; i < areaCount; i++)
            {
                writer.WriteLine(lineArea[i]);
            }

            string failureHeader = string.Empty;
            List<string> lineFailure = new List<string>();
            for (int indexPlateform = 0; indexPlateform < plateformCount; indexPlateform++)
            {
                failureHeader += "Failure seed : ;;;;";
                for (int indexFailure = 0; indexFailure < failureSeed[indexPlateform].Count; indexFailure++)
                {
                    if (indexFailure >= lineFailure.Count)
                    {
                        lineFailure.Add(string.Empty);
                    }

                    lineFailure[indexFailure] += failureSeed[indexPlateform][indexFailure] + ";;;;";
                }
            }

            writer.WriteLine("");
            writer.WriteLine(failureHeader);
            for (int i = 0; i < lineFailure.Count; i++)
            {
                writer.WriteLine(lineFailure[i]);
            }

            writer.Close();
        }
        public DungeonBlueprint GetSpawnableBluePrintStat(System.Random rng, out bool success, out List<string> areaDeadEnd, out int retry)
        {
            DungeonBlueprint blueprint = new DungeonBlueprint(AreaRotationHelper.Rotation.Front);
            success = true;
            areaDeadEnd = new List<string>();

            // Make the list of areas 
            List<PathGroup> areasToGenerateList = new List<PathGroup>();
            int dungeonPathGroupCount = path.Count;
            for (int indexPathGroup = 0; indexPathGroup < dungeonPathGroupCount; indexPathGroup++)
            {
                PathGroup pathGroup = path[indexPathGroup];
                int nbArea = rng.Next(pathGroup.numberAreaMinToSpawn, pathGroup.numberAreaMaxToSpawn + 1);
                for (int i = 0; i < nbArea; i++)
                {
                    areasToGenerateList.Add(pathGroup);
                }
            }

            int areaCount = areasToGenerateList.Count;
            Dictionary<string, List<AreaPlacement>>[] deadEndAreaPath = new Dictionary<string, List<AreaPlacement>>[areaCount];
            retry = 0;
            string redrawId = null;
            for (int areasIndex = 0; areasIndex < areaCount; areasIndex++)
            {
                bool allowNoExit = areasIndex == areaCount - 1;
                PathGroup pathGroup = areasToGenerateList[areasIndex];
                AreaPlacement newArea = GetAreaPlacementFromPathGroup(rng, blueprint, pathGroup, areasIndex > 0 ? blueprint.GetAreaPlacementAt(areasIndex - 1) : null, allowNoExit, deadEndAreaPath[areasIndex]);

                if (newArea == null)
                {
                    if (areasIndex == 0)
                    {
                        success = false;
                        break;
                    }

                    if (retry < retry_previous_area_allowed)
                    {
                        retry++;
                        if (deadEndAreaPath[areasIndex] != null)
                        {
                            deadEndAreaPath[areasIndex].Clear();
                        }

                        areasIndex--;
                        if (deadEndAreaPath[areasIndex] == null)
                        {
                            deadEndAreaPath[areasIndex] = new Dictionary<string, List<AreaPlacement>>();
                        }

                        AreaPlacement previousArea = blueprint.GetAreaPlacementAt(areasIndex);
                        string previousAreaId = previousArea.BpGeneratorId;

                        redrawId = previousAreaId;

                        if (!deadEndAreaPath[areasIndex].ContainsKey(previousAreaId))
                        {
                            deadEndAreaPath[areasIndex].Add(previousAreaId, new List<AreaPlacement>());
                        }

                        deadEndAreaPath[areasIndex][previousAreaId].Add(previousArea);

                        blueprint.RemoveLast();
                        areasIndex--;

                        continue;
                    }

                    success = false;
                    break;
                }

                if (redrawId != null)
                {
                    areaDeadEnd.Add(redrawId);
                    redrawId = null;
                }

                blueprint.Add(newArea);
            }

            return blueprint;
        }

#endif //UNITY_EDITOR
        #endregion Tools
    }
}