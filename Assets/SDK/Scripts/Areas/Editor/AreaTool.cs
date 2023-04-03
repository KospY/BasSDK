namespace ThunderRoad
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.VFX;
    public class AreaTool
    {
        [MenuItem("Assets/ThunderRoad/AreaTool/CheckAllAreaBoundaries")]
        public static void CheckAllAreaBoundaries()
        {
            Catalog.EditorLoadAllJson();
            List<CatalogData> areaList = Catalog.GetDataList(Category.Area);
            for (int i = 0; i < areaList.Count; i++)
            {
                if (areaList[i] is AreaData areaData)
                {
                    areaData.PreviewArea();
                    Area tempArea = Object.FindObjectOfType<Area>();
                    if (tempArea == null) continue;

                    tempArea.UpdateDataConnectionPositionAndDirection();
                    tempArea.UpdateDataBounds();
                }
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/SetAllAreaSunGlobalParameter")]
        public static void SetAllAreaSunGlobalParameterries()
        {
            Catalog.EditorLoadAllJson();
            List<CatalogData> areaList = Catalog.GetDataList(Category.Area);
            for (int i = 0; i < areaList.Count; i++)
            {
                if (areaList[i] is AreaData areaData)
                {
                    areaData.PreviewArea();
                    Area tempArea = null;
                    List<GameObject> rootObjects = new List<GameObject>();
                    Scene scene = SceneManager.GetActiveScene();
                    scene.GetRootGameObjects(rootObjects);
                    foreach (GameObject go in rootObjects)
                    {
                        tempArea = go.GetComponentInChildren<Area>();
                        if (tempArea != null) break;
                    }

                    if (tempArea == null) continue;

                    List<AreaGlobalParameter> parameter = new List<AreaGlobalParameter>();
                    if (areaData.areaGlobalParameters != null) parameter.AddRange(areaData.areaGlobalParameters);

                    if (tempArea.lightingGroup != null
                        && tempArea.lightingGroup.lightingPreset != null
                        && tempArea.lightingGroup.lightingPreset.applyAtRuntime)
                    {
                        AreaSunGlobalParameter sunParameter = null;
                        for (int indexParameter = 0; indexParameter < parameter.Count; indexParameter++)
                        {
                            if (parameter[indexParameter] is AreaSunGlobalParameter tempSun)
                            {
                                sunParameter = tempSun;
                                break;
                            }
                        }

                        if (sunParameter == null)
                        {
                            sunParameter = new AreaSunGlobalParameter();
                            parameter.Add(sunParameter);
                        }

                        sunParameter.dirLightColor = tempArea.lightingGroup.lightingPreset.dirLightColor;
                        sunParameter.dirLightIntensity = tempArea.lightingGroup.lightingPreset.dirLightIntensity;
                        sunParameter.dirLightIndirectMultiplier = tempArea.lightingGroup.lightingPreset.dirLightIndirectMultiplier;
                        sunParameter.directionalLightLocalRotation = tempArea.lightingGroup.lightingPreset.directionalLightLocalRotation;
                    }
                    else
                    {
                        int sunParameterIndex = -1;
                        for (int indexParameter = 0; indexParameter < parameter.Count; indexParameter++)
                        {
                            if (parameter[indexParameter] is AreaSunGlobalParameter)
                            {
                                sunParameterIndex = indexParameter;
                                break;
                            }
                        }

                        if (sunParameterIndex >= 0) parameter.RemoveAt(sunParameterIndex);
                    }

                    areaData.areaGlobalParameters = parameter.ToArray();

                    Catalog.SaveToJson(areaData);
                }
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/SetAllAreaOceanGlobalParameter")]
        public static void SetAllAreaOceanGlobalParameterries()
        {
            Catalog.EditorLoadAllJson();
            List<CatalogData> areaList = Catalog.GetDataList(Category.Area);
            for (int i = 0; i < areaList.Count; i++)
            {
                if (areaList[i] is AreaData areaData)
                {
                    areaData.PreviewArea();
                    Area tempArea = null;
                    List<GameObject> rootObjects = new List<GameObject>();
                    Scene scene = SceneManager.GetActiveScene();
                    scene.GetRootGameObjects(rootObjects);
                    foreach (GameObject go in rootObjects)
                    {
                        tempArea = go.GetComponentInChildren<Area>();
                        if (tempArea != null) break;
                    }

                    if (tempArea == null) continue;

                    List<AreaGlobalParameter> parameter = new List<AreaGlobalParameter>();
                    if (areaData.areaGlobalParameters != null) parameter.AddRange(areaData.areaGlobalParameters);

                    Ocean ocean = tempArea.GetComponentInChildren<Ocean>();
                    if (ocean != null)
                    {
                        AreaOceanGlobalParameter oceanParameter = null;
                        for (int indexParameter = 0; indexParameter < parameter.Count; indexParameter++)
                        {
                            if (parameter[indexParameter] is AreaOceanGlobalParameter tempOcean)
                            {
                                oceanParameter = tempOcean;
                                break;
                            }
                        }

                        if (oceanParameter == null)
                        {
                            oceanParameter = new AreaOceanGlobalParameter();
                            parameter.Add(oceanParameter);
                        }

                        // Todo set Ocean Data
                        oceanParameter.visibleFromGate = !ocean.showWhenInRoomOnly;
                    }
                    else
                    {
                        int oceanParameterIndex = -1;
                        for (int indexParameter = 0; indexParameter < parameter.Count; indexParameter++)
                        {
                            if (parameter[indexParameter] is AreaOceanGlobalParameter)
                            {
                                oceanParameterIndex = indexParameter;
                                break;
                            }
                        }

                        if (oceanParameterIndex >= 0) parameter.RemoveAt(oceanParameterIndex);
                    }

                    areaData.areaGlobalParameters = parameter.ToArray();

                    Catalog.SaveToJson(areaData);
                }
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/SetShaderToAllCullingVolumeToAllAreas")]
        public static void SetShaderToAllCullingVolumeToAllAreas()
        {
            ComputeShader samplingPositionComputeShader = (ComputeShader)AssetDatabase.LoadAssetAtPath("Assets/Plugins/Perfect Culling/Scripts/SamplingPositionsWorldComputeShader.compute", typeof(ComputeShader));
            Catalog.EditorLoadAllJson();
            List<string> areaIds = Catalog.GetAllID<AreaData>();
            int count = areaIds.Count;
            for (int i = 0; i < count; i++)
            {
                AreaData areaData = Catalog.GetData<AreaData>(areaIds[i]);
                if (areaData.IsSpawnable())
                {
                    GameObject area = Catalog.EditorLoad<GameObject>(areaData.areaPrefabAddress);
                    if (area != null)
                    {
                        string prefabAdress = AssetDatabase.GetAssetPath(area);
                        UpdatePerfectCullingShader(prefabAdress, samplingPositionComputeShader);
                    }
                    else
                    {
                        Debug.LogError("Can not find area : " + areaIds[i]);
                    }
                }
                else
                {
                    Debug.LogError("Can not find area at address: " + areaData.areaPrefabAddress);
                }
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/AllAreaResestImport")]
        public static void AllAreaResestImport()
        {
            List<string> allArea = GetAllAreas();
            int count = allArea.Count;
            for (int indexArea = 0; indexArea < count; indexArea++)
            {
                AreaResestImport(allArea[indexArea]);
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/AllAreasCheckAndroidLightingPreset")]
        public static void AllAreasCheckAndroidLightingPreset()
        {
            Platform initialPlateform = Common.GetPlatform();

            Catalog.EditorLoadAllJson();
            List<string> areaIds = Catalog.GetAllID<AreaData>();
            int count = areaIds.Count;
            for (int indexArea = 0; indexArea < count; indexArea++)
            {
                AreaData areaData = Catalog.GetData<AreaData>(areaIds[indexArea]);
                IAreaBlueprintGenerator bp = areaData as IAreaBlueprintGenerator;
                if (bp == null) continue;
                Common.SetPlatform(Platform.Windows);
                if( !bp.IsSpawnable()) continue;
                Area windowsArea = Catalog.EditorLoad<Area>(areaData.areaPrefabAddress);
                LightingPreset windowsPreset = windowsArea.lightingGroup.lightingPreset;


                Common.SetPlatform(Platform.Android);
                if (!bp.IsSpawnable()) continue;
                Area androidArea = Catalog.EditorLoad<Area>(areaData.areaPrefabAddress);
                LightingPreset androidPreset = androidArea.lightingGroup.lightingPreset;

                if (windowsPreset.ambientIntensity != androidPreset.ambientIntensity)
                {
                    androidPreset.ambientIntensity = windowsPreset.ambientIntensity;
                }

                if (windowsPreset.shadowColor != androidPreset.shadowColor)
                {
                    androidPreset.shadowColor = windowsPreset.shadowColor;
                }


                if (windowsPreset.indirectIntensity != androidPreset.indirectIntensity)
                {
                    androidPreset.indirectIntensity = windowsPreset.indirectIntensity;
                }

                if (windowsPreset.AOIndirectContribution != androidPreset.AOIndirectContribution)
                {
                    androidPreset.AOIndirectContribution = windowsPreset.AOIndirectContribution;
                }

                if (windowsPreset.AODirectContribution != androidPreset.AODirectContribution)
                {
                    androidPreset.AODirectContribution = windowsPreset.AODirectContribution;
                }

                if (windowsPreset.applyAtRuntime != androidPreset.applyAtRuntime)
                {
                    androidPreset.applyAtRuntime = windowsPreset.applyAtRuntime;
                }

                if (windowsPreset.dirLightColor != androidPreset.dirLightColor)
                {
                    androidPreset.dirLightColor = windowsPreset.dirLightColor;
                }

                if (windowsPreset.dirLightIntensity != androidPreset.dirLightIntensity)
                {
                    androidPreset.dirLightIntensity = windowsPreset.dirLightIntensity;
                }

                if (windowsPreset.dirLightIndirectMultiplier != androidPreset.dirLightIndirectMultiplier)
                {
                    androidPreset.dirLightIndirectMultiplier = windowsPreset.dirLightIndirectMultiplier;
                }

                if (windowsPreset.directionalLightLocalRotation != androidPreset.directionalLightLocalRotation)
                {
                    androidPreset.directionalLightLocalRotation = windowsPreset.directionalLightLocalRotation;
                }

                if (windowsPreset.fog != androidPreset.fog)
                {
                    androidPreset.fog = windowsPreset.fog;
                }

                if (windowsPreset.fogColor != androidPreset.fogColor)
                {
                    androidPreset.fogColor = windowsPreset.fogColor;
                }
                if (windowsPreset.fogStartDistance != androidPreset.fogStartDistance)
                {
                    androidPreset.fogStartDistance = windowsPreset.fogStartDistance;
                }
                if (windowsPreset.fogEndDistance != androidPreset.fogEndDistance)
                {
                    androidPreset.fogEndDistance = windowsPreset.fogEndDistance;
                }

                if (windowsPreset.skybox != androidPreset.skybox)
                {
                    androidPreset.skybox = windowsPreset.skybox;
                }

                if (windowsPreset.skyBoxMaterial != androidPreset.skyBoxMaterial)
                {
                    androidPreset.skyBoxMaterial = windowsPreset.skyBoxMaterial;
                }

                if (windowsPreset.skyBoxSunSize != androidPreset.skyBoxSunSize)
                {
                    androidPreset.skyBoxSunSize = windowsPreset.skyBoxSunSize;
                }

                if (windowsPreset.skyBoxSunConvergence != androidPreset.skyBoxSunConvergence)
                {
                    androidPreset.skyBoxSunConvergence = windowsPreset.skyBoxSunConvergence;
                }
                if (windowsPreset.skyBoxAtmosphereThickness != androidPreset.skyBoxAtmosphereThickness)
                {
                    androidPreset.skyBoxAtmosphereThickness = windowsPreset.skyBoxAtmosphereThickness;
                }
                if (windowsPreset.skyBoxSkyTint != androidPreset.skyBoxSkyTint)
                {
                    androidPreset.skyBoxSkyTint = windowsPreset.skyBoxSkyTint;
                }
                if (windowsPreset.skyBoxGroundTint != androidPreset.skyBoxGroundTint)
                {
                    androidPreset.skyBoxGroundTint = windowsPreset.skyBoxGroundTint;
                }
                if (windowsPreset.skyBoxExposure != androidPreset.skyBoxExposure)
                {
                    androidPreset.skyBoxExposure = windowsPreset.skyBoxExposure;
                }

                if (windowsPreset.clouds != androidPreset.clouds)
                {
                    androidPreset.clouds = windowsPreset.clouds;
                }
                if (windowsPreset.cloudsSoftness != androidPreset.cloudsSoftness)
                {
                    androidPreset.cloudsSoftness = windowsPreset.cloudsSoftness;
                }
                if (windowsPreset.cloudsSpeed != androidPreset.cloudsSpeed)
                {
                    androidPreset.cloudsSpeed = windowsPreset.cloudsSpeed;
                }
                if (windowsPreset.cloudsSize != androidPreset.cloudsSize)
                {
                    androidPreset.cloudsSize = windowsPreset.cloudsSize;
                }
                if (windowsPreset.cloudsAlpha != androidPreset.cloudsAlpha)
                {
                    androidPreset.cloudsAlpha = windowsPreset.cloudsAlpha;
                }
                if (windowsPreset.cloudsColor != androidPreset.cloudsColor)
                {
                    androidPreset.cloudsColor = windowsPreset.cloudsColor;
                }

                EditorUtility.SetDirty(androidPreset);
                AssetDatabase.SaveAssets();
            }

            Common.SetPlatform(initialPlateform);
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/AllAreasBakeFakeView")]
        public static void AllAreasBakeFakeView()
        {
            Catalog.EditorLoadAllJson();
            List<string> areaIds = Catalog.GetAllID<AreaData>();
            int count = areaIds.Count;
            for (int indexArea = 0; indexArea < count; indexArea++)
            {
                AreaData areaData = Catalog.GetData<AreaData>(areaIds[indexArea]);
                areaData.BakeConnectionFakeView();
                AssetDatabase.SaveAssets();
            }
        }


        [MenuItem("Assets/ThunderRoad/AreaTool/AllAreasCleanFakeviewInPrefab")]
        public static void AllAreasCleanFakeviewInPrefab()
        {
            List<string> allArea = GetAllAreas();
            int count = allArea.Count;
            for (int indexArea = 0; indexArea < count; indexArea++)
            {
                AreaCleanFakeviewFromPrefab(allArea[indexArea]);
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/SelectAreaAddDisableComponent")]
        public static void SelectAreaAddDisableComponent()
        {
            if (Selection.activeObject is Area area)
            {
                string prefabAdress = AssetDatabase.GetAssetPath(area);
                AreaAddDisableComponent(prefabAdress);
            }
        }

        [MenuItem("Assets/ThunderRoad/AreaTool/AllAreasAddDisableComponent")]
        public static void AllAreasAddDisableComponent()
        {
            Catalog.EditorLoadAllJson();
            List<string> areaIds = Catalog.GetAllID<AreaData>();
            int count = areaIds.Count;
            for (int indexArea = 0; indexArea < count; indexArea++)
            {
                AreaData areaData = Catalog.GetData<AreaData>(areaIds[indexArea]);
                Debug.Log(areaData.id);
                if (areaData.IsSpawnable())
                {
                    GameObject area = Catalog.EditorLoad<GameObject>(areaData.areaPrefabAddress);
                    if (area != null)
                    {
                        string prefabAdress = AssetDatabase.GetAssetPath(area);
                        AreaAddDisableComponent(prefabAdress);
                    }
                    /*else
                    {
                        Debug.LogError("Can not find area : " + areaIds[indexArea]);
                    }*/
                }
                else
                {
                    Debug.LogError("Can not find area at address : " + areaData.areaPrefabAddress);
                }
            }
        }

        /*[MenuItem("Assets/ThunderRoad/AreaTool/ConvertLightingPresetToLightmapMeshRenderDictionary")]
        public static void ConvertLightingPresetToLightmapMeshRenderDictionary()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(LightingPreset).Name);  //FindAssets uses tags

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                LightingPreset lightingPreset = AssetDatabase.LoadAssetAtPath<LightingPreset>(path);
            }

        }*/

        public static List<string> GetAllRoom()
        {
            List<string> roomAdress = new List<string>();
            List<AddressableAssetEntry> allAssets = new List<AddressableAssetEntry>();
            AddressableAssetSettingsDefaultObject.Settings.GetAllAssets(allAssets, true);

            int count = allAssets.Count;
            for (int i = 0; i < count; i++)
            {
                if (allAssets[i].labels.Contains("Room") && allAssets[i].labels.Contains("Windows"))
                {
                    roomAdress.Add(allAssets[i].address);
                }
            }

            return roomAdress;
        }


        public static List<string> GetAllAreas()
        {
            List<string> roomAdress = new List<string>();
            List<AddressableAssetEntry> allAssets = new List<AddressableAssetEntry>();
            AddressableAssetSettingsDefaultObject.Settings.GetAllAssets(allAssets, true);

            int count = allAssets.Count;
            for (int i = 0; i < count; i++)
            {
                if (allAssets[i].labels.Contains("Area"))
                {
                    roomAdress.Add(allAssets[i].AssetPath);
                }
            }

            return roomAdress;
        }

        public static void UpdatePerfectCullingShader(string prefabAdress, ComputeShader samplingPositionComputeShader)
        {
            
        }

        public static void AreaResestImport(string prefabAdress)
        {
            // Load the contents of the Prefab Asset.
            GameObject root = PrefabUtility.LoadPrefabContents(prefabAdress);

            // Modify Prefab contents.
            Area area = root.GetComponent<Area>();
            area.ResetImport();

            // Save contents back to Prefab Asset and unload contents.
            PrefabUtility.SaveAsPrefabAsset(root, prefabAdress);
            PrefabUtility.UnloadPrefabContents(root);
        }

        public static void AreaCleanFakeviewFromPrefab(string prefabAdress)
        {
            // Load the contents of the Prefab Asset.
            GameObject root = PrefabUtility.LoadPrefabContents(prefabAdress);

            // Modify Prefab contents.
            bool hasChanged = false;
            AreaGateway[] gateways = root.GetComponentsInChildren<AreaGateway>();
            int gatewayCount = gateways.Length;
            for (int i = 0; i < gatewayCount; i++)
            {
                ReflectionSorcery reflectionSorcery = gateways[i].GetComponentInChildren<ReflectionSorcery>();
                if (reflectionSorcery)
                {
                    if (reflectionSorcery.CleanCapturetexture())
                    {
                        hasChanged = true;
                    }
                }
            }

            // Save contents back to Prefab Asset and unload contents.
            if (hasChanged)
            {
                PrefabUtility.SaveAsPrefabAsset(root, prefabAdress);
            }

            PrefabUtility.UnloadPrefabContents(root);
        }

        public static void AreaAddDisableComponent(string prefabAdress)
        {
            // Load the contents of the Prefab Asset.
            GameObject root = PrefabUtility.LoadPrefabContents(prefabAdress);

            // Modify Prefab contents.
            Area area = root.GetComponent<Area>();
            AddDisableComponent(area);

            // Save contents back to Prefab Asset and unload contents.
            PrefabUtility.SaveAsPrefabAsset(root, prefabAdress);
            PrefabUtility.UnloadPrefabContents(root);
        }

        private static void AddDisableComponent(Area area)
        {
            GameObject prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(area.gameObject);
            List<ParticleSystem> particleSystems = new List<ParticleSystem>(area.GetComponentsInChildren<ParticleSystem>());
            List<VisualEffect> visualEffects = new List<VisualEffect>(area.GetComponentsInChildren<VisualEffect>());
            List<Behaviour> behaviourList = new List<Behaviour>();
            List<GameObject> gameObjectList = new List<GameObject>();
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem != null)
                {
                    if (particleSystem.GetComponent<AreaGameObjectDisableOnHide>() != null)
                    {
                        continue;
                    }

                    if (particleSystem.GetComponentInParent<Item>() != null)
                    {
                        // part of an item so not impacted
                        continue;
                    }

                    GameObject tempPrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(particleSystem.gameObject);
                    if (tempPrefab == prefab)
                    {
                        gameObjectList.Add(particleSystem.gameObject);
                    }
                    else
                    {
                        string tempPrefabAdress = UnityEditor.AssetDatabase.GetAssetPath(tempPrefab);
                        // Load the contents of the Prefab Asset.
                        GameObject root = UnityEditor.PrefabUtility.LoadPrefabContents(tempPrefabAdress);
                        if (root.GetComponent<AreaGameObjectDisableOnHide>() != null)
                        {
                            UnityEditor.PrefabUtility.UnloadPrefabContents(root);
                            continue;
                        }

                        // Modify Prefab contents.
                        ParticleSystem[] arrayParticleSystem = root.GetComponentsInChildren<ParticleSystem>();
                        if (arrayParticleSystem.Length > 0)
                        {
                            AreaGameObjectDisableOnHide disabler = root.AddComponent<AreaGameObjectDisableOnHide>();
                            GameObject[] gameObjects = new GameObject[arrayParticleSystem.Length];
                            for (int i = 0; i < arrayParticleSystem.Length; i++)
                            {
                                gameObjects[i] = arrayParticleSystem[i].gameObject;
                            }
                            disabler.SetComponentToHide(gameObjects);

                        }

                        // Save contents back to Prefab Asset and unload contents.
                        UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, tempPrefabAdress);
                        UnityEditor.PrefabUtility.UnloadPrefabContents(root);
                    }
                }
            }

            foreach (VisualEffect visualEffect in visualEffects)
            {
                if (visualEffect.GetComponent<AreaBehaviourDisableOnHide>() != null)
                {
                    continue;
                }

                if (visualEffect.GetComponentInParent<Item>() != null)
                {
                    // part of an item so not impacted
                    continue;
                }

                GameObject tempPrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(visualEffect.gameObject);
                if (tempPrefab == prefab)
                {
                    behaviourList.Add(visualEffect);
                }
                else
                {
                    string tempPrefabAdress = UnityEditor.AssetDatabase.GetAssetPath(tempPrefab);
                    // Load the contents of the Prefab Asset.
                    GameObject root = UnityEditor.PrefabUtility.LoadPrefabContents(tempPrefabAdress);
                    if (root.GetComponent<AreaBehaviourDisableOnHide>() != null)
                    {
                        UnityEditor.PrefabUtility.UnloadPrefabContents(root);
                        continue;
                    }

                    // Modify Prefab contents.
                    VisualEffect[] arrayVisualEffect = root.GetComponentsInChildren<VisualEffect>();
                    if (arrayVisualEffect.Length > 0)
                    {
                        AreaBehaviourDisableOnHide disabler = root.AddComponent<AreaBehaviourDisableOnHide>();
                        disabler.SetComponentToHide(arrayVisualEffect);

                    }

                    // Save contents back to Prefab Asset and unload contents.
                    UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, tempPrefabAdress);
                    UnityEditor.PrefabUtility.UnloadPrefabContents(root);
                }
            }

            if (behaviourList.Count > 0)
            {
                AreaBehaviourDisableOnHide disablerBehaviour = area.GetComponent<AreaBehaviourDisableOnHide>();
                if (disablerBehaviour == null)
                {
                    disablerBehaviour = area.gameObject.AddComponent<AreaBehaviourDisableOnHide>();
                }

                disablerBehaviour.SetComponentToHide(behaviourList.ToArray());
                disablerBehaviour.SetArea(area);
            }

            if (gameObjectList.Count > 0)
            {
                AreaGameObjectDisableOnHide disablerGo = area.gameObject.GetComponent<AreaGameObjectDisableOnHide>();
                if (disablerGo == null)
                {
                    disablerGo = area.gameObject.AddComponent<AreaGameObjectDisableOnHide>();
                }

                disablerGo.SetComponentToHide(gameObjectList.ToArray());
                disablerGo.SetArea(area);
            }
        }

    }
}