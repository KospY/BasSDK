using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class SpawnableArea
    {
        #region Constants

        public const float NAV_MESH_LINK_SIZE = 1.0f;

        #endregion Constants

        #region InternalClass

        public class ExternalConnectionData
        {
            public SpawnableArea spawnArea;
            public int connectionIndex;
            public string areaId;

            public ExternalConnectionData(SpawnableArea spawnArea, int connectionIndex, string areaId)
            {
                this.spawnArea = spawnArea;
                this.connectionIndex = connectionIndex;
                this.areaId = areaId;
            }
        }

        public class ConnectedArea
        {
            public SpawnableArea connectedArea;
            public int connectedAreaConnectionIndex;
            public string connectionTypeId;
            public bool isCrossAreaAlert;

            public ConnectedArea(SpawnableArea connectedArea, int connectedAreaConnectionIndex, string connectionTypeId, bool isCrossAreaAlert)
            {
                this.connectedArea = connectedArea;
                this.connectedAreaConnectionIndex = connectedAreaConnectionIndex;
                this.connectionTypeId = connectionTypeId;
                this.isCrossAreaAlert = isCrossAreaAlert;
            }
        }

        #endregion InternalClass

        #region Fields

        public int managedId = -1;
        private AreaData _areaData;
        private AreaRotationHelper.Rotation _rotation;
        private Vector3 _position;
        private Bounds _bounds;
        private AreaGlobalParameter[] _areaGlobalParameters;

        // Connections
        private Dictionary<int, ConnectedArea> _connectedArea = null; // key = index connection
        private Dictionary<string, List<ExternalConnectionData>> _subAreaTableExternalConnection = null;

        // Prefab load
        private Area _areaPrefab = null;
        private LightingPreset _preloadLightningPreset = null;
        private Area _areaPrefabSpawn = null;
        private Coroutine _spawnCoroutine = null;
        private bool _isLiteMemoryState = false; // In Lite memory state some data are unload (use when area is far away) 
        private bool _isCulled = false; // true -> area spawn disable

        private bool _prefabsLoaded = false;
        private bool _prefabLoadInProgress = false;

        // Blockers load
        private Dictionary<int, string> _connectionBlockerAddress = null;
        Dictionary<int, IResourceLocation> _blockerResourceLocation;
        private Dictionary<int, GameObject> _connectionBlockerPrefab = null;

        // Gates load
        private Dictionary<int, string> _connectionGateAddress = null;
        Dictionary<int, IResourceLocation> _gateResourceLocation;
        private Dictionary<int, GameObject> _connectionGatePrefab = null;
        private Dictionary<int, GameObject> _connectionGateSpawn = null;

        // NavMesh instantiate
        private Dictionary<int, GameObject> _connectionLinkNavMesh = null;

        HashSet<SpawnableArea> markedAreas = null;
        Queue<SpawnableArea> areaListToCheck = null;

        // Creature instantiate
        private bool _isSharedNPCAlert = false;
        private int _numberCreature = 0;
        private bool _respawnDeadCreature = false;
        public bool[] isCreatureSpawnedExist = null;
        public bool[] isCreatureDead = null;

        #endregion Fields

        #region Properties
        public string AreaAddress => _areaData.areaPrefabAddress;
        public IResourceLocation AreaAddressLocation => _areaData.areaPrefabAddressLocation;
        public IResourceLocation LightingPresetAddressLocation => _areaData.lightingPresetAddressLocation;
        public IResourceLocation CullingAddressLocation => _areaData.cullingAddressLocation;
        public AreaRotationHelper.Rotation Rotation => _rotation;
        public Vector3 Position => _position;

        public Dictionary<int, GameObject> connectionBlockerPrefab => _connectionBlockerPrefab;
        public Dictionary<int, GameObject> connectionGatePrefab => _connectionGatePrefab;
        public Dictionary<int, GameObject> connectionGateSpawn => _connectionGateSpawn;
        public Dictionary<int, GameObject> connectionLinkNavMesh => _connectionLinkNavMesh;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public string AreaDataId => _areaData.id;
        public AreaData AreaData => _areaData;

        public bool IsUnique
        {
            get { return _areaData.isUnique; }
        }

        public Bounds Bounds
        {
            get { return _bounds; }
        }

        public bool IsSpawned
        {
            get { return _areaPrefabSpawn != null; }
        }

        public Area SpawnedArea
        {
            get { return _areaPrefabSpawn; }
        }

        /// <summary>
        /// When cull Most of the area is deactivate 
        /// </summary>
        public bool IsCulled
        {
get => _isCulled;
set => _isCulled = value;
        }

        /// <summary>
        /// In lite memory state some data are unload, use when area is far away for memory performance
        /// </summary>
        public bool IsLiteMemoryState
        {
get => _isLiteMemoryState;
set => _isLiteMemoryState = value;
        }

        public int NumberCreature => _numberCreature;
        public bool ResapawnDeadCreature => _respawnDeadCreature;
#endregion Properties

        #region Events

        private Action<SpawnableArea> _spawnAreaEvent = null;

        public event Action<SpawnableArea> SpawnAreaEvent
        {
            add
            {
                _spawnAreaEvent -= value;
                _spawnAreaEvent += value;
            }

            remove { _spawnAreaEvent -= value; }
        }

        private Action<SpawnableArea> _despawnAreaEvent = null;

        public event Action<SpawnableArea> DespawnAreaEvent
        {
            add
            {
                _despawnAreaEvent -= value;
                _despawnAreaEvent += value;
            }

            remove { _despawnAreaEvent -= value; }
        }

        #endregion Events

        #region Methods

        public SpawnableArea(AreaData area,
            AreaRotationHelper.Rotation rotation,
            int entranceIndex,
            Vector3 entrancePosition)
        {
        }


#endregion Methods

        #region Tools

 //UNITY_EDITOR

        #endregion Tools
    }
}
