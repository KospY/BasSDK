using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class SpawnableArea
    {
        #region Constants

        private const float NAV_MESH_LINK_SIZE = 1.0f;

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
        private IResourceLocation _resourceLocation;
        private Area _areaPrefab = null;
        private Area _areaPrefabSpawn = null;
        private Coroutine _spawnCoroutine = null;
        private bool _isCulled = false; // true -> area spawn disable

        // Blockers load
        private Dictionary<int, string> _connectionBlockerAddress = null;
        Dictionary<int, IResourceLocation> _blockerResourceLocation;
        private Dictionary<int, GameObject> _connectionBlockerPrefab = null;

        // Fake view load
        private Dictionary<int, string> _connectionFakeViewAddress = null;
        Dictionary<int, IResourceLocation> _fakeViewResourceLocation;
        private Dictionary<int, FakeViewData> _connectionFakeViewData = null;

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

        public string AreaAddress
        {
            get { return _areaData.areaPrefabAddress; }
        }

        public AreaRotationHelper.Rotation Rotation
        {
            get { return _rotation; }
        }

        public Vector3 Position
        {
            get { return _position; }
        }

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public string AreaDataId
        {
            get { return _areaData.id; }
        }

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

        public bool IsCulled
        {
            get { return _isCulled; }
            set
            {
                if (_isCulled == value)
                {
                    return;
                }

                _isCulled = value;
                if (IsSpawned)
                {
                    _areaPrefabSpawn.SetCull(_isCulled);
                }
            }
        }

        public int NumberCreature
        {
            get { return _numberCreature; }
        }

        public bool ResapawnDeadCreature
        {
            get { return _respawnDeadCreature; }
        }

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
            _areaData = area;

            _rotation = rotation;
            // Compute position
            if (entranceIndex < 0)
            {
                _position = entrancePosition;
            }
            else
            {
                AreaData.AreaConnection entranceConnection = area.GetConnection(entranceIndex);
                if (entranceConnection == null)
                {
                    Debug.LogError("Entrance index invalid");
                }

                Vector3 entranceDirection = entranceConnection.position;
                Quaternion quaternion = AreaRotationHelper.GetRotationQuaternionFromRotation(_rotation);
                entranceDirection = quaternion * entranceDirection;
                _position = entrancePosition - entranceDirection;
            }

            // Compute Bounds
            Vector3 boundsCenter = AreaRotationHelper.GetRotationQuaternionFromRotation(_rotation) * area.Bounds.center;
            boundsCenter = boundsCenter + _position;
            Vector3 boundsSize = AreaRotationHelper.GetBoundsSizeFromRotation(area.Bounds.size, _rotation);
            _bounds = new Bounds(boundsCenter, boundsSize);

            // Get Blocker for connection
            int connectionCount = _areaData.connections.Count;
            _connectionBlockerAddress = new Dictionary<int, string>();
            for (int connectionIndex = 0; connectionIndex < connectionCount; connectionIndex++)
            {
                AreaData.AreaConnection connection = _areaData.connections[connectionIndex];
                string blockerPrefabAddress = null;
                if (connection.overrideBlockerTableAdress == null || !connection.overrideBlockerTableAdress.TryPick(out blockerPrefabAddress))
                {
                    int connectionTypeCount = connection.connectionTypeIdContainerList.Count;
                    for (int indexType = 0; indexType < connectionTypeCount; indexType++)
                    {
                        AreaConnectionTypeData typeData = connection.connectionTypeIdContainerList[indexType].Data;
                        if (typeData.randomBlockerTableAdress != null && typeData.randomBlockerTableAdress.TryPick(out blockerPrefabAddress))
                        {
                            break;
                        }
                        else
                        {
                            blockerPrefabAddress = null;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(blockerPrefabAddress))
                {
                    _connectionBlockerAddress[connectionIndex] = blockerPrefabAddress;
                }
            }

            if (_areaData.areaGlobalParameters != null)
            {
                _areaGlobalParameters = new AreaGlobalParameter[_areaData.areaGlobalParameters.Length];
                for (int i = 0; i < _areaData.areaGlobalParameters.Length; i++)
                {
                    _areaGlobalParameters[i] = _areaData.areaGlobalParameters[i].GetParameterFor(this);
                }
            }
        }

        public void SetSubAreaTableExternalConnection(string areaId, List<ExternalConnectionData> subAreaTableExternalConnection)
        {
            if (_subAreaTableExternalConnection == null)
            {
                _subAreaTableExternalConnection = new Dictionary<string, List<ExternalConnectionData>>();
            }

            if (subAreaTableExternalConnection != null && subAreaTableExternalConnection.Count > 0)
            {
                _subAreaTableExternalConnection.Add(areaId, subAreaTableExternalConnection);
            }
        }

        public bool isGlobalParameterValid(string areaId, int indexEntrance, SpawnableArea previousArea, string previousAreaId, int indexConnectionPreviousArea)
        {
            int finalIndexEntrance;
            SpawnableArea finalArea = GetFinalArea(areaId, indexEntrance, out finalIndexEntrance);

            int previousFinalAreaIndexConnection;
            SpawnableArea previousFinalArea = previousArea.GetFinalArea(previousAreaId, indexConnectionPreviousArea, out previousFinalAreaIndexConnection);

            if (finalArea._areaGlobalParameters != null && previousFinalArea._areaGlobalParameters != null)
            {
                for (int i = 0; i < finalArea._areaGlobalParameters.Length; i++)
                {
                    AreaGlobalParameter globalParameter = finalArea._areaGlobalParameters[i];
                    for (int j = 0; j < previousFinalArea._areaGlobalParameters.Length; j++)
                    {
                        AreaGlobalParameter previousGlobalParameter = previousFinalArea._areaGlobalParameters[j];
                        if (!globalParameter.IsCompatible(previousGlobalParameter)
                            || !previousGlobalParameter.IsCompatible(globalParameter))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void ApplyGlobalParameters(bool isInArea, bool inGateWay, SpawnableArea connectedArea)
        {
            if (_areaGlobalParameters != null)
            {
                for (int i = 0; i < _areaGlobalParameters.Length; i++)
                {
                    _areaGlobalParameters[i].Apply(isInArea, inGateWay, connectedArea);
                }
            }
        }

        public bool TryGetGlobalParameter<T>(out T globalParameter) where T : AreaGlobalParameter
        {
            if (_areaGlobalParameters != null)
            {
                for (int i = 0; i < _areaGlobalParameters.Length; i++)
                {
                    if (_areaGlobalParameters[i] is T parameter)
                    {
                        globalParameter = parameter;
                        return true;
                    }
                }
            }

            globalParameter = null;
            return false;
        }

        public SpawnableArea GetFinalArea(string areaId, int indexConnection, out int indexConnectionFinalArea)
        {
            SpawnableArea finalArea;
            string finalAreaId;
            if (isConnectionFromSubArea(areaId, indexConnection, out finalArea, out indexConnectionFinalArea, out finalAreaId))
            {
                return finalArea.GetFinalArea(finalAreaId, indexConnectionFinalArea, out indexConnectionFinalArea);
            }

            return this;
        }

        public AreaRotationHelper.Face GetConnectionFace(string areaId, int indexConnection)
        {
            AreaData.AreaConnection connection = GetConnection(areaId, indexConnection);
            return AreaRotationHelper.RotateFace(connection.face, _rotation);
        }

        public AreaData.AreaConnection GetConnection(string areaId, int indexConnection)
        {
            int indexConnectionFinalArea;
            SpawnableArea finalArea = GetFinalArea(areaId, indexConnection, out indexConnectionFinalArea);

            return finalArea._areaData.GetConnection(indexConnectionFinalArea);
        }

        public Vector3 GetConnectionPosition(string areaId, int indexConnection)
        {
            SpawnableArea subArea;
            int indexConnectionSubArea;
            string subAreaId;
            if (isConnectionFromSubArea(areaId, indexConnection, out subArea, out indexConnectionSubArea, out subAreaId))
            {
                return subArea.GetConnectionPosition(subAreaId, indexConnectionSubArea);
            }

            AreaData.AreaConnection connection = GetConnection(areaId, indexConnection);

            if (connection == null)
            {
                Debug.LogError($"connection Index : {indexConnection} is invalide for sub area Managed");
                return Vector3.zero;
            }

            Vector3 connectionPosition = connection.position;
            Quaternion quaternion = AreaRotationHelper.GetRotationQuaternionFromRotation(_rotation);
            connectionPosition = quaternion * connectionPosition;
            return _position + connectionPosition;
        }

        public static void ConnectAreas(SpawnableArea area1, string area1Id, int indexConnection1, SpawnableArea area2, string area2Id, int indexConnection2, bool isCrossAreaAlert)
        {
            AreaConnectionTypeData connectionType = null;
            AreaData.AreaConnection connectionArea1 = area1.GetConnection(area1Id, indexConnection1);
            AreaData.AreaConnection connectionArea2 = area2.GetConnection(area2Id, indexConnection2);
            if (!connectionArea1.TryGetFirstConnectionValid(connectionArea2, out connectionType))
            {
                Debug.LogError($" Connection between area : {area1Id} with connection index : {indexConnection1} and area : {area2Id} with connection index : {indexConnection2} is wrong, connection type doesn't match : ");
                return;
            }

            string gatePrefabAddress = null;
            if (connectionType.randomGateTableAdress != null) connectionType.randomGateTableAdress.TryPick(out gatePrefabAddress);
            area1.AddConnectedArea(area1Id, indexConnection1, area2, indexConnection2, connectionType.id, connectionArea2.fakeViewAddress, gatePrefabAddress, isCrossAreaAlert);
            area2.AddConnectedArea(area2Id, indexConnection2, area1, indexConnection1, connectionType.id, connectionArea1.fakeViewAddress, gatePrefabAddress, isCrossAreaAlert);
        }

        private void AddConnectedArea(string areaId, int indexConnection, SpawnableArea connectedArea, int connectedAreaIndexConnection, string connectionTypeId, string fakeViewAddress, string gatePrefabAddress, bool isCrossAreaAlert)
        {
            SpawnableArea subArea;
            int indexConnectionSubArea;
            string subAreaId;
            if (isConnectionFromSubArea(areaId, indexConnection, out subArea, out indexConnectionSubArea, out subAreaId))
            {
                subArea.AddConnectedArea(subAreaId, indexConnectionSubArea, connectedArea, connectedAreaIndexConnection, connectionTypeId, fakeViewAddress, gatePrefabAddress, isCrossAreaAlert);
                return;
            }

            if (_connectedArea == null)
            {
                _connectedArea = new Dictionary<int, ConnectedArea>();
            }

            if (_connectedArea.ContainsKey(indexConnectionSubArea))
            {
                Debug.LogError("error");
            }

            _connectedArea.Add(indexConnectionSubArea, new ConnectedArea(connectedArea, connectedAreaIndexConnection, connectionTypeId, isCrossAreaAlert));

            // SetFakeView
            if (_connectionFakeViewAddress == null)
            {
                _connectionFakeViewAddress = new Dictionary<int, string>();
            }

            _connectionFakeViewAddress.Add(indexConnectionSubArea, fakeViewAddress);

            // Set Gate Prefab
            if (_connectionGateAddress == null)
            {
                _connectionGateAddress = new Dictionary<int, string>();
            }

            _connectionGateAddress.Add(indexConnectionSubArea, gatePrefabAddress);
        }

        public static void DiconnectedAreas(SpawnableArea area1, string area1Id, int indexConnection1, SpawnableArea area2, string area2Id, int indexConnection2)
        {
            area1.RemoveConnectedArea(area1Id, indexConnection1);
            area2.RemoveConnectedArea(area2Id, indexConnection2);
        }

        private void RemoveConnectedArea(string areaId, int indexConnection)
        {
            SpawnableArea subArea;
            int indexConnectionSubArea;
            string subAreaId;
            if (isConnectionFromSubArea(areaId, indexConnection, out subArea, out indexConnectionSubArea, out subAreaId))
            {
                subArea.RemoveConnectedArea(subAreaId, indexConnectionSubArea);
                return;
            }

            if (_connectedArea == null)
            {
                return;
            }

            _connectedArea.Remove(indexConnectionSubArea);

            // Remove Fake View
            if (_connectionFakeViewAddress != null)
            {
                _connectionFakeViewAddress.Remove(indexConnectionSubArea);
            }

            // Remove Gate Prefab
            if (_connectionGateAddress != null)
            {
                _connectionGateAddress.Remove(indexConnectionSubArea);
            }
        }

        public ConnectedArea GetConnectedArea(string areaId, int indexConnection)
        {

            SpawnableArea subArea;
            int indexConnectionSubArea;
            string subAreaId;
            if (isConnectionFromSubArea(areaId, indexConnection, out subArea, out indexConnectionSubArea, out subAreaId))
            {
                return subArea.GetConnectedArea(subAreaId, indexConnectionSubArea);
            }

            if (_connectedArea == null)
            {
                return null;
            }

            ConnectedArea result = null;
            if (_connectedArea.TryGetValue(indexConnectionSubArea, out result))
            {
                return result;
            }

            return null;
        }

        public FakeViewData GetFakeViewData(int connectionIndex)
        {
            if (_connectionFakeViewData == null) return null;
            if (_connectionFakeViewData.TryGetValue(connectionIndex, out FakeViewData fakeViewData))
            {
                return fakeViewData;
            }

            return null;
        }

        public void SetCreatureData(int numberCreature, bool isSahredNPCAlert)
        {
            _numberCreature = numberCreature;
            _isSharedNPCAlert = isSahredNPCAlert;
        }

        private bool isConnectionFromSubArea(string areaId, int indexConnection, out SpawnableArea subArea, out int indexConnectionSubArea, out string subAreaId)
        {
            subArea = null;
            indexConnectionSubArea = indexConnection;
            subAreaId = areaId;

            if (_subAreaTableExternalConnection == null)
            {
                return false;
            }

            List<ExternalConnectionData> areaTableExternalConnection = null;
            if (!_subAreaTableExternalConnection.TryGetValue(areaId, out areaTableExternalConnection))
            {
                return false;
            }

            if (indexConnection < 0 || indexConnection >= areaTableExternalConnection.Count)
            {
                Debug.LogError($"connection Index : {indexConnection} is invalide for sub area Managed");
                return false;
            }

            subAreaId = areaTableExternalConnection[indexConnection].areaId;
            subArea = areaTableExternalConnection[indexConnection].spawnArea;
            indexConnectionSubArea = areaTableExternalConnection[indexConnection].connectionIndex;

            return true;
        }

        public List<SpawnableArea> GetBreadthTree(out List<int> indexDepthChange)
        {
            List<SpawnableArea> resultTree = new List<SpawnableArea>();
            indexDepthChange = new List<int>();

            HashSet<SpawnableArea> markedAreas = new HashSet<SpawnableArea>();
            Queue<SpawnableArea> areaListToCheck = new Queue<SpawnableArea>();
            areaListToCheck.Enqueue(this);
            markedAreas.Add(this);

            int sameDepthCount = 1;
            int nextSameDepthCount = 0;
            int tempDepthCounter = 0;
            int index = 0;
            while (areaListToCheck.Count > 0)
            {
                SpawnableArea tempArea = areaListToCheck.Peek();
                areaListToCheck.Dequeue();

                resultTree.Add(tempArea);
                index++;
                if (tempArea._connectedArea == null)
                {
                    continue;
                }

                foreach (KeyValuePair<int, ConnectedArea> pair in tempArea._connectedArea)
                {
                    SpawnableArea child = pair.Value.connectedArea;
                    if (markedAreas.Add(child))
                    {
                        areaListToCheck.Enqueue(child);
                        nextSameDepthCount++;
                    }
                }

                tempDepthCounter++;
                if (tempDepthCounter == sameDepthCount)
                {
                    tempDepthCounter = 0;
                    sameDepthCount = nextSameDepthCount;
                    nextSameDepthCount = 0;
                    indexDepthChange.Add(index);
                }

            }

            return resultTree;
        }

        public bool IntersectRecursif(Bounds otherBound, float boundsMargin)
        {
            HashSet<SpawnableArea> taggedArea = new HashSet<SpawnableArea>();
            return IntersectRecursif(otherBound, boundsMargin, ref taggedArea);
        }

        private bool IntersectRecursif(Bounds otherBound, float boundsMargin, ref HashSet<SpawnableArea> taggedArea)
        {
            if (!taggedArea.Add(this))
            {
                return false;
            }

            if (Intersects(otherBound, boundsMargin))
            {
                return true;
            }

            if (_connectedArea == null)
            {
                return false;
            }

            foreach (KeyValuePair<int, ConnectedArea> pair in _connectedArea)
            {
                if (pair.Value.connectedArea.IntersectRecursif(otherBound, boundsMargin, ref taggedArea))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IntersectRecursif(IAreaBlueprintGenerator.SpawnableBlueprint bluePrint, float boundsMargin)
        {
            HashSet<SpawnableArea> taggedArea = new HashSet<SpawnableArea>();
            return IntersectRecursif(bluePrint, boundsMargin, ref taggedArea);
        }

        private bool IntersectRecursif(IAreaBlueprintGenerator.SpawnableBlueprint bluePrint, float boundsMargin, ref HashSet<SpawnableArea> taggedArea)
        {
            if (!taggedArea.Add(this))
            {
                return false;
            }

            if (bluePrint.Intersects(_bounds, boundsMargin))
            {
                return true;
            }

            if (_connectedArea == null)
            {
                return false;
            }

            foreach (KeyValuePair<int, ConnectedArea> pair in _connectedArea)
            {
                if (pair.Value.connectedArea.IntersectRecursif(bluePrint, boundsMargin, ref taggedArea))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Intersects(Bounds otherBounds, float boundsMargin)
        {
            // Check Intersection with margin error (areas tend to be side by side which return true with normal intersect)
            float xDistance = Mathf.Abs(_bounds.center.x - otherBounds.center.x);
            float yDistance = Mathf.Abs(_bounds.center.y - otherBounds.center.y);
            float zDistance = Mathf.Abs(_bounds.center.z - otherBounds.center.z);

            float xIntersectDistance = (_bounds.size.x / 2.0f) + (otherBounds.size.x / 2.0f) - boundsMargin;
            float yIntersectDistance = (_bounds.size.y / 2.0f) + (otherBounds.size.y / 2.0f) - boundsMargin;
            float zIntersectDistance = (_bounds.size.z / 2.0f) + (otherBounds.size.z / 2.0f) - boundsMargin;

            return (xDistance < xIntersectDistance
                    && yDistance < yIntersectDistance
                    && zDistance < zIntersectDistance);
        }


        /// <summary>
        /// Find the area that contains the position inb this SpawnableArea and in connected area using a breadth search.
        /// (Breadth search means that it will first look in this area then in the connected areas and after connected areas to connected areas)
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Return the SpawnableArea that contains the position</returns>
        public SpawnableArea FindRecursive(Vector3 position)
        {
            markedAreas ??= new HashSet<SpawnableArea>(16);
            areaListToCheck ??= new Queue<SpawnableArea>(16);
            markedAreas.Clear();
            areaListToCheck.Clear();
            areaListToCheck.Enqueue(this);
            markedAreas.Add(this);

            while (areaListToCheck.Count > 0)
            {
                SpawnableArea tempArea = areaListToCheck.Dequeue();

                if (tempArea._bounds.Contains(position))
                {
                    return tempArea;
                }

                if (tempArea._connectedArea == null)
                {
                    continue;
                }

                foreach (KeyValuePair<int, ConnectedArea> pair in tempArea._connectedArea)
                {
                    SpawnableArea child = pair.Value.connectedArea;
                    if (markedAreas.Add(child))
                    {
                        areaListToCheck.Enqueue(child);
                    }
                }
            }
            return null;
        }

        public bool IsSharedNPCAlert(out List<Creature> alertCreature)
        {
            HashSet<SpawnableArea> taggedArea = new HashSet<SpawnableArea>();
            return IsSharedNPCAlert(out alertCreature, ref taggedArea);
        }
        private bool IsSharedNPCAlert(out List<Creature> alertCreature, ref HashSet<SpawnableArea> taggedArea)
        {
            if (!taggedArea.Add(this))
            {
                alertCreature = null;
                return false;
            }

            if (!_isSharedNPCAlert)
            {
                alertCreature = null;
                return false;
            }

            alertCreature = new List<Creature>();
            if (IsSpawned)
            {
                alertCreature.AddRange(SpawnedArea.creatures);
            }

            if (_connectedArea != null)
            {
                foreach (KeyValuePair<int, ConnectedArea> pair in _connectedArea)
                {
                    if (pair.Value.isCrossAreaAlert)
                    {
                        List<Creature> connectedAreaAlertCreature;
                        if (IsSharedNPCAlert(out connectedAreaAlertCreature, ref taggedArea))
                        {
                            alertCreature.AddRange(connectedAreaAlertCreature);
                        }
                    }
                }
            }

            return true;
        }


        public IEnumerator LoadPrefab()
        {
            // Area prefab
            GameObject prefab = null;
            yield return Catalog.LoadLocationCoroutine<GameObject>(AreaAddress, value => _resourceLocation = value, AreaAddress);
            yield return Catalog.LoadAssetCoroutine<GameObject>(_resourceLocation, value => prefab = value, "SpawnableArea");
            _areaPrefab = prefab.GetComponent<Area>();

            yield return LoadBlockers();

            yield return LoadFakeViews();

            yield return LoadGates();

        }

        private IEnumerator LoadGates()
        {
            // Gate
            _gateResourceLocation ??= new Dictionary<int, IResourceLocation>();
            _connectionGatePrefab ??= new Dictionary<int, GameObject>();

            if (_connectionGateAddress == null) yield break;
            foreach (KeyValuePair<int, string> pair in _connectionGateAddress)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    IResourceLocation tempLocation = null;
                    if (!_gateResourceLocation.TryGetValue(pair.Key, out tempLocation))
                    {
                        yield return Catalog.LoadLocationCoroutine<GameObject>(pair.Value, value => _gateResourceLocation.Add(pair.Key, value), AreaAddress);
                        tempLocation = _gateResourceLocation[pair.Key];
                    }

                    yield return Catalog.LoadAssetCoroutine<GameObject>(tempLocation,
                        value => _connectionGatePrefab.Add(pair.Key, value),
                        "SpawnableArea");
                }
            }
        }
        private IEnumerator LoadFakeViews()
        {
            // FakeView
            _fakeViewResourceLocation ??= new Dictionary<int, IResourceLocation>();
            _connectionFakeViewData ??= new Dictionary<int, FakeViewData>();
            if (_connectionFakeViewAddress == null) yield break;

            foreach (KeyValuePair<int, string> pair in _connectionFakeViewAddress)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    IResourceLocation tempLocation = null;
                    if (!_fakeViewResourceLocation.TryGetValue(pair.Key, out tempLocation))
                    {
                        yield return Catalog.LoadLocationCoroutine<FakeViewData>(pair.Value, value => _fakeViewResourceLocation.Add(pair.Key, value), AreaAddress);
                        tempLocation = _fakeViewResourceLocation[pair.Key];
                    }

                    yield return Catalog.LoadAssetCoroutine<FakeViewData>(tempLocation,
                        value => _connectionFakeViewData.Add(pair.Key, value),
                        "SpawnableArea");
                }
            }
        }
        private IEnumerator LoadBlockers()
        {
            // Blocker
            _blockerResourceLocation ??= new Dictionary<int, IResourceLocation>();
            _connectionBlockerPrefab ??= new Dictionary<int, GameObject>();
            if (_connectionBlockerAddress == null) yield break;

            foreach (KeyValuePair<int, string> pair in _connectionBlockerAddress)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    IResourceLocation tempLocation = null;
                    if (!_blockerResourceLocation.TryGetValue(pair.Key, out tempLocation))
                    {
                        yield return Catalog.LoadLocationCoroutine<GameObject>(pair.Value, value => _blockerResourceLocation.Add(pair.Key, value), AreaAddress);
                        tempLocation = _blockerResourceLocation[pair.Key];
                    }

                    yield return Catalog.LoadAssetCoroutine<GameObject>(tempLocation,
                        value => _connectionBlockerPrefab.Add(pair.Key, value),
                        "SpawnableArea");
                }
            }
        }

        public void Spawn(bool inLoadingMenu)
        {
            if (_spawnCoroutine != null)
            {
                return;
            }

            if (_areaPrefabSpawn != null)
            {
                return;
            }

            _spawnCoroutine = AreaManager.Instance.StartCoroutine(SpawnCoroutine(inLoadingMenu));
        }

        public IEnumerator SpawnCoroutine(bool inLoadingMenu)
        {
            yield break;
        }

        public void Unload()
        {
            if (_spawnCoroutine != null)
            {
                AreaManager.Instance.StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }

            if (_areaPrefabSpawn == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(_areaPrefabSpawn.gameObject);
            _areaPrefabSpawn = null;

            Catalog.ReleaseAsset(_areaPrefab.gameObject);
            _areaPrefab = null;

            foreach (KeyValuePair<int, FakeViewData> pair in _connectionFakeViewData)
            {
                Catalog.ReleaseAsset(pair.Value);
            }
            _connectionFakeViewData.Clear();
            _connectionFakeViewData = null;

            foreach (KeyValuePair<int, GameObject> pair in _connectionBlockerPrefab)
            {
                Catalog.ReleaseAsset(pair.Value);
            }
            _connectionBlockerPrefab.Clear();
            _connectionBlockerPrefab = null;

            foreach (KeyValuePair<int, GameObject> pair in _connectionGatePrefab)
            {
                Catalog.ReleaseAsset(pair.Value);
                int connection = pair.Key;
                GameObject gateSpawn = null;
                if (_connectionGateSpawn.TryGetValue(connection, out gateSpawn))
                {
                    ConnectedArea connectedAreaInfo = null;
                    if (_connectedArea.TryGetValue(connection, out connectedAreaInfo))
                    {
                        SpawnableArea connectedArea = connectedAreaInfo.connectedArea;
                        int connectedAreaConnectionIndex = connectedAreaInfo.connectedAreaConnectionIndex;
                        if (connectedArea.IsSpawned)
                        {
                            if (!connectedArea._connectionGateSpawn.ContainsKey(connectedAreaConnectionIndex))
                            {
                                gateSpawn.transform.parent = connectedArea._areaPrefabSpawn.transform;
                                connectedArea._connectionGateSpawn.Add(connectedAreaConnectionIndex, gateSpawn);
                                continue;
                            }
                        }
                    }

                    UnityEngine.Object.Destroy(gateSpawn);
                }
            }

            if (_connectionGatePrefab != null)
            {
                _connectionGatePrefab.Clear();
                _connectionGatePrefab = null;
            }

            if (_connectionGateSpawn != null)
            {
                _connectionGateSpawn.Clear();
                _connectionGateSpawn = null;
            }

            if (_connectionLinkNavMesh != null)
            {
                _connectionLinkNavMesh.Clear();
                _connectionLinkNavMesh = null;
            }

            if (_despawnAreaEvent != null) _despawnAreaEvent.Invoke(this);
        }

        public void OnPlayerEnter(SpawnableArea previousArea)
        {
            if (!IsSpawned)
            {
                return;
            }

            _areaPrefabSpawn.OnPlayerEnter(previousArea.SpawnedArea);
        }

        public void OnPlayerExit(SpawnableArea newArea)
        {
            if (!IsSpawned)
            {
                return;
            }

            _areaPrefabSpawn.OnPlayerExit(newArea.SpawnedArea);
        }

        public void OnCreatureKill(int areaSpawnerIndex)
        {
            if (!_respawnDeadCreature
                && isCreatureDead != null
                && areaSpawnerIndex >= 0
                && areaSpawnerIndex < isCreatureDead.Length)
            {
                isCreatureDead[areaSpawnerIndex] = true;
            }
        }

        #endregion Methods

        #region Tools

#if UNITY_EDITOR
        public void EditorSpawnArea(Transform root, bool spawnItem)
        {
            EditorSpawnArea(root, spawnItem, AreaAddress);
        }

        private void EditorSpawnArea(Transform root, bool spawnItem, string areaAddress)
        {
            
        }
#endif //UNITY_EDITOR

        #endregion Tools
    }
}
