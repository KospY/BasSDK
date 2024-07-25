using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
            }

            public void RemoveLast()
            {
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
return false;
            }

            public bool isGlobalParameterValid(SpawnableArea nextSpawnableArea, string areaId, int indexEntranceConnection)
            {
return false;
            }

            public static void ConnectAreas(AreaPlacement previousArea, AreaPlacement nextArea)
            {
            }

            public static void DisconnectAreas(AreaPlacement previousArea, AreaPlacement nextArea)
            {
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
return default;
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
return default;
        }

        public bool TryGetSpawnableBluePrint(out DungeonBlueprint blueprint, System.Random rng)
        {
blueprint = null;

            return true;
        }

        public override void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp, int numberCreature, bool isShareNPCAlert)
        {
        }

        private AreaPlacement GetAreaPlacementFromPathGroup(System.Random rng,
                                                        DungeonBlueprint dungeonBp,
                                                        PathGroup pathGroup,
                                                        AreaPlacement previousSpawnableArea,
                                                        bool allowNoExit,
                                                        Dictionary<string, List<AreaPlacement>> deadEndAreas)
        {

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
return default;
        }

        private bool IsDeadEnd(List<AreaPlacement> deadEndList, AreaPlacement area)
        {

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
return default;
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
        public void GenerateBackup()
        {
        }

        // Stat Tool
        [JsonIgnore]
        public string StatsResultFileFolderPath = "BuildStaging/DungeonsResults";
        [JsonIgnore]
        public int numberTest;
        [JsonIgnore]
        public List<QualityLevel> qualityLevels;

        [Button]
        public void UpdateStats()
        {
        }

        public DungeonBlueprint GetSpawnableBluePrintStat(System.Random rng, out bool success, out List<string> areaDeadEnd, out int retry)
        {
success = false;
areaDeadEnd = default;
retry = 0;
return default;
        }

#endif //UNITY_EDITOR
#endregion Tools
    }
}