using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor.Build.Reporting;
using UnityEngine.Rendering;
using Ionic.Zip;
using UnityEditor.Build;
using UnityEditor.Rendering;

namespace ThunderRoad
{
    public class AssetBundleBuilder
    {
        public static bool stripGameIncludedShaderVariantsOnBuild;

        public static string exportFolderName;

        public delegate void BuildEvent(EventTime eventTime);
        public static event BuildEvent OnBuildEvent;

        public static string androidAssetFolderName = "assets";

        public static string assetsLocalPath
        {
            get
            {
                string buildtarget = "Windows";
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildtarget = "Android";
                return (Path.Combine(GameSettings.instance.addressableEditorPath, buildtarget, exportFolderName));
            }
        }

        public static string catalogLocalPath
        {
            get
            {
                return (Path.Combine(GameSettings.instance.catalogsEditorPath, exportFolderName));
            }
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                // Cleanup addressable groups
                for (int i = AddressableAssetSettingsDefaultObject.Settings.groups.Count - 1; i >= 0; i--)
                {
                    if (AddressableAssetSettingsDefaultObject.Settings.groups[i] == null)
                    {
                        Debug.Log("Removing missing AA group at index " + i);
                        AddressableAssetSettingsDefaultObject.Settings.groups.RemoveAt(i);
                    }
                }
                foreach (AddressableAssetGroup aaGroup in EditorCommon.GetAllInstances<AddressableAssetGroup>())
                {
                    if (AddressableAssetSettingsDefaultObject.Settings.groups.Contains(aaGroup)) continue;
                    Debug.Log("Adding missing AA group " + aaGroup.name);
                    AddressableAssetSettingsDefaultObject.Settings.groups.Add(aaGroup);
                }

                string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), GameSettings.instance.catalogsEditorPath);
                if (!Directory.Exists(catalogFullPath))
                {
                    Directory.CreateDirectory(catalogFullPath);
                    Debug.Log("Created folder " + catalogFullPath);
                }
                string buildFullPath = Path.Combine(Directory.GetCurrentDirectory(), GameSettings.instance.addressableEditorPath);
                if (!Directory.Exists(buildFullPath))
                {
                    Directory.CreateDirectory(buildFullPath);
                    Debug.Log("Created folder " + buildFullPath);
                }
            }
        }

        public static void CopyAssetsToBuild(string assetFolderPath, string folderName, bool toDefault, string buildPath)
        {
            string buildtarget = "Windows";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildtarget = "Android";

            string assetsFullPath = Path.Combine(assetFolderPath, buildtarget, folderName);
            string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), GameSettings.instance.catalogsEditorPath, folderName);

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                string buildFolderPath = Path.Combine(buildPath, androidAssetFolderName);
                if (Directory.Exists(buildFolderPath)) Directory.Delete(buildFolderPath, true);
                CopyDirectory(assetsFullPath, buildFolderPath);
                Debug.Log("Copied folder " + folderName + " to " + buildFolderPath);

                string jsondbPath = Path.Combine(buildFolderPath, folderName + ".jsondb");
                ZipFile zip = new ZipFile();

                string[] files = Directory.GetFiles(catalogFullPath, "*.json", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    zip.AddFile(file, Path.GetDirectoryName(FileManager.GetRelativePath(Path.Combine(catalogFullPath, folderName), file)));
                }

                zip.Save(jsondbPath);
                Debug.Log("Zipped json " + folderName + " to " + jsondbPath);
            }
            else
            {
                string streamingAssetDefaultPath = Path.Combine(buildPath, PlayerSettings.productName + "_Data/StreamingAssets/Default");
                string streamingAssetModPath = Path.Combine(buildPath, PlayerSettings.productName + "_Data/StreamingAssets/Mods");
                string destFolder = toDefault ? streamingAssetDefaultPath : streamingAssetModPath;

                if (toDefault)
                {
                    CopyDirectory(assetsFullPath, destFolder);
                    Debug.Log("Copied folder " + folderName + " to " + destFolder);
                }
                else
                {
                    CopyDirectory(assetsFullPath, Path.Combine(destFolder, folderName));
                    Debug.Log("Copied folder " + folderName + " to " + Path.Combine(destFolder, folderName));
                }

                string jsondbPath = Path.Combine(destFolder, folderName + ".jsondb");
                ZipFile zip = new ZipFile();

                string[] files = Directory.GetFiles(catalogFullPath, "*.json", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    zip.AddFile(file, Path.GetDirectoryName(FileManager.GetRelativePath(Path.Combine(catalogFullPath, folderName), file)));
                }

                zip.Save(jsondbPath);
                Debug.Log("Zipped json " + folderName + " to " + jsondbPath);
            }
        }

        public static void CopyDirectory(string strSource, string strDestination, string searchPattern = "*.*")
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
            }
            if (!Directory.Exists(strSource))
            {
                Directory.CreateDirectory(strSource);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(strSource);
            FileInfo[] files = dirInfo.GetFiles(searchPattern);
            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name), true);
            }

            DirectoryInfo[] directories = dirInfo.GetDirectories();
            foreach (DirectoryInfo tempdir in directories)
            {
                CopyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name), searchPattern);
            }
        }

        public static void BatchBuild()
        {
            AssetBundleGroup assetBundleGroup = null;
            bool purgeCache = false;
            bool dryRun = false;
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-assetbundlegroup":
                        {
                            Debug.Log("AssetBundleGroup: " + args[i + 1]);
                            foreach (AssetBundleGroup abg in EditorCommon.GetAllProjectAssets<AssetBundleGroup>())
                            {
                                if (abg.name == args[i + 1])
                                {
                                    assetBundleGroup = abg;
                                    break;
                                }
                            }
                            if (!assetBundleGroup)
                            {
                                Debug.LogError("assetBundleGroup: " + args[i + 1] + " , cannot be found");
                            }
                            break;
                        }
                    case "-purgecache":
                        {
                            Debug.Log("Purge Cache");
                            purgeCache = true;
                            break;
                        }
                    case "-dryrun":
                        {
                            Debug.Log("dryrun mode");
                            dryRun = true;
                            break;
                        }
                }
            }


            if (dryRun)
            {
                return;
            }

            if (assetBundleGroup)
            {
                if (Build(assetBundleGroup, purgeCache))
                {
                    EditorApplication.Exit(0);
                }
                else
                {
                    EditorApplication.Exit(1);
                }
            }
            else
            {
                Debug.LogError("-assetBundleGroup parameter is not set");
            }
        }

        public static bool Build(AssetBundleGroup assetBundleGroup, bool purgeCache)
        {
            exportFolderName = assetBundleGroup.folderName;

            Debug.Log("Building asset bundles to destination: " + assetsLocalPath);

            // Configure addressable groups
            List<string> defaultGroups = new List<string>();
            if (AddressableAssetSettingsDefaultObject.Settings != null)
            {
                foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
                {
                    BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                    if (bundledAssetGroupSchema != null)
                    {
                        if (group.Default)
                        {
                            bundledAssetGroupSchema.BuildPath.SetVariableByName(AddressableAssetSettingsDefaultObject.Settings, "LocalBuildPath");
                            bundledAssetGroupSchema.LoadPath.SetVariableByName(AddressableAssetSettingsDefaultObject.Settings, "LocalLoadPath");
                        }
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;

                        bundledAssetGroupSchema.IncludeInBuild = assetBundleGroup.addressableAssetGroups.Contains(group);

                        if (bundledAssetGroupSchema.BuildPath.GetName(AddressableAssetSettingsDefaultObject.Settings) == "DefaultBuildPath")
                        {
                            defaultGroups.Add(group.name);
                        }
                        AddressableAssetSettingsDefaultObject.Settings.OverridePlayerVersion = assetBundleGroup.folderName;
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalBuildPath", "[ThunderRoad.AssetBundleBuilder.assetsLocalPath]");
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalLoadPath", assetBundleGroup.isDefault ? "{ThunderRoad.FileManager.aaDefaultPath}" : ("{ThunderRoad.FileManager.aaModPath}/" + assetBundleGroup.folderName));
                        // Set builtin shader to export folder name to avoid duplicates
                        AddressableAssetSettingsDefaultObject.Settings.ShaderBundleNaming = UnityEditor.AddressableAssets.Build.ShaderBundleNaming.Custom;
                        AddressableAssetSettingsDefaultObject.Settings.ShaderBundleCustomNaming = assetBundleGroup.folderName;
                        AddressableAssetSettingsDefaultObject.Settings.BuildRemoteCatalog = true;
                    }
                }
            }

            SetGraphicSettings(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? true : false);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            // Create build path if doesn't exist
            if (!Directory.Exists(assetsLocalPath))
            {
                Directory.CreateDirectory(assetsLocalPath);
            }

            // Clean build path
            foreach (string filePath in Directory.GetFiles(assetsLocalPath, "*.*", SearchOption.AllDirectories))
            {
                File.Delete(filePath);
            }

            if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnStart);

            if (purgeCache) BuildCache.PurgeCache(false);
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);

            stripGameIncludedShaderVariantsOnBuild = true;
            AddressableAssetSettings.BuildPlayerContent(out var buildResult);
            stripGameIncludedShaderVariantsOnBuild = false;

            if (!string.IsNullOrEmpty(buildResult.Error))
            {
                Debug.LogError("BuildPlayerContent failed");
                return false;
            }

            foreach (AddressableAssetGroup addressableAssetGroup in assetBundleGroup.addressableAssetGroups)
            {
                BundledAssetGroupSchema bundledAssetGroupSchema = addressableAssetGroup.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema) bundledAssetGroupSchema.IncludeInBuild = false;
            }

            Debug.Log("Build done");

            if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnEnd);

            return true;
        }

        public static void SetGraphicSettings(bool android)
        {
            // Configure stereo rendering
            if (android)
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.SinglePass;
            }
            else
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.Instancing;
            }

            if (!AssetBundleBuilderGUI.forceLinearFog)
            {
                return;
            }

            var graphicsSettings = GraphicsSettings.GetGraphicsSettings();
            SerializedObject serializedObject = new SerializedObject(graphicsSettings);
            SerializedProperty lightmapKeepDirCombined = serializedObject.FindProperty("m_LightmapKeepDirCombined");
            SerializedProperty lightmapKeepShadowMask = serializedObject.FindProperty("m_LightmapKeepShadowMask");
            SerializedProperty lightmapKeepSubtractive = serializedObject.FindProperty("m_LightmapKeepSubtractive");

            lightmapKeepDirCombined.boolValue = !android;
            lightmapKeepShadowMask.boolValue = !android;
            lightmapKeepSubtractive.boolValue = android;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(graphicsSettings);
            AssetDatabase.SaveAssets();
        }

        public class ShaderStripPreProcess : IPreprocessShaders
        {
            public int callbackOrder { get { return 0; } }

            public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
            {
                if (!stripGameIncludedShaderVariantsOnBuild) return;

                List<string> sdkShaderNames = new List<string>();
                sdkShaderNames.Add("ThunderRoad/Lit");
                sdkShaderNames.Add("Hidden/Universal Render Pipeline/UberPost");
                sdkShaderNames.Add("Universal Render Pipeline/Particles/Lit_FixedProbeOcclusion");
                sdkShaderNames.Add("Universal Render Pipeline/Particles/Simple Lit_FixedProbeOcclusion");
                sdkShaderNames.Add("ThunderRoad/SkyboxProceduralCustom");
                sdkShaderNames.Add("Universal Render Pipeline/Lit");
                sdkShaderNames.Add("Universal Render Pipeline/Simple Lit");
                sdkShaderNames.Add("Universal Render Pipeline/Unlit");
                sdkShaderNames.Add("Universal Render Pipeline/Particles/Lit");
                sdkShaderNames.Add("Universal Render Pipeline/Particles/Simple Lit");
                sdkShaderNames.Add("Universal Render Pipeline/Particles/Unlit");
                sdkShaderNames.Add("Hidden/Universal Render Pipeline/UberPost");

                if (sdkShaderNames.Contains(shader.name))
                {
                    Debug.Log("Stripped shader variant of [" + shader.name + "] as they are already included in base game SDKShaders bundle");
                    shaderCompilerData.Clear();
                }
            }
        }
    }
}