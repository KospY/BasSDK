using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

using TriInspector;

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Level/Lighting Data")]
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/LightingPreset.html")]
    [DrawWithTriInspector]
    [DeclareVerticalGroup("general")]
    [DeclareVerticalGroup("baking")]
    [DeclareVerticalGroup("directionalLight")]
    [DeclareVerticalGroup("fog")]
    [DeclareVerticalGroup("skybox")]
    public class LightingPreset : ScriptableObject
    {
        #region Fields
        // ! WARNING !
        // If you add parameters there, don't forget to update UpdateFrom() method!
        // ! WARNING !

        [Header("Runtime")]
        [Header("General")]
        [GroupNext("general")]
        public float ambientIntensity = 1;
        public Color shadowColor = new Color(0.7f, 0.7f, 0.7f);
        
#if UNITY_EDITOR
        [Header("Baking")]
        [GroupNext("baking")]
        public float bakedShadowAngle = 0.0f;
        public LightingSettings.Sampling multipleImportanceSamplingTool;
        public int directSamples;
        public int indirectSamples;
        public int environmentSamples;
        public int minBounces;
        public int maxBounces;
        [Range(0, 5)]
        public float indirectIntensity = 1;
        [Range(0, 10)]
        public float AOIndirectContribution = 1;
        [Range(0, 10)]
        public float AODirectContribution = 0;
#endif
        [Header("Directional light")]
        [GroupNext("directionalLight")]
        public bool applyAtRuntime;
        public Color dirLightColor = new Color(1, 0.8687521f, 0.7122642f);
        public float dirLightIntensity = 2;
        public float dirLightIndirectMultiplier = 1;
        public Quaternion directionalLightLocalRotation;

        
        [Header("Fog")]
        [GroupNext("fog")]
        public State fog = State.NoChange;
        [ShowIf("fog", State.Enabled)]
        public Color fogColor;
        [ShowIf("fog", State.Enabled)]
        public float fogStartDistance;
        [ShowIf("fog", State.Enabled)]
        public float fogEndDistance;
        
        [Header("Skybox")]
        [GroupNext("skybox")]
        public State skybox = State.NoChange;
        public SerializedMaterialProperties skyBoxMaterialProp = new SerializedMaterialProperties();
        
        [Header("Clouds")]
        [GroupNext("clouds")]
        public State clouds = State.NoChange;

        [ShowIf("clouds", State.Enabled)]
        public float cloudsSoftness = 1;

        [ShowIf("clouds", State.Enabled)]
        public float cloudsSpeed = 1;

        [ShowIf("clouds", State.Enabled)]
        public float cloudsSize = 1;
        
        [ShowIf("clouds", State.Enabled)]
        public float cloudsAlpha = 1;

        [ShowIf("clouds", State.Enabled)]
        public Color cloudsColor;

        public enum State
        {
            NoChange,
            Disabled,
            Enabled,
        }
        [UnGroupNext]
        [Header("Lightmaps")]

        [TableList]
        public List<SerializedLightmapData> serializedLightmaps = new List<SerializedLightmapData>();

        [TableList]
        public List<MeshRendererData> rendererDataListForLightmaps = new List<MeshRendererData>();
        public List<int> indexLightmapsRendererMeshCount = new List<int>();


        [TableList]

        public List<LightmapLightData> lightmapLights = new List<LightmapLightData>();

        [TableList]
        public List<LightProbeVolumeData> lightProbeVolumes = new List<LightProbeVolumeData>();

        [TableList]
        public List<ReflectionProbeData> reflectionProbes = new List<ReflectionProbeData>();

        public List<SphericalHarmonicsL2> bakedProbes;
        public List<Vector3> probePositions;


        [ReadOnly]
        public UDateTime lastSave = DateTime.Now;


        #endregion Fields

        #region InternalClass
        [Serializable]
        public class SerializedLightmapData
        {
            public Texture2D color;
            public Texture2D directional;
            public Texture2D shadowMask;
            public SerializedLightmapData(Texture2D color, Texture2D directional, Texture2D shadowMask)
            {
                this.color = color;
                this.directional = directional;
                this.shadowMask = shadowMask;
            }
            public SerializedLightmapData(LightmapData lightmapData)
            {
                this.color = lightmapData.lightmapColor;
                this.directional = lightmapData.lightmapDir;
                this.shadowMask = lightmapData.shadowMask;
            }

            public LightmapData ToLightmapData()
            {
                LightmapData lightmapData = new LightmapData();
                lightmapData.lightmapColor = color;
                lightmapData.lightmapDir = directional;
                lightmapData.shadowMask = shadowMask;
                return lightmapData;
            }

            public SerializedLightmapData Clone()
            {
                return MemberwiseClone() as SerializedLightmapData;
            }
        }

        [Serializable]
        public class LightmapMeshRendererData
        {
            public string meshRendererGuid;
            public Vector4 offsetScale;
            public bool generateSecondaryUV;
            public Texture2D colorReference;
            public LightmapMeshRendererData(LightingGroup.MeshRendererReference meshRendererReference, SerializedLightmapData serializedLightmapData)
            {
                this.meshRendererGuid = meshRendererReference.guid;
                this.offsetScale = meshRendererReference.meshRenderer.lightmapScaleOffset;
#if UNITY_EDITOR
                MeshFilter meshFilter = meshRendererReference.meshRenderer.GetComponent<MeshFilter>();
                if (meshFilter)
                {
                    if (meshFilter.sharedMesh)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
                        if (!string.IsNullOrEmpty(assetPath))
                        {
                            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                            if (assetImporter && assetImporter is ModelImporter)
                            {
                                generateSecondaryUV = (assetImporter as ModelImporter).generateSecondaryUV;
                            }
                        }
                    }
                }
#endif
                this.colorReference = serializedLightmapData.color;
            }
        }

        [Serializable]
        public class MeshRendererData
        {
            public string meshRendererGuid;
            public Vector4 offsetScale;
            public bool generateSecondaryUV;
            public MeshRendererData(LightingGroup.MeshRendererReference meshRendererReference)
            {
                this.meshRendererGuid = meshRendererReference.guid;
                this.offsetScale = meshRendererReference.meshRenderer.lightmapScaleOffset;
#if UNITY_EDITOR
                MeshFilter meshFilter = meshRendererReference.meshRenderer.GetComponent<MeshFilter>();
                if (meshFilter)
                {
                    if (meshFilter.sharedMesh)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
                        if (!string.IsNullOrEmpty(assetPath))
                        {
                            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                            if (assetImporter && assetImporter is ModelImporter)
                            {
                                generateSecondaryUV = (assetImporter as ModelImporter).generateSecondaryUV;
                            }
                        }
                    }
                }
#endif
            }

            public MeshRendererData(LightmapMeshRendererData data)
            {
                this.meshRendererGuid = data.meshRendererGuid;
                this.offsetScale = data.offsetScale;
                this.generateSecondaryUV = data.generateSecondaryUV;
            }
        }

        [Serializable]
        public class LightmapLightData
        {
            public string lightGuid;
            public int baketype;
            public int mixedLightingMode;
            public int probeOcclusionLightIndex;
            public int occlusionMaskChannel;
            public LightmapLightData(LightingGroup.LightReference lightReference, int mixedLightingMode)
            {
                this.lightGuid = lightReference.guid;
#if UNITY_EDITOR
                this.baketype = (int)lightReference.light.lightmapBakeType;
#endif
                this.probeOcclusionLightIndex = lightReference.light.bakingOutput.probeOcclusionLightIndex;
                this.occlusionMaskChannel = lightReference.light.bakingOutput.occlusionMaskChannel;
                this.mixedLightingMode = mixedLightingMode;
            }
        }

        [Serializable]
        public class LightProbeVolumeData
        {
            public string lightProbeVolumeGuid;
            public Texture3D SHAr;
            public Texture3D SHAg;
            public Texture3D SHAb;
            public Texture3D occ;
            public LightProbeVolumeData(LightingGroup.LightProbeVolumeReference lightProbeVolumeReference)
            {
                this.lightProbeVolumeGuid = lightProbeVolumeReference.guid;
                this.SHAr = lightProbeVolumeReference.lightProbeVolume.SHAr;
                this.SHAg = lightProbeVolumeReference.lightProbeVolume.SHAg;
                this.SHAb = lightProbeVolumeReference.lightProbeVolume.SHAb;
                this.occ = lightProbeVolumeReference.lightProbeVolume.occ;
            }
        }

        [Serializable]
        public class ReflectionProbeData
        {
            public string reflectionProbeGuid;
            public Texture texture;
            public ReflectionProbeData(LightingGroup.ReflectionProbeReference reflectionProbeReference)
            {
                this.reflectionProbeGuid = reflectionProbeReference.guid;
                this.texture = reflectionProbeReference.reflectionProbe.bakedTexture;
            }
        }
        #endregion InternalClass

        #region Methods
        /// <summary>
        /// Call this method when releasing preset so it destroys/release instanciated object in it
        /// </summary>
        public void OnReleasePreset()
        {
            skyBoxMaterialProp?.ReleaseInstancedMaterial();
        }

        public void UpdateFrom(LightingPreset source, out bool needRebake)
        {
            needRebake = false;

            // General
            if (source.ambientIntensity != ambientIntensity)
            {
                ambientIntensity = source.ambientIntensity;
                needRebake = true;
            }
            shadowColor = source.shadowColor;

#if UNITY_EDITOR            
            // Baking

            if (source.bakedShadowAngle != bakedShadowAngle)
            {
                bakedShadowAngle = source.bakedShadowAngle;
                needRebake = true;
            }
            if (source.multipleImportanceSamplingTool != multipleImportanceSamplingTool)
            {
                multipleImportanceSamplingTool = source.multipleImportanceSamplingTool;
                needRebake = true;
            }
            if (source.directSamples != directSamples)
            {
                directSamples = source.directSamples;
                needRebake = true;
            }
            if (source.indirectSamples != indirectSamples)
            {
                indirectSamples = source.indirectSamples;
                needRebake = true;
            }
            if (source.environmentSamples != environmentSamples)
            {
                environmentSamples = source.environmentSamples;
                needRebake = true;
            }
            if (source.minBounces != minBounces)
            {
                minBounces = source.minBounces;
                needRebake = true;
            }
            if (source.maxBounces != maxBounces)
            {
                maxBounces = source.maxBounces;
                needRebake = true;
            }
            if (source.indirectIntensity != indirectIntensity)
            {
                indirectIntensity = source.indirectIntensity;
                needRebake = true;
            }
            if (source.AOIndirectContribution != AOIndirectContribution)
            {
                AOIndirectContribution = source.AOIndirectContribution;
                needRebake = true;
            }
            if (source.AODirectContribution != AODirectContribution)
            {
                AODirectContribution = source.AODirectContribution;
                needRebake = true;
            }
#endif
            // Directional light
            if (source.applyAtRuntime != applyAtRuntime)
            {
                applyAtRuntime = source.applyAtRuntime;
                needRebake = true;
            }
            if (source.dirLightColor != dirLightColor)
            {
                dirLightColor = source.dirLightColor;
                needRebake = true;
            }
            if (source.dirLightIntensity != dirLightIntensity)
            {
                dirLightIntensity = source.dirLightIntensity;
                needRebake = true;
            }
            if (source.dirLightIndirectMultiplier != dirLightIndirectMultiplier)
            {
                dirLightIndirectMultiplier = source.dirLightIndirectMultiplier;
                needRebake = true;
            }
            if (source.directionalLightLocalRotation != directionalLightLocalRotation)
            {
                directionalLightLocalRotation = source.directionalLightLocalRotation;
                needRebake = true;
            }

            // Fog
            fog = source.fog;
            fogColor = source.fogColor;
            fogStartDistance = source.fogStartDistance;
            fogEndDistance = source.fogEndDistance;

            // Skybox
            if (source.skybox != skybox)
            {
                skybox = source.skybox;
                needRebake = true;
            }

            if (skyBoxMaterialProp.CopyFrom(source.skyBoxMaterialProp))
            {
                needRebake = true;
            }

            // Clouds
            clouds = source.clouds;
            cloudsSoftness = source.cloudsSoftness;
            cloudsSpeed = source.cloudsSpeed;
            cloudsSize = source.cloudsSize;
            cloudsAlpha = source.cloudsAlpha;
            cloudsColor = source.cloudsColor;
        }

        private void OnValidate()
        {
            ValidateFogParameters();
        }

        public void ValidateFogParameters()
        {
            if (fogStartDistance < 0) fogStartDistance = 0;
            if (fogEndDistance < 0) fogEndDistance = 0;
            if (fogStartDistance > fogEndDistance)
            {
                fogEndDistance = fogStartDistance + 0.01f;
            }
        }
        
        private string GetPresetName()
		{
			string presetName = this.name;
			//strip off the "_LightingPreset" part of the name and _Android
			presetName = presetName.Replace("_LightingPreset", "").Replace("_Android", "");
			return presetName;
		}
        
#if UNITY_EDITOR
        //button that renames the referenced lightmap files to include the name of the preset
        [Button]
        public void UpdateAssetNames()
        {
	        //get the name of this preset
	        string presetName = GetPresetName();
	        string path = "";
	        string newPath = "";
	        foreach (var lightmap in serializedLightmaps)
	        {
		        if (lightmap.color != null && !lightmap.color.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightmap.color);
			        newPath = path.Replace(lightmap.color.name, $"{presetName}_{lightmap.color.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
		        if (lightmap.directional != null && !lightmap.directional.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightmap.directional);
			        newPath = path.Replace(lightmap.directional.name, $"{presetName}_{lightmap.directional.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
		        if (lightmap.shadowMask != null && !lightmap.shadowMask.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightmap.shadowMask);
			        newPath = path.Replace(lightmap.shadowMask.name, $"{presetName}_{lightmap.shadowMask.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
	        }
	        foreach (LightProbeVolumeData lightProbeVolumeData in lightProbeVolumes)
	        {
		        if (lightProbeVolumeData.SHAr != null && !lightProbeVolumeData.SHAr.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightProbeVolumeData.SHAr);
			        newPath = path.Replace(lightProbeVolumeData.SHAr.name, $"{presetName}_{lightProbeVolumeData.SHAr.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
		        if (lightProbeVolumeData.SHAg != null && !lightProbeVolumeData.SHAg.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightProbeVolumeData.SHAg);
			        newPath = path.Replace(lightProbeVolumeData.SHAg.name, $"{presetName}_{lightProbeVolumeData.SHAg.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
		        if (lightProbeVolumeData.SHAb != null && !lightProbeVolumeData.SHAb.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightProbeVolumeData.SHAb);
			        newPath = path.Replace(lightProbeVolumeData.SHAb.name, $"{presetName}_{lightProbeVolumeData.SHAb.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
		        if (lightProbeVolumeData.occ != null && !lightProbeVolumeData.occ.name.Contains(presetName))
		        {
			        path = AssetDatabase.GetAssetPath(lightProbeVolumeData.occ);
			        newPath = path.Replace(lightProbeVolumeData.occ.name, $"{presetName}_{lightProbeVolumeData.occ.name}");
			        AssetDatabase.DeleteAsset(newPath); //delete the asset if it already exists
			        string result = AssetDatabase.RenameAsset(path, Path.GetFileName(newPath));
			        if (!string.IsNullOrEmpty(result))
			        {
				        Debug.LogError($"Failed to rename asset: {result}");
			        }
		        }
	        }
        }
#endif

        public bool TryGetSerializedLightmapData(Texture2D color, out SerializedLightmapData serializedLightmapData)
        {
            for (int i = 0; i < serializedLightmaps.Count; i++)
            {
                if (serializedLightmaps[i].color == color)
                {
                    serializedLightmapData = serializedLightmaps[i];
                    return true;
                }
            }
            serializedLightmapData = null;
            return false;
        }

        public bool TryGetSerializedLightmapData(Texture2D color, out SerializedLightmapData serializedLightmapData, out int index)
        {
            for (int i = 0; i < serializedLightmaps.Count; i++)
            {
                if (serializedLightmaps[i].color == color)
                {
                    serializedLightmapData = serializedLightmaps[i];
                    index = i;
                    return true;
                }
            }
            serializedLightmapData = null;
            index = -1;
            return false;
        }

        public bool TryGetReflectionProbeData(Texture texture, out ReflectionProbeData reflectionProbeData)
        {
            for (int i = 0; i < reflectionProbes.Count; i++)
            {
                if (reflectionProbes[i].texture == texture)
                {
                    reflectionProbeData = reflectionProbes[i];
                    return true;
                }
            }
            reflectionProbeData = null;
            return false;
        }

        protected bool TryGetLightingGroupInScene(out LightingGroup lightingGroup)
        {
            lightingGroup = GameObject.FindObjectOfType<LightingGroup>();
            return lightingGroup;
        }

#if UNITY_EDITOR

        public static LightingPreset Create(Component owner, string extraName = null)
        {
            GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(owner.gameObject);
            string sourcePath = sourcePrefab ? AssetDatabase.GetAssetPath(sourcePrefab) : owner.gameObject.scene.path;
            string sourceFolderPath = Path.GetDirectoryName(sourcePath);
            string sourceName = Path.GetFileNameWithoutExtension(sourcePath);
            if (extraName != null) sourceName += $"_{extraName}";
            string newLightingPresetPath = Path.Combine(sourceFolderPath, $"{sourceName}_LightingPreset.asset");
            Common.EditorCreateOrReplaceAsset(ScriptableObject.CreateInstance<LightingPreset>(), newLightingPresetPath);
            LightingPreset newLightingPreset = AssetDatabase.LoadAssetAtPath<LightingPreset>(newLightingPresetPath);
            newLightingPreset.GetSceneSettings();
            return newLightingPreset;
        }

        public void SaveLightingGroup(LightingGroup lightingGroup, string destinationFolderPath, bool probeVolumeOnly = false, bool saveBakedReflectionProbe = true)
        {
            bool lightmapsUpdated = false;
            List<string> failedPaths = new List<string>();

            if (!probeVolumeOnly)
            {
                // Move or copy lightmaps to safe place
                List<string> sourceLightmapPaths = new List<string>();
                List<LightmapData> sceneLightmaps = new List<LightmapData>(LightmapSettings.lightmaps);
                for (int i = sceneLightmaps.Count - 1; i >= 0; i--)
                {
                    if (!sceneLightmaps[i].lightmapColor) continue;
                    SerializedLightmapData serializedLightmapData;
                    if (TryGetSerializedLightmapData(sceneLightmaps[i].lightmapColor, out serializedLightmapData))
                    {
                        // Scene lightmap is already the preset one
                        if (lightingGroup.changeLightmapFormatForAndroid && !IsLightmapUseCorrectFormatForAndroid(sceneLightmaps[i].lightmapColor))
                        {
                            SetLightmapFormatForAndroid(sceneLightmaps[i].lightmapColor);
                        }
                    }
                    else
                    {
                        lightmapsUpdated = true;
                        Texture2D destTexture = CopyFile(sceneLightmaps[i].lightmapColor, destinationFolderPath, sourceLightmapPaths);
                        if (destTexture != sceneLightmaps[i].lightmapColor)
                        {
                            sceneLightmaps[i].lightmapColor = destTexture;
                            if (lightingGroup.changeLightmapFormatForAndroid) SetLightmapFormatForAndroid(sceneLightmaps[i].lightmapColor);
                        }

                        if (sceneLightmaps[i].lightmapDir)
                        {
                            Texture2D destTextureDir = CopyFile(sceneLightmaps[i].lightmapDir, destinationFolderPath, sourceLightmapPaths);
                            if (destTextureDir != sceneLightmaps[i].lightmapDir)
                            {
                                sceneLightmaps[i].lightmapDir = destTextureDir;
                                SetLightmapFormatForDirectional(sceneLightmaps[i].lightmapDir);
                            }
                        }
                        if (sceneLightmaps[i].shadowMask)
                        {
                            sceneLightmaps[i].shadowMask = CopyFile(sceneLightmaps[i].shadowMask, destinationFolderPath, sourceLightmapPaths);
                        }
                    }
                }
                LightmapSettings.lightmaps = sceneLightmaps.ToArray();
                AssetDatabase.DeleteAssets(sourceLightmapPaths.ToArray(), failedPaths);

                serializedLightmaps.Clear();
                rendererDataListForLightmaps.Clear();
                indexLightmapsRendererMeshCount.Clear();

                Dictionary<int, List<MeshRendererData>> mappingSerializedLightmapIndexRendererDataList = new Dictionary<int, List<MeshRendererData>>();

                foreach (var meshRendererReference in lightingGroup.meshRendererReferences)
                {
                    if (meshRendererReference.meshRenderer.lightmapIndex >= LightmapSettings.lightmaps.Length)
                    {
                        Debug.LogErrorFormat(meshRendererReference.meshRenderer, "MeshRenderer: " + meshRendererReference.meshRenderer.name + " use a lightmapIndex that don't exist");
                        continue;
                    }
                    if (meshRendererReference.meshRenderer.lightmapIndex < 0)
                    {
                        Debug.LogErrorFormat(meshRendererReference.meshRenderer, "MeshRenderer: " + meshRendererReference.meshRenderer.name + " don't have any lightmapIndex set");
                        continue;
                    }

                    LightmapData lightmapData = LightmapSettings.lightmaps[meshRendererReference.meshRenderer.lightmapIndex];
                    int index = -1;
                    if (TryGetSerializedLightmapData(lightmapData.lightmapColor, out SerializedLightmapData serializedLightmapData, out index))
                    {
                        mappingSerializedLightmapIndexRendererDataList[index].Add(new MeshRendererData(meshRendererReference));
                    }
                    else
                    {
                        serializedLightmapData = new SerializedLightmapData(lightmapData);
                        serializedLightmaps.Add(serializedLightmapData);
                        index = serializedLightmaps.Count - 1;
                        List<MeshRendererData> meshList = new List<MeshRendererData>();
                        meshList.Add(new MeshRendererData(meshRendererReference));
                        mappingSerializedLightmapIndexRendererDataList.Add(index, meshList);
                    }
                }

                int lightmapCount = mappingSerializedLightmapIndexRendererDataList.Count;
                for (int i = 0; i < lightmapCount; i++)
                {
                    if (mappingSerializedLightmapIndexRendererDataList.ContainsKey(i))
                    {
                        rendererDataListForLightmaps.AddRange(mappingSerializedLightmapIndexRendererDataList[i]);
                        indexLightmapsRendererMeshCount.Add(rendererDataListForLightmaps.Count);
                    }
                    else
                    {
                        Debug.LogError($"MeshRendererLightmapMapping no list for index {i}");
                        indexLightmapsRendererMeshCount.Add(0);
                    }
                }

                int mixedLightingMode = 0;
                if (Lightmapping.TryGetLightingSettings(out LightingSettings settings))
                {
                    mixedLightingMode = (int)settings.mixedBakeMode;
                }
                else
                {
                    Debug.LogError("Cannot save mixedLightingMode because LightingSettings as not be found");
                }
                // Cache lights lightmap data
                lightmapLights.Clear();
                foreach (var lightReference in lightingGroup.lightReferences)
                {
                    lightmapLights.Add(new LightmapLightData(lightReference, mixedLightingMode));
                }
            }

            // Export and cache lightProbeVolume data
            lightProbeVolumes.Clear();
            foreach (var lightProbeVolumeReference in lightingGroup.lightProbeVolumeReferences)
            {
                lightProbeVolumeReference.lightProbeVolume.Export3DTextures(destinationFolderPath);
                lightProbeVolumes.Add(new LightProbeVolumeData(lightProbeVolumeReference));
                lightProbeVolumeReference.lightProbeVolume.SHAr = null;
                lightProbeVolumeReference.lightProbeVolume.SHAg = null;
                lightProbeVolumeReference.lightProbeVolume.SHAb = null;
                lightProbeVolumeReference.lightProbeVolume.occ = null;
                EditorUtility.SetDirty(lightProbeVolumeReference.lightProbeVolume);
            }

            // Export and cache reflectionProbe texture
            List<ReflectionProbeData> newReflectionProbes = new List<ReflectionProbeData>();
            List<string> sourceTexturePaths = new List<string>();
            foreach (var reflectionProbeReference in lightingGroup.reflectionProbeReferences)
            {
                if (saveBakedReflectionProbe && reflectionProbeReference.reflectionProbe.mode == ReflectionProbeMode.Baked)
                {
                    if (TryGetReflectionProbeData(reflectionProbeReference.reflectionProbe.bakedTexture, out ReflectionProbeData reflectionProbeData))
                    {
                        newReflectionProbes.Add(reflectionProbeData);
                    }
                    else
                    {
                        reflectionProbeReference.reflectionProbe.bakedTexture = CopyFile(reflectionProbeReference.reflectionProbe.bakedTexture, destinationFolderPath, sourceTexturePaths);
                        newReflectionProbes.Add(new ReflectionProbeData(reflectionProbeReference));
                    }
                }
                else
                {
                    //if its not being saved, get its source and add it to the list of source textures
                    if (reflectionProbeReference.reflectionProbe.bakedTexture != null)
                    {
                        string sourceFilePath = AssetDatabase.GetAssetPath(reflectionProbeReference.reflectionProbe.bakedTexture);
                        sourceTexturePaths.Add(sourceFilePath);
                    }
                }
            }
            reflectionProbes = newReflectionProbes;
            List<string> failedPaths2 = new List<string>();
            string[] strings = sourceTexturePaths.ToArray();
            Debug.Log($"Deleting reflection probe textures at: \n{string.Join("\n", strings)}");
            AssetDatabase.DeleteAssets(strings, failedPaths2);

            // Save light probes if no volume found
            if (lightProbeVolumes.Count == 0)
            {
                if (LightmapSettings.lightProbes.bakedProbes != null) bakedProbes = new List<SphericalHarmonicsL2>(LightmapSettings.lightProbes.bakedProbes);
                if (LightmapSettings.lightProbes.positions != null) probePositions = new List<Vector3>(LightmapSettings.lightProbes.positions);
            }

            // Clean unused lightmaps and reflection probes textures in preset folder
            List<string> toDeleteTexturePaths = new List<string>();
            string[] destinationFolderfiles = Directory.GetFiles(destinationFolderPath, "*.exr", SearchOption.TopDirectoryOnly);
            foreach (var file in destinationFolderfiles)
            {
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(file);
                if (texture is Texture2D && !TryGetSerializedLightmapData(texture as Texture2D, out SerializedLightmapData serializedLightmapData))
                {
                    toDeleteTexturePaths.Add(file);
                }
                else if (texture is Cubemap && !TryGetReflectionProbeData(texture, out ReflectionProbeData reflectionProbeData))
                {
                    toDeleteTexturePaths.Add(file);
                }
            }
            if (toDeleteTexturePaths.Count > 0)
            {
                AssetDatabase.DeleteAssets(toDeleteTexturePaths.ToArray(), failedPaths);
                Debug.Log($"Removed {toDeleteTexturePaths.Count} unused texture from {destinationFolderPath}");
            }
			//rename the asset names so they containe the preset name
            UpdateAssetNames();
            if (lightmapsUpdated) lastSave = DateTime.Now;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }


        protected T CopyFile<T>(T obj, string destinationFolderPath, List<string> sourceTexturePaths) where T : UnityEngine.Object
        {
            if (obj == null) return null;
            string sourceFilePath = AssetDatabase.GetAssetPath(obj);
            sourceTexturePaths.Add(sourceFilePath);
            string sourceFileName = Path.GetFileName(sourceFilePath);
            string presetName = GetPresetName();
            string fileName = $"{presetName}_{sourceFileName}";
            string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(destinationFilePath) == obj)
            {
                // Source file is same as destination file, no move needed
                return obj;
            }
            AssetDatabase.CopyAsset(sourceFilePath, destinationFilePath);
            return AssetDatabase.LoadAssetAtPath<T>(destinationFilePath);
        }

        public static bool IsLightmapUseCorrectFormatForAndroid(Texture2D lightmap)
        {
            string assetPath = AssetDatabase.GetAssetPath(lightmap);
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            if ((assetImporter as TextureImporter).mipmapEnabled == true) return false;
            TextureImporterPlatformSettings textureImporterPlatformSettings = (assetImporter as TextureImporter).GetPlatformTextureSettings("Android");
            if (textureImporterPlatformSettings.overridden == false) return false;
            if (textureImporterPlatformSettings.format != LightmapBakeHelper.defaultAndroidLightmapColorFormat) return false;
            return true;
        }

        public static void SetLightmapFormatForAndroid(Texture2D lightmap)
        {
            string assetPath = AssetDatabase.GetAssetPath(lightmap);
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            // Set mipmap to false (fix lightmap issue on android lower resolution)
            if (assetImporter is TextureImporter textureImporter)
            {
                textureImporter.mipmapEnabled = false;
                TextureImporterPlatformSettings textureImporterPlatformSettings = textureImporter.GetPlatformTextureSettings("Android");
                textureImporterPlatformSettings.overridden = true;
                // Set a texture format that don't cause color artifacts on android
                textureImporterPlatformSettings.format = LightmapBakeHelper.defaultAndroidLightmapColorFormat;
                // Prevent lightmap to be resized, causing light bleeding on edges
                textureImporterPlatformSettings.maxTextureSize = 4096;
                textureImporter.SetPlatformTextureSettings(textureImporterPlatformSettings);
            
                EditorUtility.SetDirty(assetImporter);
                assetImporter.SaveAndReimport();
            }
        }

        protected void SetLightmapFormatForDirectional(Texture2D lightmap)
        {
            string assetPath = AssetDatabase.GetAssetPath(lightmap);
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            TextureImporterPlatformSettings textureImporterPlatformSettings = (assetImporter as TextureImporter).GetPlatformTextureSettings("Android");
            textureImporterPlatformSettings.overridden = true;
            textureImporterPlatformSettings.format = LightmapBakeHelper.defaultLightmapDirectionalFormat;
            (assetImporter as TextureImporter).SetPlatformTextureSettings(textureImporterPlatformSettings);
            EditorUtility.SetDirty(assetImporter);
            assetImporter.SaveAndReimport();
        }

        public void GetSceneSettings()
        {
            GetRenderSettings();
            GetBakeSettings();
            GetCloudSettings();
            GetDirLightSettings();
            GetSkyboxSettings();
            GetFogSettings();
        }

        [Button(ButtonSizes.Small,"Current Settings"), Group("general")] 
        protected void GetRenderSettings()
        {
            ambientIntensity = RenderSettings.ambientIntensity;
            shadowColor = RenderSettings.subtractiveShadowColor;
            EditorUtility.SetDirty(this);
        }

        [Button(ButtonSizes.Small,"Current Settings"), Group("baking")] 
        protected void GetBakeSettings()
        {
#if UNITY_EDITOR            
            bakedShadowAngle = RenderSettings.sun.shadowAngle;
            multipleImportanceSamplingTool = Lightmapping.lightingSettings.sampling;
            directSamples = Lightmapping.lightingSettings.directSampleCount;
            indirectSamples = Lightmapping.lightingSettings.indirectSampleCount;
            environmentSamples = Lightmapping.lightingSettings.environmentSampleCount;
            minBounces = Lightmapping.lightingSettings.minBounces;
            maxBounces = Lightmapping.lightingSettings.maxBounces;
            indirectIntensity = Lightmapping.lightingSettings.indirectScale;
            AOIndirectContribution = Lightmapping.lightingSettings.aoExponentIndirect;
            AODirectContribution = Lightmapping.lightingSettings.aoExponentDirect;
            EditorUtility.SetDirty(this);
#endif            
        }

        [Button(ButtonSizes.Small,"Current Settings"), Group("directionalLight")]
        protected void GetDirLightSettings()
        {
            if (RenderSettings.sun)
            {
                dirLightColor = RenderSettings.sun.color;
                dirLightIntensity = RenderSettings.sun.intensity;
                dirLightIndirectMultiplier = RenderSettings.sun.bounceIntensity;
                if (TryGetLightingGroupInScene(out LightingGroup lightingGroup))
                {
                    Debug.LogFormat(lightingGroup, "Get local rotation using the lightingGroup " + lightingGroup.name + " in the scene as reference");
                    directionalLightLocalRotation = lightingGroup.transform.InverseTransformRotation(RenderSettings.sun.transform.rotation);
                }
                else
                {
                    Debug.Log("Could not get local rotation as the scene have no lightingGroup, using world rotation instead");
                    directionalLightLocalRotation = RenderSettings.sun.transform.rotation;
                }
                EditorUtility.SetDirty(this);
            }
            else
            {
                Debug.LogError("Scene have no sun source set in lighting parameters");
            }
        }

        [Button(ButtonSizes.Small,"Current Settings"), Group("skybox")]
        protected void GetSkyboxSettings()
        {
            Material skyBoxMaterial = RenderSettings.skybox;
            skyBoxMaterialProp = SerializedMaterialProperties.CreateProperties(skyBoxMaterial);

            EditorUtility.SetDirty(this);
        }

        [Button(ButtonSizes.Small,"Current Settings"), Group("clouds")]
        protected void GetCloudSettings()
        {
            if (Clouds.instance && Clouds.instance.meshRenderer)
            {
                if (Clouds.instance.meshRenderer.sharedMaterial.HasProperty("_CloudSoftness"))
                {
                    cloudsSoftness = Clouds.instance.meshRenderer.sharedMaterial.GetFloat("_CloudSoftness");
                }
                if (Clouds.instance.meshRenderer.sharedMaterial.HasProperty("_Speed"))
                {
                    cloudsSpeed = Clouds.instance.meshRenderer.sharedMaterial.GetFloat("_Speed");
                }
                if (Clouds.instance.meshRenderer.sharedMaterial.HasProperty("_Size"))
                {
                    cloudsSize = Clouds.instance.meshRenderer.sharedMaterial.GetFloat("_Size");
                }
                if (Clouds.instance.meshRenderer.sharedMaterial.HasProperty("_Color"))
                {
                    cloudsColor = Clouds.instance.meshRenderer.sharedMaterial.GetColor("_Color");
                }
                if (Clouds.instance.meshRenderer.sharedMaterial.HasProperty("_Alpha"))
                {
                    cloudsAlpha = Clouds.instance.meshRenderer.sharedMaterial.GetFloat("_Alpha");
                }

                EditorUtility.SetDirty(this);
            }
        }

        [Button(ButtonSizes.Small,"Current Settings"), Group("fog")]
        protected void GetFogSettings()
        {
            fogColor = RenderSettings.fogColor;
            fogStartDistance = RenderSettings.fogStartDistance;
            fogEndDistance = RenderSettings.fogEndDistance;
            EditorUtility.SetDirty(this);
        }
#endif
        #endregion Methods
    }
}