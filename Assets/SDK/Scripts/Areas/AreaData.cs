using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
 // ProjectCore
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
return false;
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

            [NonSerialized]
            public IResourceLocation fakeViewAddressLocation;
            private FakeViewData _fakeviewData;
            private int _fakeviewDataCounter = 0;

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
                if (other?.connectionTypeIdContainerList == null)
                {
                    connectionType = null;
                    return false;
                }

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

            public void GetFakeviewData(Action<FakeViewData> callback)
            {
                if (_fakeviewData != null)
                {
                    if (callback != null)
                        callback(_fakeviewData);
                    _fakeviewDataCounter++;
                    return;
                }

                Catalog.LoadAssetAsync<FakeViewData>(fakeViewAddressLocation,
                    (FakeViewData fakeview) =>
                    {
                        _fakeviewDataCounter++;
                        _fakeviewData = fakeview;
                        if (callback != null)
                            callback(_fakeviewData);
                    }, fakeViewAddress);
            }

            public void ReleaseFakeview()
            {
                _fakeviewDataCounter--;
                if (_fakeviewDataCounter <= 0)
                {
                    Catalog.ReleaseAsset(_fakeviewData);
                    _fakeviewData = null;
                }
            }
        }
#endregion InternalClass

        #region Data

        public bool isUnique = true;
        public string areaPrefabAddress;
        public string lightingPresetAddress;
        public string cullingAddress;

        [NonSerialized]
        public IResourceLocation areaPrefabAddressLocation;

        [NonSerialized]
        public IResourceLocation lightingPresetAddressLocation;

        [NonSerialized]
        public IResourceLocation cullingAddressLocation;

        public Bounds Bounds;
        public List<AreaRotationHelper.Rotation> allowedRotation;
        public List<AreaConnection> connections;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public AreaGlobalParameter[] areaGlobalParameters;
        #endregion Data

        #region Fields
        private Area _areaPrefab;
        private int _areaPrefabLoadCounter = 0;
        private LightingPreset _lightingPreset;
        private int _lightingPresetLoadCounter = 0;
        private int _cullingVolumeBakeDataLoadCounter = 0;

        #endregion Fields

        #region Methods
        public string GetId()
        {
            return id;
        }

        public HashSet<string> GetSpawnableAreasIds()
        {
            return new HashSet<string> { GetId() };
        }

        public List<AreaConnection> GetConnections()
        {
            return connections;
        }

        public AreaConnection GetConnection(int index)
        {
            if (index < 0 || index >= connections.Count)
                return null;
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
return default;
        }

        public IAreaBlueprintGenerator.SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition)
        {
return default;
        }

        public void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp, int numberCreature, bool isShareNPCAlert)
        {
        }

        public IEnumerator GetAreaPrefab(Action<Area> callback)
        {
yield break;
        }

        public void ReleaseAreaPrefab()
        {
        }

        public IEnumerator GetLightingPreset(Action<LightingPreset> callback)
        {
yield break;
        }

        public void ReleaseLightingPreset()
        {
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
yield break;
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
                if (disableOnCondition[i].condition == DisableOnCondition.Condition.OnPlay)
                {
                    disableOnCondition[i].gameObject.SetActive(false);
                }
            }
        }

        public void PreviewArea(bool spawnItem = false)
        {
        }

        public void ForceBoudaryStopAtConnection()
        {
        }

        public void SetAllRotation()
        {
            allowedRotation = new List<AreaRotationHelper.Rotation>() { AreaRotationHelper.Rotation.Front,
                                                                        AreaRotationHelper.Rotation.Back,
                                                                        AreaRotationHelper.Rotation.Left,
                                                                        AreaRotationHelper.Rotation.Right };
        }

        public void BakeConnectionFakeView(bool openRoom = true)
        {
        }
#endif //UNITY_EDITOR
#endregion Tools
    }
}
