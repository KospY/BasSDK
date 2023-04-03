using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif //UNITY_EDITOR

namespace ThunderRoad
{
    public class AreaData : CatalogData, IAreaBlueprintGenerator
    {
        #region InternalClass

        public class AreaBlueprint : IAreaBlueprintGenerator.SpawnableBlueprint
        {
            public SpawnableArea root;

            public AreaBlueprint(AreaRotationHelper.Rotation rotation) : base(rotation) { }

            public override SpawnableArea GetRoot()
            {
                return root;
            }

            public override bool Intersects(Bounds bounds, float margin)
            {
                return root.Intersects(bounds, margin);
            }
        }

        [Serializable]
        public class AreaConnectionTypeIdContainer : DataIdContainer<AreaConnectionTypeData>
        {
            public AreaConnectionTypeIdContainer(string dataId) : base(dataId) { }

            public AreaConnectionTypeIdContainer() : base() { }
        }

        [Serializable]
        public class AreaConnection
        {
            public List<AreaConnectionTypeIdContainer> connectionTypeIdContainerList;
            public AreaConnectionTypeData.PrefabAdressTable overrideBlockerTableAdress;
            public Vector3 position;
            public AreaRotationHelper.Face face;
            public string fakeViewAddress = null;

            public void Refresh()
            {
                if (overrideBlockerTableAdress != null)
                {
                    overrideBlockerTableAdress.CalculateWeight();
                }
            }

            public bool IsConnectionValid(AreaConnection other)
            {
                int otherTypeCount = other.connectionTypeIdContainerList.Count;
                int typeCount = connectionTypeIdContainerList.Count;
                for (int otherTypeIndex = 0; otherTypeIndex < otherTypeCount; otherTypeIndex++)
                {
                    string otherTypeId = other.connectionTypeIdContainerList[otherTypeIndex].dataId;
                    for (int typeIndex = 0; typeIndex < typeCount; typeIndex++)
                    {
                        if (otherTypeId.Equals(connectionTypeIdContainerList[typeIndex].dataId))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            public bool TryGetFirstConnectionValid(AreaConnection other, out AreaConnectionTypeData connectionType)
            {
                int otherTypeCount = other.connectionTypeIdContainerList.Count;
                int typeCount = connectionTypeIdContainerList.Count;
                for (int otherTypeIndex = 0; otherTypeIndex < otherTypeCount; otherTypeIndex++)
                {
                    string otherTypeId = other.connectionTypeIdContainerList[otherTypeIndex].dataId;
                    for (int typeIndex = 0; typeIndex < typeCount; typeIndex++)
                    {
                        if (otherTypeId.Equals(connectionTypeIdContainerList[typeIndex].dataId))
                        {
                            connectionType = connectionTypeIdContainerList[typeIndex].Data;
                            return true;
                        }
                    }
                }

                connectionType = null;
                return false;
            }

            public bool IsVertical()
            {
                return face == AreaRotationHelper.Face.Down || face == AreaRotationHelper.Face.Up;
            }
        }
        #endregion InternalClass

        #region Data
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), LabelWidth(70)]
#endif
        public string groupPath;

        public bool isUnique = true;
        public string areaPrefabAddress;
        [NonSerialized]
        public IResourceLocation areaPrefabAddressLocation;

        public Bounds Bounds;
        public List<AreaRotationHelper.Rotation> allowedRotation;
        public List<AreaConnection> connections;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public AreaGlobalParameter[] areaGlobalParameters;
        #endregion Data

        #region Methods
        public string GetId()
        {
            return id;
        }

        public List<AreaConnection> GetConnections()
        {
            return connections;
        }

        public AreaConnection GetConnection(int index)
        {
            if (index < 0 || index >= connections.Count) return null;
            return connections[index];
        }

        public List<AreaRotationHelper.Rotation> GetAllowedRotation()
        {
            return allowedRotation;
        }

        public bool IsSpawnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return Catalog.EditorExist<Area>(areaPrefabAddress);
            }
#endif //UNITY_EDITOR

            return areaPrefabAddressLocation != null;
        }

        public SpawnableArea GetSpawnableArea(AreaRotationHelper.Rotation rotation,
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

        public IAreaBlueprintGenerator.SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition)
        {
            AreaBlueprint bluePrint = new AreaBlueprint(rotation);
            bluePrint.root = new SpawnableArea(this, rotation, entranceIndex, entrancePosition);
            return bluePrint;
        }

        public void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp, int numberCreature, bool isShareNPCAlert)
        {
            numberCreature = Math.Min(numberCreature, Catalog.gameData.platformParameters.maxRoomNpc);
            if (spawnableBp is AreaBlueprint areaBp)
            {
                areaBp.root.SetCreatureData(numberCreature, isShareNPCAlert);
            }
        }
        #endregion Methods

        #region Tools
        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (connections != null)
            {
                int count = connections.Count;
                for (int i = 0; i < count; i++)
                {
                    connections[i].Refresh();
                }
            }
        }

        public override IEnumerator OnCatalogRefreshCoroutine()
        {
            yield return Catalog.LoadLocationCoroutine<GameObject>(areaPrefabAddress, value => areaPrefabAddressLocation = value, id);
        }

#if UNITY_EDITOR
        [Button]
        public void PreviewAreaAndDisableCondition()
        {
            PreviewAreaWithItemAndDisableCondition(false);
        }

        [Button]
        public void PreviewAreaWithItemAndDisableCondition()
        {
            PreviewAreaWithItemAndDisableCondition(true);
        }

        public void PreviewAreaWithItemAndDisableCondition(bool spawnItem = false)
        {
            PreviewArea(spawnItem);
            DisableOnCondition[] disableOnCondition = UnityEngine.Object.FindObjectsOfType<DisableOnCondition>();
            for (int i = 0; i < disableOnCondition.Length; i++)
            {
                if(disableOnCondition[i].condition == DisableOnCondition.Condition.OnPlay)
                {
                    disableOnCondition[i].gameObject.SetActive(false);
                }
            }
        }

        public void PreviewArea(bool spawnItem = false)
        {
            SceneAsset areaSceneAsset = Catalog.EditorLoad<SceneAsset>(ThunderRoadSettings.current.areaSceneAddress);
            
            if (areaSceneAsset == null)
            {
                Debug.LogError("Area scene at address : " + ThunderRoadSettings.current.areaSceneAddress + " not found");
                return;
            }

            string areaScenePath = AssetDatabase.GetAssetPath(areaSceneAsset);
            UnityEngine.SceneManagement.Scene areaScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(areaScenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

            if (areaScene == null)
            {
                Debug.LogError("Scene : " + areaScenePath + " not found");
                return;
            }

            AreaManager areaManager = UnityEngine.Object.FindObjectOfType<AreaManager>();
            if (areaManager == null)
            {
                GameObject areaManagerRoot = UnityEngine.Object.Instantiate(new GameObject());
                areaManagerRoot.name = "PreviewArea";
                areaManager = areaManagerRoot.AddComponent<AreaManager>();
            }

            IAreaBlueprintGenerator.SpawnableBlueprint bluePrint = GetSpawnableBluePrint(AreaRotationHelper.Rotation.Front, -1, Vector3.zero);
            areaManager.EditorSpawn(bluePrint.GetRoot(), spawnItem);
        }

        [Button]
        public void ForceBoudaryStopAtConnection()
        {
            Vector3 min = Bounds.min;
            Vector3 max = Bounds.max;

            for (int i = 0; i < connections.Count; i++)
            {
                switch (connections[i].face)
                {
                    case AreaRotationHelper.Face.Back:
                        {
                            min.z = Math.Max(connections[i].position.z, min.z);
                        }
                        break;

                    case AreaRotationHelper.Face.Front:
                        {
                            max.z = Math.Min(connections[i].position.z, max.z);
                        }
                        break;

                    case AreaRotationHelper.Face.Left:
                        {
                            min.x = Math.Max(connections[i].position.x, min.x);
                        }
                        break;

                    case AreaRotationHelper.Face.Right:
                        {
                            max.x = Math.Min(connections[i].position.x, max.x);
                        }
                        break;

                    case AreaRotationHelper.Face.Up:
                        {
                            max.y = Math.Min(connections[i].position.y, max.y);
                        }
                        break;
                    case AreaRotationHelper.Face.Down:
                        {
                            min.y = Math.Max(connections[i].position.y, min.y);
                        }
                        break;
                }
            }

            Bounds.SetMinMax(min, max);
        }

        [Button]
        public void SetAllRotation()
        {
            allowedRotation = new List<AreaRotationHelper.Rotation>() { AreaRotationHelper.Rotation.Front,
                                                                        AreaRotationHelper.Rotation.Back,
                                                                        AreaRotationHelper.Rotation.Left,
                                                                        AreaRotationHelper.Rotation.Right };
        }

        [Button]
        public void BakeConnectionFakeView()
        {
            PreviewArea();

            Area area = null;
            List<GameObject> rootObjects = new List<GameObject>();
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);
            foreach (GameObject go in rootObjects)
            {
                area = go.GetComponentInChildren<Area>();
                if (area != null) break;
            }

            if (area == null)
            {
                Debug.LogError("Can not find Area");
                return;
            }

            GameObject areaPrefab = PrefabUtility.GetCorrespondingObjectFromSource(area.gameObject);
            string areaPath = areaPrefab ? AssetDatabase.GetAssetPath(areaPrefab) : area.gameObject.scene.path;
            AddressableAssetEntry areaAssetEntry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(areaPath));

            List<FakeViewData> list = new List<FakeViewData>();
            AreaGateway[] gateways = area.GetComponentsInChildren<AreaGateway>();
            for (int i = 0; i < gateways.Length; i++)
            {
                if (gateways[i].gameObject.activeInHierarchy)
                {
                    ReflectionSorcery fakeView = gateways[i].GetComponentInChildren<ReflectionSorcery>();
                    FakeViewData fakeViewData = null;
                    string fakeViewDataPath = Path.Combine(fakeView.FolderLocation, fakeView.CaptureName + "_FakeViewData.asset");
                    if (string.IsNullOrEmpty(connections[i].fakeViewAddress))
                    {
                        // Create FakeView Data
                        fakeViewData = Common.EditorCreateOrReplaceAsset(ScriptableObject.CreateInstance<FakeViewData>(), fakeViewDataPath);

                        // Create addressable entry 
                        var assetGUID = AssetDatabase.AssetPathToGUID(fakeViewDataPath);
                        var fakeViewEntry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(assetGUID, areaAssetEntry.parentGroup, false, false);

                        fakeViewEntry.address = areaAssetEntry.address + ".FakeView." + i;
                        fakeViewEntry.SetLabel(Platform.Android.ToString(), true, true, false);
                        fakeViewEntry.SetLabel(Platform.Windows.ToString(), true, true, false);

                        connections[i].fakeViewAddress = fakeViewEntry.address;
                        Catalog.SaveToJson(this);
                    }
                    else
                    {
                        fakeViewData = Catalog.EditorLoad<FakeViewData>(connections[i].fakeViewAddress);
                        if (fakeViewData == null)
                        {
                            Debug.LogError("Can not find fake view data for Area : " + id + " with connection " + i + "Create a new one");
                            // Create FakeView Data
                            string newFakeViewDataPath = Path.Combine(fakeView.FolderLocation, fakeView.CaptureName + "_FakeViewData.asset");
                            fakeViewData = Common.EditorCreateOrReplaceAsset(ScriptableObject.CreateInstance<FakeViewData>(), newFakeViewDataPath);

                            // Create addressable entry 
                            var assetGUID = AssetDatabase.AssetPathToGUID(newFakeViewDataPath);
                            var fakeViewEntry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(assetGUID, areaAssetEntry.parentGroup, false, false);

                            fakeViewEntry.address = areaAssetEntry.address + ".FakeView." + i;
                            fakeViewEntry.SetLabel(Platform.Android.ToString(), true, true, false);
                            fakeViewEntry.SetLabel(Platform.Windows.ToString(), true, true, false);

                            connections[i].fakeViewAddress = fakeViewEntry.address;
                            Catalog.SaveToJson(this);
                        }
                        else
                        {
                            string path = AssetDatabase.GetAssetPath(fakeViewData);
                            if (!path.Equals(fakeViewDataPath))
                            {
                                // rename
                                AssetDatabase.RenameAsset(path, fakeView.CaptureName + "_FakeViewData.asset");
                            }
                        }

                    }

                    // Set Cubemap
                    fakeViewData.resolution = fakeView.resolution;
                    fakeViewData.mask = fakeView.mask;
                    fakeViewData.capturePosition = fakeView.capturePosition;
                    fakeViewData.roomVolumePosition = fakeView.roomVolumePosition;
                    fakeViewData.roomVolumeRotation = fakeView.roomVolumeRotation;
                    fakeViewData.roomVolumeScale = fakeView.roomVolumeScale;

                    fakeView.Capture();
                    string texturePath = fakeView.FolderLocation + "/" + fakeView.CaptureName + ".exr";
                    fakeViewData.captureTexture = AssetDatabase.LoadAssetAtPath<Cubemap>(texturePath);
                    fakeViewData.capturedMatrix = fakeView.CapturedMatrix;

                    list.Add(fakeViewData);
                }
            }

            foreach(FakeViewData fakeView in list)
            {
                EditorUtility.SetDirty(fakeView);
            }
            
            AssetDatabase.SaveAssets();

        }
#endif //UNITY_EDITOR
        #endregion Tools
    }
}
